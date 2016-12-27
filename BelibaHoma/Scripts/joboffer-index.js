$(function() {
    $('#job-offer-table')
        .DataTable({
            initComplete: function () {
                this.api()
                    .columns()
                    .every(function () {
                        var column = this;
                        var isActiveSelect = false;
                        if ($(this.header()).attr('id') === "is-active") {
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


                    });
                var val = 'פעיל';
                this.api().columns(9).search('^' + val + '$', true, false)
                                        .draw();
            }
        });
});