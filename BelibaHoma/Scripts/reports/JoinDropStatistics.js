$(function () {
    function getJoinDropStatistics() {

        $.post("/Rackaz/Report/GetJoinDropStatistics", function (result) {
            if (result != null) {
                Highcharts.chart('container', result);
            }

        });
    }

    // Cathcing the click event and collecti the data 
    $("#get-JoinDrop-statistics").click(function () {

        //// get year from input
        //var year = $('#year').val();

        // get data from server
        getJoinDropStatistics();
    });
    //// defualt call to get hour statitics 
    //var year = new Date().getFullYear();
    getJoinDropStatistics();
});