import * as utils from './utils'

const self = this;
let StudentId = null;

let completedCourses = [];
let incompleteCourses = [];
let completedSearchTimeout = null;
let incompleteSearchTimeout = null;


function makeCard(course) {
    let wrapper = $(`
        <div class="col s4">
            <div class="card grey darken-2">
                <div class="card-content white-text">
                    <span class="card-title"></span>
                    <p></p>
                </div>
                <div class="card-action">
                </div>
            </div>
        </div>
    `);

    const card = wrapper.find('.card');

    card.parent().prop('id', course.id);

    wrapper.find('.card-title').text(course.institution.name);
    wrapper.find('.card-content>p').text(course.name);

    return wrapper;
}

function markNotComplete(courseId) {
    $.ajax({
        url: apiRoot + '/student/' + self.StudentId + '/courses/completed/' + courseId,
        type: 'DELETE',
        complete: fetchCompletedCourses,
        error: checkExpiredSession
    });
}

function markCourseComplete(courseId) {
    $.ajax({
        url: apiRoot + '/student/' + self.StudentId + '/courses/completed/' + courseId,
        type: 'PUT',
        complete: function () {
            fetchCompletedCourses();
            doIncompleteSearch();
        },
        error: checkExpiredSession
    })
}

function setIncompleteCourses(data) {
    const resultsDiv = $('#incomplete-courses');
    resultsDiv.html('');
    
    for (let chunk of utils.chunk(data,  3)) {
        const rowWrapper = $(`<div class="row"></div>`);
        
        for (let course of chunk) {
            const card = makeCard(course);
            const link = $('<a href="#">Mark As Complete</a>');

            link.click(function () {
                markCourseComplete(course.id);
                card.remove();
                utils.focusInput("#incomplete-search");
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }

        resultsDiv.append(rowWrapper);
    }
}

function setCompletedCourses(data) {
    const resultsDiv = $('#completed-courses');
    resultsDiv.html('');
    
    for (let chunk of utils.chunk(data,  3)) {
        const rowWrapper = $(`<div class="row"></div>`);

        for (let course of chunk) {

            const card = makeCard(course);
            const link = $('<a href="#">Mark As Incomplete</a>');

            link.click(function () {
                markNotComplete(course.id);
                $("#completed-search").val('');
                utils.focusInput("#completed-search");
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }

        resultsDiv.append(rowWrapper);

    }
}

function fetchCompletedCourses() {
    $.get(apiRoot + '/student/' + self.StudentId + '/courses/completed')
        .done(function (data) {
            self.completedCourses = data;
            setCompletedCourses(data);
        })
        .fail(function (err) {
            checkExpiredSession(err);
            console.error("broke: ", err)
        });
}

function fetchIncompleteCourses() {
    $.get(apiRoot + '/student/' + self.StudentId + '/courses/incomplete')
        .done(function (data) {
            self.incompleteCourses = data;
            setIncompleteCourses(data);
        })
        .fail(function (err) {
            checkExpiredSession(err);
            console.error("broke: ", err)
        });
}

function doIncompleteSearch() {
    const q = $("#incomplete-search").val();
    
    if (q.length < 3) {
        setIncompleteCourses(self.incompleteCourses);
        return;
    }
    $.get({
        url: apiRoot + '/student/' + self.StudentId + '/courses/incomplete',
        data: { query: q }
    }).done(setIncompleteCourses)
      .fail(function (err) {
          checkExpiredSession(err);
          setIncompleteCourses([]);
          console.error("Failed to search courses");
      });
}

export function init(studentId) {
    this.StudentId = studentId;
    fetchCompletedCourses();
    fetchIncompleteCourses();

    $("#completed-search").on("paste keyup",
        function () {
            const q = $(this).val();

            if (completedSearchTimeout) {
                clearTimeout(completedSearchTimeout);
            }

            completedSearchTimeout = setTimeout(
                function () {
                    if (q.length < 3) {
                        setCompletedCourses(self.completedCourses);
                        return;
                    }
                    $.get({
                        url: apiRoot + '/student/' + self.StudentId + '/courses/completed',
                        data: { query: q }
                    }).done(setCompletedCourses)
                      .fail(function (err) {
                          checkExpiredSession(err);
                          setCompletedCourses([]);
                          console.error("Failed to search courses");
                      });
                },
                250
            );
        }
    );

    $("#incomplete-search").on("paste keyup",
        function () {
            if (incompleteSearchTimeout) {
                clearTimeout(incompleteSearchTimeout);
            }

            incompleteSearchTimeout = setTimeout(
                doIncompleteSearch,
                250
            );
        }
    );
    
    $('#add-completed-courses-dialog').modal({
        ready: function () {
            utils.focusInput("#incomplete-search");
        }
    })
}