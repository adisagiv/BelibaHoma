$(function () {
    function getHourStaistics(hourType, year) {

        $.post("/Rackaz/Report/GetHourStatistics", { hourStatisticsType : hourType,  year : year }, function (result) {
            if (result != null) {
                Highcharts.chart('container', result);
            }

        });
    }

    // Cathcing the click event and collecti the data 
    $("#get-hour-statistics").click(function () {
        // get year from input
        var year = $('#year').val();
        var hourType = $('#hour-type').val();
        // get data from server
        getHourStaistics(hourType, year);
    });

    // defualt call to get hour statitics 
    var year = $('#year').val();
    getHourStaistics(0, year);
});