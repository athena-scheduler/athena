import $ from 'jquery';
import * as utils from './utils';
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

let isFirstLoad = true;
let searchTimeout = null;
let loadedCallback = null;

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

            $("#complete-courses-trigger, .print-trigger").prop("disabled", data.length === 0);
            
            for(let ev of data) {
                if (!colorMap[ev.offeringId]) {
                    colorMap[ev.offeringId] = offeringColors[nextColor];
                    nextColor = (nextColor + 1) % offeringColors.length;
                }
                
                ev.color = colorMap[ev.offeringId];
                if (ev.allDay) {
                    ev.start = "0000-01-01";
                    ev.end = "9999-12-31";
                }
                
                self.calendar.renderEvent(ev, false);
            }
            
            if (isFirstLoad) {
                isFirstLoad = false;
                if (loadedCallback) {
                    loadedCallback();
                }
            }
        })
        .fail(function (err) {
            checkExpiredSession(err);
            console.error(err);
            $("#complete-courses-trigger").prop("disabled", true);
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
    
    for (let meeting of offering.meetings.sort(function (a, b) { return a.day - b.day })) {
        if (meeting.external) {
            ul.remove();
            wrapper.find('.card-content').append($(`<p>This course does not have any meetings. It may be an online or self-study course.</p>`));
            break;
        }
        
        const start = timeSpanToMoment(meeting.time).format('h:mm A');
        const end   = timeSpanToMoment(meeting.time).add(moment.duration(meeting.duration)).format('h:mm A');
        
        ul.append($('<li></li>').text(moment.weekdays()[meeting.day] + ': ' + start + ' - ' + end + ' in ' + meeting.room));
    }
    
    return wrapper;
}

function setSearchResults(data) {
    const results = $("#course-search-results");
    results.html('');
    
    for (let chunk of utils.chunk(data,  3))
    {
        const rowWrapper = $(`<div class="row"></div>`);
        
        for (let offering of chunk) {
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
                        reloadAll();
                        $('#course-search').val('');
                        utils.focusInput('#course-search');
                    },
                    error: function (err) {
                        checkExpiredSession(err);
                        
                        const payload = err.responseJSON;
                        if (err.status === 409 && payload.details)
                        {
                            const start = timeSpanToMoment(payload.details.conflictingTimeSlot.Time).format('h:mm A');
                            const end   = timeSpanToMoment(payload.details.conflictingTimeSlot.End).format('h:mm A');

                            const toastContent = $(`
                                <div>
                                    <div style="margin-bottom: 0.25rem">
                                        Unable to enroll in <span class="conflict-target" style="text-decoration: underline"></span>
                                    </div>
                                    <div style="margin-bottom: 0.25rem">
                                        As it conflicts with <span class="conflict-source" style="text-decoration: underline"></span>
                                    </div>
                                    <div style="margin-bottom: 0.25rem">
                                        Between <span class="conflict-time-start"></span> and <span class="conflict-time-end"></span> on <span class="conflict-dow"></span>
                                    </div>
                                </div>`
                            );

                            toastContent.find('.conflict-target').text(offering.course.name);
                            toastContent.find('.conflict-source').text(payload.details.conflict.Course.Name);
                            toastContent.find('.conflict-time-start').text(start);
                            toastContent.find('.conflict-time-end').text(end);
                            toastContent.find('.conflict-dow').text(moment.weekdays()[payload.details.conflictingTimeSlot.Day]);

                            Materialize.Toast.removeAll();
                            Materialize.toast(toastContent, 10000, 'red darken-4 toast-wrap right');
                        } else if (err.status === 412 && payload.details) {
                            const modal = $('#unmet-dependency-error');

                            modal.find('#unmet-target').text(payload.details.course.Name);

                            const unmetConcurrentContainer = modal.find('#unmet-concurrent-container').html('');
                            if (payload.details.unmetConcurrent.length > 0) {
                                const unmetConcurrentList = $('<ul class="browser-default"></ul>');
                                for (let r of payload.details.unmetConcurrent) {
                                    unmetConcurrentList.append($(`<li></li>`).text(r.Name + ' - ' + r.Description));
                                }

                                unmetConcurrentContainer.append($(`<h5>It's requried that you have taken or are scheduled for these courses:</h5>`));
                                unmetConcurrentContainer.append(unmetConcurrentList);
                            }

                            const unmetContainer = modal.find('#unmet-container').html('');
                            if (payload.details.unmet.length > 0) {
                                const unmetList = $('<ul class="browser-default"></ul>');

                                for (let r of payload.details.unmet) {
                                    unmetList.append($(`<li></li>`).text(r.Name + ' - ' + r.Description));
                                }

                                unmetContainer.append($(`<h5>Must be taken first:</h5>`));
                                unmetContainer.append(unmetList);
                            }

                            modal.modal('open');
                        }
                    }
                });
            });

            rowWrapper.append(card);
        }
        
        results.append(rowWrapper);
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
              .fail(function (err) {
                  checkExpiredSession(err);
                  setSearchResults([]);
                  console.error("Failed to search for completed courses")
              })
        },
        250
    );
}

export function reloadAll() {
    $('.tooltipped').tooltip('remove');
    reloadSchedule();
    doSearch();
    
    // Tooltips are re-added in the afterAllRender event for the calendar
}

export function render() {
    self.calendar.render();
}

export function completeSchedule() {
    $.get(apiRoot + '/student/' + self.studentId + '/schedule/complete')
        .done(reloadAll)
        .fail(function (err) {
            checkExpiredSession(err);
            Materialize.toast("Failed to update schedule", 5000, "red darken-4")
        });
}

export function setLoadedCallback(cb) {
    loadedCallback = cb;
}

export function init(studentId, readOnly) {
    self.studentId = studentId;
    self.isReadOnly = readOnly;
    
    $('#schedule-complete-btn').click(completeSchedule);
    
    const calendarDiv = $('#calendar');

    function makeRemoveButton(event) {
        return $(`<span data-target="remove-class-modal" class="right modal-trigger remove-target"><i class="material-icons red-text text-accent-1" style="font-size: 16px;">close</i></span>`)
            .click(function () {
                $("#Remove-Course-Button").attr("data-confirm-id", event.offeringId);
                $("#remove-class-modal").modal('open');
            });
    }

    calendarDiv.fullCalendar({
        defaultView: "agendaWeek",
        minTime: "07:00:00",
        maxTime: "22:00:00",
        allDaySlot: true,
        allDayText: "Independent",
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
                .attr('data-tooltip', event.tooltip);
            if (!readOnly) {
                if (event.allDay) {
                    $(element).find('.fc-title').append(makeRemoveButton(event));
                } else {
                    $(element).find('.fc-time').append(makeRemoveButton(event));
                }
            }
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
        
        const unmetDependencyModal = $("#unmet-dependency-error");
        unmetDependencyModal.modal({
            ready: function () {
                $(this.$overlay[0]).css('z-index', 8000);
                unmetDependencyModal.css('z-index', 9000);
            }
        });
        $('#add-courses').modal({
            ready: function () {
                utils.focusInput('#course-search');
            }
        })
    }
}
