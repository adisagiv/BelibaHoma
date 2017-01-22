$(function () {
    $('#Late-Tutors-a-table')
        .DataTable({
            scrollY: '32vh',
            scrollCollapse: true,
            paging: false,
            "bInfo": false,
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
    $('#Intervention-a-Table')
        .DataTable({
            scrollY: '32vh',
            scrollCollapse: true,
            paging: false,
            "bInfo": false,
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
    $('#Trainee-Grade-a-Table')
    .DataTable({
        scrollY: '32vh',
        scrollCollapse: true,
        paging: false,
        "bInfo": false,
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