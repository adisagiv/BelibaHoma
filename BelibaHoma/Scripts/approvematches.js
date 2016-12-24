$(function () {
    function populateTutorTrainee(selectedArea) {
        // Send the value to the appropiate action $.post
        $.post('/Rackaz/TutorTrainee/ApproveMatchesAreaSelected', { Area: selectedArea }, function (data) {
            // Success function replace the contant of the html in the correct div
            $('#area-partial').html(data);
            //Catch the form submition -
        }).fail(function (data) {
            // Make sure that the data is now a html of the error page
            $('html').html(data.responseText);
        });
    }

    var selectedArea = $('#area-select select').val();
    if (selectedArea) {
        populateTutorTrainee(selectedArea);
    }

    //Catch the select change event
    $('#area-select select').change(function () {
        //alert('changed');
        // Extract the selected value
        var selectedArea = $(this).val();

        populateTutorTrainee(selectedArea);
    });
});