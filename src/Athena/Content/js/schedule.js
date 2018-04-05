import $ from 'jquery';
import 'fullcalendar';

const self = this;

let calendar = null;
let studentId = null;
let isReadOnly = false;

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

export function render() {
    self.calendar.fullCalendar('render');
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
        width: "auto",
        header: false,
        columnFormat: 'ddd',
    });
    
    self.calendar.fullCalendar('next');

    reloadSchedule();
    
    if (!self.isReadOnly)
    {
        // TODO: Fetch available courses
    }
}