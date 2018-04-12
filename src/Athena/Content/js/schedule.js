import $ from 'jquery';
import 'fullcalendar';
import Materialize from 'materialize-css';
import moment from 'moment';

const self = this;
const offeringColors = [
    "cyan darken-4",
    "red darken-4",
    "pink darken-4",
    "deep-orange darken-4",
    "indigo darken-4",
    "green darken-4",
    "blue darken-4",
];

let nextColor = 0;

let searchTimeout = null;

let calendar = null;
let studentId = null;
let isReadOnly = false;

function timeSpanToMoment(ts) {
    return moment().startOf('day').clone().add(moment.duration(ts));
}

function reloadSchedule() {
    $.get(apiRoot + '/student/' + self.studentId + '/schedule')
        .done(function (data) {
            self.calendar.removeEvents();
            nextColor = 0;
            let colorMap = {};
            
            for(let ev of data) {
                if (!colorMap[ev.offeringId]) {
                    colorMap[ev.offeringId] = offeringColors[nextColor];
                    nextColor = (nextColor + 1) % offeringColors.length;
                }
                
                ev.color = colorMap[ev.offeringId];
                
                self.calendar.renderEvent(ev, false);
            }
        })
        .fail(function (err) {
            console.error(err);
            window.Materialize.toast('Failed to load enrolled courses', 3000, 'red darken-4');
        });
}

function makeCard(offering) {
    const wrapper = $(`
        <div class="col s4">
            <div class="card grey darken-2">
                <div class="card-content white-text">
                    <span class="card-title"></span>
                    <div>
                        <p></p>
                        <ul class="browser-default"></ul>
                    </div>
                </div>
                <div class="card-action">
                    <a href="#" class="enroll">Enroll</a>
                </div>
            </div>
        </div>
    `);

    const card = wrapper.find('.card');

    card.parent().prop('id', offering.id);
    wrapper.find('.card-title')
        .text(offering.course.institution.name)
        .append($('<span class="new badge blue darken-4 white-text right" data-badge-caption><strong></strong></span>'))
        .find('span.badge strong').text(offering.campus.name);
    
    wrapper.find('.card-content>div>p').text(offering.course.name);
    const ul = wrapper.find('.card-content>div>ul');
    
    for (let meeting of offering.meetings) {
        const start = timeSpanToMoment(meeting.time).format('h:mm A');
        const end   = timeSpanToMoment(meeting.time).add(moment.duration(meeting.duration)).format('h:mm A');
        
        ul.append($('<li></li>').text(moment.weekdays()[meeting.day] + ': ' + start + ' - ' + end + ' in ' + meeting.room));
    }
    
    return wrapper;
}

function setSearchResults(data) {
    const results = $("#course-search-results");
    results.html('');
    
    for (let offering of data) {
        const card = makeCard(offering);
        
        card.find('.enroll').click(function () {
            if ($(this).hasClass('.disabled')) {
                return;
            }
            
            $(this).addClass('disabled').attr('disabled', 'disabled');
            $.ajax({
                url: apiRoot + '/student/' + self.studentId + '/offerings/' + offering.id,
                method: 'PUT',
                complete: reloadAll,
                error: function (err) {
                    const payload = err.responseJSON;
                    if (err.status === 409 && payload.details)
                    {
                        const start = timeSpanToMoment(payload.details.conflictingTimeSlot.Time).format('h:mm A');
                        const end   = timeSpanToMoment(payload.details.conflictingTimeSlot.End).format('h:mm A');
                        
                        const toastContent = $(`<div>
                            <div style="margin-bottom: 0.25rem">
                                Unable to enroll in <span class="conflict-target" style="text-decoration: underline"></span>
                            </div>
                            <div style="margin-bottom: 0.25rem">
                                As it conflicts with <span class="conflict-source" style="text-decoration: underline"></span>
                            </div>
                            <div style="margin-bottom: 0.25rem">
                                Between <span class="conflict-time-start"></span> and <span class="conflict-time-end"></span> on <span class="conflict-dow"></span>
                            </div>
                        </div>`);
                        
                        toastContent.find('.conflict-target').text(offering.course.name);
                        toastContent.find('.conflict-source').text(payload.details.conflict.Course.Name);
                        toastContent.find('.conflict-time-start').text(start);
                        toastContent.find('.conflict-time-end').text(end);
                        toastContent.find('.conflict-dow').text(moment.weekdays()[payload.details.conflictingTimeSlot.Day]);
                        
                        Materialize.Toast.removeAll();
                        Materialize.toast(toastContent, 10000, 'amber darken-4 toast-wrap right');
                    }
                }
            });
        });
        
        results.append(card);
    }
}

function doSearch() {
    const q = $('#course-search').val();
    
    if (searchTimeout) {
        clearTimeout(searchTimeout)
    }

    searchTimeout = setTimeout(
        function () {
            if (q.length < 3) {
                setSearchResults([]);
                return;
            }

            $.get({
                url: apiRoot + '/student/' + self.studentId + '/schedule/offerings/available',
                data: { q: q }
            }).done(setSearchResults)
            .fail(function () {
                setSearchResults([]);
                console.error("Failed to search for completeed courses")
            })
        },
        250
    );
}

function reloadAll() {
    $('.tooltipped').tooltip('remove');
    reloadSchedule();
    doSearch();
    
    // Tooltips are re-added in the afterAllRender event for the calendar
}

export function render() {
    self.calendar.render();
}

export function init(studentId, readOnly) {
    self.studentId = studentId;
    self.isReadOnly = readOnly;
    
    const calendarDiv = $('#calendar');

    calendarDiv.fullCalendar({
        defaultView: "agendaWeek",
        minTime: "07:00:00",
        maxTime: "22:00:00",
        allDaySlot: false,
        slotDuration: "00:30",
        slotWidth: 2,
        height: "auto",
        width: "auto",
        header: false,
        columnFormat: 'ddd',
        eventRender: function (event, element, view) {
            $(element).addClass('tooltipped')
                .addClass(event.color)
                .attr('data-position', 'bottom')
                .attr('data-delay', 25)
                .attr('data-tooltip', event.title)
                .find('.fc-time').append(
                    $(`<span class="right hidden-print"><i class="material-icons red-text text-accent-1" style="font-size: 16px;">close</i></span>`)
                        .click(function () {
                            $.ajax({
                                url: apiRoot + '/student/' + self.studentId + '/offerings/' + event.offeringId,
                                method: 'DELETE',
                                complete: reloadAll
                            });
                        })
                );
        },
        eventAfterAllRender: function (view) {
            $('.tooltipped').tooltip();
        }
    });
    
    self.calendar = calendarDiv.fullCalendar('getCalendar');
    
    self.calendar.next();

    reloadSchedule();
    
    if (!self.isReadOnly)
    {
        $('#course-search').on("paste keyup", doSearch);
    }
}
