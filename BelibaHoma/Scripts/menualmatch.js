﻿$(function () {


    function populateTutorTrainee(selectedArea) {

        // Send the value to the appropiate action $.post
        $.post('/Rackaz/TutorTrainee/ManualMatchAreaSelected', { Area: selectedArea }, function (data) {
            // Success function replace the contant of the html in the correct div
            $('#area-partial').html(data);
            //Catch the form submition -
            $('#manual-match-submit').click(function (e) {
                e.preventDefault();
                var selectedTrainee = $('input[name="ChooseTrainee"]:checked').val();
                var selectedTutor = $('input[name="ChooseTutor"]:checked').val();
                if (selectedTrainee == undefined || selectedTutor == undefined) {
                    alert("נא לבחור חניך וחונך לציוות");
                } else {
                    var model =
                    {
                        Id: undefined,
                        TutorId: undefined,
                        TraineeId: undefined,
                        Status: undefined,
                        Trainee: undefined,
                        Tutor: undefined
                    }

                    model.TutorId = selectedTutor;
                    model.TraineeId = selectedTrainee;

                    var traineeMatchedCount = $('#trainee-matched-count-' + selectedTrainee).val();
                    var tutorMatchedCount = $('#tutor-matched-count-' + selectedTutor).val();
                    var traineeUnApproved = $('#trainee-unapproved-' + selectedTrainee).val();
                    var tutorUnApproved = $('#tutor-unapproved-' + selectedTutor).val();

                    var message = "";
                    var message2 = "";
                    var messageEnding = "האם ברצונכם להמשיך?";
                    var confirmatonrequired1 = false;
                    var confirmatonrequired2 = false;

                    if (traineeMatchedCount > 0 && tutorMatchedCount > 0) {
                        message = "שימו לב! לחניך הנבחר קיימים " + traineeMatchedCount + " קשרי חונכות ולחונך קיימים " + tutorMatchedCount + " קשרים.";
                        confirmatonrequired1 = true;
                    } else if (traineeMatchedCount > 0) {
                        message = "שימו לב! לחניך הנבחר קיימים " + traineeMatchedCount + " קשרי חונכות.";
                        confirmatonrequired1 = true;

                    } else if (tutorMatchedCount > 0) {
                        message = "שימו לב! לחונך הנבחר קיימים " + tutorMatchedCount + " קשרי חונכות.";
                        confirmatonrequired1 = true;
                    }
                    if (traineeUnApproved > 0 && tutorUnApproved > 0) {
                        message2 = "שימו לב! לחניך ולחונך שנבחרו קיימות המלצות לציוות במערכת";
                        confirmatonrequired2 = true;
                    } else if (traineeUnApproved > 0) {
                        message2 = "שימו לב! לחניך שנבחר קיימות המלצות לציוות במערכת";
                        confirmatonrequired2 = true;

                    } else if (tutorUnApproved > 0) {
                        message2 = "שימו לב! לחונך שנבחר קיימות המלצות לציוות במערכת";
                        confirmatonrequired2 = true;
                    }
                    message += "\n" + messageEnding;
                    message2 += "\n" + messageEnding;

                    if (confirmatonrequired1 === true) {
                        if (confirm(message)) {
                            //first confirm
                            if (confirmatonrequired2 === true) {
                                if (confirm(message2)) {
                                    //second confirm
                                    $.post("/Rackaz/TutorTrainee/ManualMatchCreate", model, function (data) {
                                        alert(data.Message);
                                        if (data.Success) {
                                            window.location.href = "/Rackaz/TutorTrainee/ManualMatch";
                                        }
                                    });
                                }
                            } else {
                                //no second confirmation required, first was confirmed
                                $.post("/Rackaz/TutorTrainee/ManualMatchCreate", model, function (data) {
                                    alert(data.Message);
                                    if (data.Success) {
                                        window.location.href = "/Rackaz/TutorTrainee/ManualMatch";
                                    }
                                });
                            }
                        }
                    }
                    else {
                        //no first confirmation needed, checking outer
                        if (confirmatonrequired2 === true) {
                            if (confirm(message2)) {
                                //outer confirm
                                $.post("/Rackaz/TutorTrainee/ManualMatchCreate", model, function (data) {
                                    alert(data.Message);
                                    if (data.Success) {
                                        window.location.href = "/Rackaz/TutorTrainee/ManualMatch";
                                    }
                                });
                            }
                        } else {
                            //no confirmation needed
                            $.post("/Rackaz/TutorTrainee/ManualMatchCreate", model, function (data) {
                                alert(data.Message);
                                if (data.Success) {
                                    window.location.href = "/Rackaz/TutorTrainee/ManualMatch";
                                }
                            });
                        }
                    }
                }
            });
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