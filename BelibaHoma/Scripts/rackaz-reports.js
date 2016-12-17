$(function () {
    $('#Report').change(function() {
        var report = $(this).val();
        $.post('/Rackaz/Report/' + report, function(data) {
                $('#report-partial').html(data);
            }
        );
    });

});