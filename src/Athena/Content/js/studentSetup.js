let institutionSearchTimeout = null;
let programSearchTimeout = null;

function makeCard(id, title, description) {
    const wrapper = $(`
                    <div class="col xl4 s6">
                        <div class="card grey darken-2 white-text">
                            <div class="card-content">
                                <span class="card-title"></span>
                                <p class="truncate"></p>
                            </div>
                            <div class="card-action">
                                <a href="#" class="unenroll">Unenroll</a>
                                <a href="#" class="enroll">Enroll</a>
                            </div>
                        </div>
                    </div>
                `);

    wrapper.find('.card').parent().prop('id', id);
    wrapper.find('.card-title').text(title);
    wrapper.find('p').text(description);

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
                type: 'PUT'
            });
        });
        card.find('.unenroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/institutions/' + i.id,
                type: 'DELETE'
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
                type: 'PUT'
            });
        });
        card.find('.unenroll').click(function() {
            $.ajax({
                url: apiRoot + '/student/' + studentId + '/programs/' + p.id,
                type: 'DELETE'
            });
        });

        results.append(card);
    }
}

module.exports = {
    init: function (studentId) {
        $.get(apiRoot + "/student/" + studentId + "/institutions")
            .done(function(data) {
                setInstitutionResults(studentId,  data);
            })
            .fail(function(err) {
                setInstitutionResults(studentId, []);
                console.error("Failed to get institutions");
            });

        $.get(apiRoot + "/student/" + studentId + "/programs")
            .done(function(data) {
                setProgramResults(studentId,  data);
            })
            .fail(function(err) {
                setProgramResults(studentId,  []);
                console.error("Failed to get institutions");
            });

        $("#institution-search").on("change paste keyup",
            function() {
                const q = $(this).val();

                if (institutionSearchTimeout) {
                    clearTimeout(institutionSearchTimeout);
                }

                institutionSearchTimeout = setTimeout(
                    function() {
                        if (q.length < 3) {
                            setInstitutionResults(studentId,  []);
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

        $("#program-search").on("change paste keyup",
            function() {
                const q = $(this).val();

                if (programSearchTimeout) {
                    clearTimeout(programSearchTimeout);
                }

                programSearchTimeout = setTimeout(
                    function() {
                        if (q.length < 3) {
                            setProgramResults([]);
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
};