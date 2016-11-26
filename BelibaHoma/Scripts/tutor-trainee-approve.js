$(function() {
    $('.approve-match').click(function() {
        var tutortraineeid = $(this).data('tutortraineeid');
        $.post("/Rackaz/TutorTrainee/ApproveSpecificMatch/" + tutortraineeid, function (data) {
            alert("הציוות אושר בהצלחה");
            $('#area-partial').html(data);
        })
        .fail(function(data) {
            // Make sure that the data is now a html of the error page
            $('html').html(data.responseText);
        });
    });

    $('.decline-match').click(function () {
        var tutortraineeid = $(this).data('tutortraineeid');
        $.post("/Rackaz/TutorTrainee/RemoveSpecificMatch/" + tutortraineeid, function (data) {
            alert("ההמלצה לציוות הוסרה בהצלחה");
            $('#area-partial').html(data);
        })
        .fail(function (data) {
            // Make sure that the data is now a html of the error page
            $('html').html(data.responseText);
        });
    });
});