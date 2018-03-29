function fetchCourses(studentId) {
    $.get(apiRoot + '/student/' + studentId + '/courses/in-progress')
        .done(function (data) {
            for(let course of data) {
                
            }
        })
        .fail(function (err) {
            console.error(err);
            Materialize.toast('Failed to load courses', 3000, 'red darken-4');
        });
}

export function init(studentId, readonly) {
    const calendar = $('#calendar');
    
    calendar.fullCalendar({
        // put your options and callbacks here
        defaultView: "agendaWeek",
        minTime: "07:00:00",
        maxTime: "22:00:00",
        allDaySlot: false,
        slotDuration: "1:00",
        slotWidth: 2,
        height: "auto",
        header: false,
        columnFormat: 'ddd',
    });
    
    calendar.fullCalendar('next');
    
    fetchCourses(studentId);
}