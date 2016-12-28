$(function () {
    function getAvrGradeStatistics() {

        $.post("/Rackaz/Report/GetAvrGradeStatistics", function (result) {
            if (result != null) {
                Highcharts.chart('container', result);
            }

        });
    }

    // Cathcing the click event and collecti the data 
    $("#get-AvrGrade-statistics").click(function () {

        //// get year from input
        //var year = $('#year').val();

        // get data from server
        getAvrGradeStatistics();
    });
    //// defualt call to get hour statitics 
    //var year = new Date().getFullYear();
    getAvrGradeStatistics();
});