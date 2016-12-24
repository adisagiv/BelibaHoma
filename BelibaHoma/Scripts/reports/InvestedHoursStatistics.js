﻿$(function () {
    function getInvestedHoursStatistics() {

        $.post("/Rackaz/Report/InvestedHoursStatistics", function (result) {
            if (result != null) {
                Highcharts.chart('container', result);
            }

        });
    }

    // Cathcing the click event and collecti the data 
    $("#get-InvestedHours-statistics").click(function () {

        //// get year from input
        //var year = $('#year').val();

        // get data from server
        getInvestedHoursStatistics();
    });
    //// defualt call to get hour statitics 
    //var year = new Date().getFullYear();
    getInvestedHoursStatistics();
});