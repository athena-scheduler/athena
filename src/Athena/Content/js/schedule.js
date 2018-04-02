import $ from 'jquery';
import 'fullcalendar';

const self = this;

let calendar = null;
let studentId = null;
let isReadOnly = false;

function fetchAvailableCourses() {
    $.get(apiRoot + '/student/' + self.studentId + '/offerings/available')
        .done(function (data) {
            for (let course of data)
            {
                console.log(course);
            }
        })
        .fail(function (err) {
            console.error(err);
            window.Materialize.toast('Failed to load available courses', 3000, 'red darken-4');
        })
}

function reloadSchedule() {
    $.get(apiRoot + '/student/' + self.studentId + '/schedule')
        .done(function (data) {
            for(let ev of data) {
                console.log("Got an event:", ev);
                self.calendar.fullCalendar('renderEvent', ev, false);
            }
        })
        .fail(function (err) {
            console.error(err);
            window.Materialize.toast('Failed to load enrolled courses', 3000, 'red darken-4');
        });
}

export function init(studentId, readOnly) {
    self.studentId = studentId;
    self.isReadOnly = readOnly;
    
    self.calendar = $('#calendar');
    
    self.calendar.fullCalendar({
        defaultView: "agendaWeek",
        minTime: "07:00:00",
        maxTime: "22:00:00",
        allDaySlot: false,
        slotDuration: "01:00",
        slotWidth: 2,
        height: "auto",
        header: false,
        columnFormat: 'ddd',
    });
    
    self.calendar.fullCalendar('next');

    reloadSchedule();
    
    if (!self.isReadOnly)
    {
        fetchAvailableCourses();
    }
}