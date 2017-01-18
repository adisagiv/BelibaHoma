$(function () {
    var tutorLateCol = $('#late-tutors-last-col').val();
    var interventionCol = $('#intervention-last-col').val();
    var traineeGradeCol = $('#trainee-grade-last-col').val();
    $('#Late-Tutors-table')
        .DataTable({
            scrollY: '50vh',
            scrollCollapse: true,
            "bInfo": false,
            paging: false,
            "order": [[tutorLateCol, "asc"]],
            initComplete: function () {
                this.api()
                    .columns()
                    .every(function () {
                        var column = this;
                        var headerId = $(this.header()).attr('id');

                        if (headerId !== "actions") {
                            var isActiveSelect = false;
                            if (headerId === "is-active") {
                                isActiveSelect = true;
                            }
                            var select = $('<select class="form-control"><option value=""></option></select>')
                                .appendTo($(column.footer()).empty())
                                .on('change',
                                    function () {
                                        var val = $.fn.dataTable.util.escapeRegex(
                                            $(this).val()
                                        );

                                        column
                                            .search(val ? '^' + val + '$' : '', true, false)
                                            .draw();
                                    });

                            column.data()
                                .unique()
                                .sort()
                                .each(function (d, j) {
                                    var option = '<option ';
                                    if (isActiveSelect && d === "פעיל") {
                                        option += ' selected="selected" ';
                                    }
                                    option += 'value="' + d + '">' + d + '</option>';
                                    select.append(option);
                                });
                        }

                    });
            }
        });
    $('#Intervention-Table')
        .DataTable({
            scrollY: '40vh',
            scrollCollapse: true,
            paging: false,
            "bInfo": false,
            "order": [[interventionCol, "asc"]],
            initComplete: function () {
                this.api()
                    .columns()
                    .every(function () {
                        var column = this;
                        var headerId = $(this.header()).attr('id');

                        if (headerId !== "actions") {
                            var isActiveSelect = false;
                            if (headerId === "is-active") {
                                isActiveSelect = true;
                            }
                            var select = $('<select class="form-control"><option value=""></option></select>')
                                .appendTo($(column.footer()).empty())
                                .on('change',
                                    function () {
                                        var val = $.fn.dataTable.util.escapeRegex(
                                            $(this).val()
                                        );

                                        column
                                            .search(val ? '^' + val + '$' : '', true, false)
                                            .draw();
                                    });

                            column.data()
                                .unique()
                                .sort()
                                .each(function (d, j) {
                                    var option = '<option ';
                                    if (isActiveSelect && d === "פעיל") {
                                        option += ' selected="selected" ';
                                    }
                                    option += 'value="' + d + '">' + d + '</option>';
                                    select.append(option);
                                });
                        }

                    });
            }
        });
    $('#Trainee-Grade-Table')
    .DataTable({
        scrollY: '40vh',
        scrollCollapse: true,
        paging: false,
        "bInfo": false,
        "order": [[traineeGradeCol, "asc"]],
        initComplete: function () {
            this.api()
                .columns()
                .every(function () {
                    var column = this;
                    var headerId = $(this.header()).attr('id');

                    if (headerId !== "actions") {
                        var isActiveSelect = false;
                        if (headerId === "is-active") {
                            isActiveSelect = true;
                        }
                        var select = $('<select class="form-control"><option value=""></option></select>')
                            .appendTo($(column.footer()).empty())
                            .on('change',
                                function () {
                                    var val = $.fn.dataTable.util.escapeRegex(
                                        $(this).val()
                                    );

                                    column
                                        .search(val ? '^' + val + '$' : '', true, false)
                                        .draw();
                                });

                        column.data()
                            .unique()
                            .sort()
                            .each(function (d, j) {
                                var option = '<option ';
                                if (isActiveSelect && d === "פעיל") {
                                    option += ' selected="selected" ';
                                }
                                option += 'value="' + d + '">' + d + '</option>';
                                select.append(option);
                            });
                    }

                });
        }
    });


    
});