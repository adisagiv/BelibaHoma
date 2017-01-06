$(function () {
    debugger;
    function getAlertsStatistics(year) {

        $.post("/Rackaz/Report/GetAlertsStatistics", { year: year }, function (result) {
            if (result != null) {
                Highcharts.chart('container', result);
            }

        });
    }

    // Cathcing the click event and collecti the data 
    $("#get-alert-statistics").click(function () {

        //// get year from input
        var year = $('#year').val();

        // get data from server
        getAlertsStatistics(year);
    });
    //// defualt call to get hour statitics 
    var year = new Date().getFullYear();
    getAlertsStatistics(year);
});