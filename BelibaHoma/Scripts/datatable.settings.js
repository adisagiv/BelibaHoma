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
});