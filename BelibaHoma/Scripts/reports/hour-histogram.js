$(function () {
    function getHourHistogram() {
        function VerifySeries(series) {
            for (var i = 0; i < series.length; i++) {
                var serie = series[i];
                if (serie.data.length !== 0) {
                    return true;
                }
            }

            return false;
        }

        $.post("/Rackaz/Report/GetHourHistogram",  function (result) {
            if (result != null) {
                if (VerifySeries(result.series)) {
                    Highcharts.chart('container', result);
                } else {
                    $('#container').html("<div class='row' style='direction: rtl'><div class='form-group col-md-10'><label style='right: 50%;' class='control-label col-md-2'>אין נתונים</label></div></div>");
                }
            }

        });
    }

    getHourHistogram();
});