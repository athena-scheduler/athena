let institutionSearchTimeout = null;
let programSearchTimeout = null;

let enrolledInstitutions = [];
let enrolledPrograms = [];

function updateEnrolledInstitutions(studentId) {
    $.get(apiRoot + "/student/" + studentId + "/institutions")
        .done(function(data) {
            enrolledInstitutions = data;
            setInstitutionResults(studentId,  enrolledInstitutions);
        })
        .fail(function(err) {
            setInstitutionResults(studentId, []);
            console.error("Failed to get institutions");
        });
    
    $("#institution-search").val("");
}

function updateEnrolledPrograms(studentId) {
    $.get(apiRoot + "/student/" + studentId + "/programs")
        .done(function(data) {
            enrolledPrograms = data;
            setProgramResults(studentId,  enrolledPrograms);
        })
        .fail(function(err) {
            setProgramResults(studentId,  []);
            console.error("Failed to get institutions");
        });
    
    $("#program-search").val("");
}

function makeCard(id, title, description) {
    const wrapper = $(`
                    <div class="col xl4 s6">
                        <div class="card grey darken-2 white-text">
                            <div class="card-content">
                                <span class="card-title activator"></span>
                            </div>
                            <div class="card-reveal grey darken-2 white-text">
                                <span class="card-title"></span>
                                <p></p>
                            </div>
                            <div class="card-action">
                                <a href="#" class="unenroll">Unenroll</a>
                                <a href="#" class="enroll">Enroll</a>
                            </div>
                        </div>
                    </div>
                `);

    const card = wrapper.find('.card');
    
    card.parent().prop('id', id);
    wrapper.find('.card-title').text(title);
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

function setInstitutionResults(studentId, data) {
    const results = $("#institution-results");

    results.html("");
    for (let i of data) {
        const card = makeCard(i.id, i.name, i.description);

        card.find('.enroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/institutions/' + i.id,
                type: 'PUT',
                complete: function() {
                    updateEnrolledInstitutions(studentId);
                }
            })
        });
        card.find('.unenroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/institutions/' + i.id,
                type: 'DELETE',
                complete: function() {
                    updateEnrolledInstitutions(studentId);
                }
            });
        });

        results.append(card);
    }
}

function setProgramResults(studentId, data) {
    const results = $("#program-results");

    results.html("");
    for (let p of data) {
        const card = makeCard(p.id, p.name, p.description);

        card.find('.enroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/programs/' + p.id,
                type: 'PUT',
                complete: function() {
                    updateEnrolledPrograms(studentId);
                }
            });
        });
        card.find('.unenroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/programs/' + p.id,
                type: 'DELETE',
                complete: function() {
                    updateEnrolledPrograms(studentId);
                }
            });
        });

        results.append(card);
    }
}

export function init (studentId) {
    updateEnrolledInstitutions(studentId);
    updateEnrolledPrograms(studentId);

    $("#institution-search").on("paste keyup",
        function() {
            const q = $(this).val();

            if (institutionSearchTimeout) {
                clearTimeout(institutionSearchTimeout);
            }

            institutionSearchTimeout = setTimeout(
                function() {
                    if (q.length < 3) {
                        setInstitutionResults(studentId,  enrolledInstitutions);
                        return;
                    }

                    $.get({
                        url: apiRoot + "/institution",
                        data: { q: q }
                    }).done(function(data){
                        setInstitutionResults(studentId, data)
                    }).fail(function() {
                        setInstitutionResults(studentId,  []);
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
                        setProgramResults(enrolledPrograms);
                        return;
                    }

                    $.get({
                        url: apiRoot + "/program",
                        data: { q: q }
                    }).done(function(data) {
                        setProgramResults(studentId, data)
                    }).fail(function(data) {
                        setProgramResults(studentId, []);
                        console.error("Failed to search programs");
                    });
                },
                250
            );
        }
    );
}