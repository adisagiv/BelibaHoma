$(function () {
    $('#show-matched-trainees').change(function (e) {
        // Extract the selected value
        var selectedValue = $(this).prop("checked");
        var selectedArea = $('#area-select select').val();
        // Send the value to the appropiate action $.post
        $.post('/Rackaz/TutorTrainee/ManualMatchTraineeSelect', { showMatched: selectedValue, area: selectedArea }, function (data) {
            // Success function replace the contant of the html in the correct div
            $('#trainees-partial').html(data);
        }).fail(function (data) {
            // Make sure that the data is now a html of the error page
            $('html').html(data.responseText);
        });
    });

    $('#show-matched-tutors').change(function (e) {
        // Extract the selected value
        var selectedValue = $(this).prop("checked");
        var selectedArea = $('#area-select select').val();
        // Send the value to the appropiate action $.post
        $.post('/Rackaz/TutorTrainee/ManualMatchTutorSelect', { showMatched: selectedValue, area: selectedArea }, function (data) {
            // Success function replace the contant of the html in the correct div
            $('#tutors-partial').html(data);
        }).fail(function (data) {
            // Make sure that the data is now a html of the error page
            $('html').html(data.responseText);
        });
    });
});