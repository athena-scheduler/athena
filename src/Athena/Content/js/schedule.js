import $ from 'jquery';
import 'fullcalendar';
import 'materialize-css';
import moment from 'moment';

const self = this;

let searchTimeout = null;

let calendar = null;
let studentId = null;
let isReadOnly = false;

function reloadSchedule() {
    self.calendar.removeEvents();
    
    $.get(apiRoot + '/student/' + self.studentId + '/schedule')
        .done(function (data) {
            for(let ev of data) {
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
        const startOfDay = moment().startOf('day');
        
        const start = startOfDay.clone().add(moment.duration(meeting.time)).format('h:mm A');
        const end   = startOfDay.clone().add(moment.duration(meeting.time)).add(moment.duration(meeting.duration)).format('h:mm A');
        
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
                complete: function () {
                    reloadSchedule();
                    doSearch();
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
                .attr('data-position', 'bottom')
                .attr('data-delay', 25)
                .attr('data-tooltip', event.title);
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
