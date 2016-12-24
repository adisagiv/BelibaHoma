$(function () {
    // setting defualt language to hebrew
    $.ajax({
        type: "GET",
        url: "/Scripts/datatable/language/hebrew.json",
        dataType: "json",
        async: false,
        success: function (result) {
            $.fn.DataTable.defaults.oLanguage = $.extend({}, $.fn.DataTable.defaults.oLanguage, result);
        }
    });

    $.extend($.fn.DataTable.ext.classes, {
        sWrapper: "dataTables_wrapper form-inline dt-bootstrap",
        sFilterInput: "form-control input-sm",
        sLengthSelect: "form-control input-sm",
        sProcessing: "dataTables_processing panel panel-default"
    });
});