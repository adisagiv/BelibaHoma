$(function () {
    function getReport(report) {
        
        $.post('/Rackaz/Report/' + report, function (data) {
            $('#report-partial').html(data);
        }
        );
    }

    $('#Report').change(function () {
        var report = $(this).val();
        getReport(report);
    });

    var report = $('#Report').val();
    getReport(report);
});