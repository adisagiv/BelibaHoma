$(function () {
    $('#tutortrainee-table')
         .DataTable({
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
                 var val = 'פעיל';
                 var column = $("#column-value").val();
                 this.api().columns(4).search('^' + val + '$', true, false)
                                         .draw();
             }
         });
    $("#tutortrainee-table").on('click', '.change-tutor-trainee-status' , function (event) {
        var ttId = $(this).data("tutortraineeid");
        $.post("/Rackaz/TutorTrainee/ChangeStatus", { tutorTraineeId: ttId }, function (status) {
            if (status.Success) {
                window.location.reload();
            } else {
                alert(status.Message);
            }
        }).fail(function (data) {
            // Make sure that the data is now a html of the error page
            $('div.container.body-content').html(data.responseText);
        });;
    });
});