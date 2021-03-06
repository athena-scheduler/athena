import * as utils from './utils';

let institutionSearchTimeout = null;
let programSearchTimeout = null;

let enrolledInstitutions = [];
let enrolledPrograms = [];

function updateEnrolledInstitutions(studentId, focus) {
    $.get(apiRoot + "/student/" + studentId + "/institutions")
        .done(function(data) {
            enrolledInstitutions = data;
            setInstitutionResults(studentId, enrolledInstitutions);
        })
        .fail(function (err) {
            checkExpiredSession(err);
            setInstitutionResults(studentId, []);
            console.error("Failed to get institutions");
        });
    
    if (focus) {
        $("#institution-search").val("");
        utils.focusInput("#institution-search");    
    }
}

function updateEnrolledPrograms(studentId, focus) {
    $.get(apiRoot + "/student/" + studentId + "/programs")
        .done(function(data) {
            enrolledPrograms = data;
            setProgramResults(studentId,  enrolledPrograms);
        })
        .fail(function(err) {
            checkExpiredSession(err);
            setProgramResults(studentId,  []);
            console.error("Failed to get Programs");
        });
    
    if (focus) {
        $("#program-search").val("");
        utils.focusInput("#program-search");
    }
}

function makeCard(id, title, description) {
    const wrapper = $(`
        <div class="col xl4 s6">
            <div class="card grey darken-2 white-text">
                <div class="card-content">
                    <span class="card-title activator"> 
                        <span class="card-title-target activator"> </span>
                         <i class="material-icons right">info</i>
                    </span>
                </div>
                <div class="card-reveal grey darken-2 white-text">
                   <span class="card-title"> 
                        <span class="card-title-target"></span>
                        <i class="material-icons right">close</i>
                    </span>
                    <p></p>
                </div>
                <div class="card-action">
                </div>
            </div>
        </div>
    `);

    const card = wrapper.find('.card');
    
    card.parent().prop('id', id);
    wrapper.find('.card-title-target').text(title);
    wrapper.find('.card-reveal>p').text(description);
    wrapper.find('.card-title').click(function () {
        // This fires before materialize reveals the card so the statement is backwards
        if (card.find('.card-reveal').css('display') !== 'none') {
            card.animate({height: card._restoreHeight}, 250);
        } else {
            card._restoreHeight = card.height().toString() + "px";
            card.animate({height: 300}, 250);
        }
    });

    return wrapper;
}

function setInstutitonSearchResults(studentId, data) {
    const results = $("#institution-results");

    results.html("");
    
    for (let chunk of utils.chunk(data,  3)) {
        const rowWrapper = $(`<div class="row"></div>`);
        
        for (let i of chunk) {
            const card = makeCard(i.id, i.name, i.description);
            const link = $('<a href="#">Enroll</a>');

            link.click(function () {
                $.ajax({
                    url: apiRoot + '/student/' + studentId + '/institutions/' + i.id,
                    type: 'PUT',
                    complete: function () {
                        updateEnrolledInstitutions(studentId, true);
                    },
                    error: checkExpiredSession
                });
                card.remove();
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }

        results.append(rowWrapper);
    }
}

function setInstitutionResults(studentId, data) {
    const results = $("#institution-results");

    results.html('');
    
    for (let chunk of utils.chunk(data, 3)) {
        const rowWrapper = $(`<div class="row"></div>`);

        for (let i of data) {
            const card = makeCard(i.id, i.name, i.description);
            const link = $('<a href="#">Unenroll</a>');

            link.click(function () {
                $.ajax({
                    url: apiRoot + '/student/' + studentId + '/institutions/' + i.id,
                    type: 'DELETE',
                    complete: function () {
                        updateEnrolledInstitutions(studentId, true);
                    },
                    error: checkExpiredSession
                });
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }
        
        results.append(rowWrapper);
    }
}

function setProgramResults(studentId, data) {
    const results = $("#program-results");

    results.html("");
    
    for (let chunk of utils.chunk(data, 3)) {
        const rowWrapper = $(`<div class="row"></div>`);
        
        for (let p of chunk) {
            const card = makeCard(p.id, p.name, p.description);
            const link = $('<a href="#">Unenroll</a>');

            link.click(function () {
                $.ajax({
                    url: apiRoot + '/student/' + studentId + '/programs/' + p.id,
                    type: 'DELETE',
                    complete: function() {
                        updateEnrolledPrograms(studentId, true);
                    },
                    error: checkExpiredSession
                });
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }
        
        results.append(rowWrapper);
    }
    
}

function setProgramSearchResults(studentId, data) {
    const results = $("#program-results");

    results.html("");
    for (let chunk of utils.chunk(data,  3)) {
        const rowWrapper = $(`<div class="row"></div>`);

        for (let p of chunk) {
            const card = makeCard(p.id, p.name, p.description);
            const link = $('<a href="#">Enroll</a>');

            link.click(function () {
                $.ajax({
                    url: apiRoot + '/student/' + studentId + '/programs/' + p.id,
                    type: 'PUT',
                    complete: function () {
                        updateEnrolledPrograms(studentId, true);
                    },
                    error: checkExpiredSession
                });
                card.remove();
            });

            card.find('.card-action').append(link);
            rowWrapper.append(card);
        }
        
        results.append(rowWrapper);
    }
}

export function init (studentId) {
    updateEnrolledInstitutions(studentId, false);
    updateEnrolledPrograms(studentId, false);

    $("#institution-search").on("paste keyup",
        function() {
            const q = $(this).val();

            if (institutionSearchTimeout) {
                clearTimeout(institutionSearchTimeout);
            }

            institutionSearchTimeout = setTimeout(
                function() {
                    if (q.length < 3) {
                        setInstitutionResults(studentId, enrolledInstitutions);
                        return;
                    }

                    $.get({
                        url: apiRoot + "/institution",
                        data: { q: q }
                    }).done(function(data){
                        setInstutitonSearchResults(studentId, data)
                    }).fail(function(err) {
                        checkExpiredSession(err);
                        setInstutitonSearchResults(studentId,  []);
                        console.error("Failed to search institutions");
                    });
                },
                250
            );
        }
    );

    $("#program-search").on("paste keyup",
        function() {
            const q = $(this).val();

            if (programSearchTimeout) {
                clearTimeout(programSearchTimeout);
            }

            programSearchTimeout = setTimeout(
                function() {
                    if (q.length < 3) {
                        setProgramResults(studentId,  enrolledPrograms);
                        return;
                    }
                    
                    $.get({
                        url: apiRoot + "/program",
                        data: {
                            q: q,
                            student: studentId
                        }
                    }).done(function(data) {
                        setProgramSearchResults(studentId, data)
                    }).fail(function(err) {
                        checkExpiredSession(err);
                        setProgramSearchResults(studentId, []);
                        console.error("Failed to search programs");
                    });
                },
                250
            );
        }
    );
}