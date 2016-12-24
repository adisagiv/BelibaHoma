$(function () {
    $('#submit-and-run').click(function (e) {
        e.preventDefault();
        if (window.confirm("הרצת האלגוריתם גוררת איפוס השיבוצים הקיימים באזור.\nהאם ברצונך להמשיך?")) {
            var area = $('#Area').val();
            //if okay
            $.post('/Rackaz/TutorTrainee/RunAlgorithm', { Area: area }, function (result) {
                //debugger;
                $('#run-algorithm-form').addClass("hidden");
                if (result.Success) {
                    
                    $('#after-run').removeClass("hidden");
                    if (result.Data) {
                        $("#unmatched").removeClass("hidden");
                    }
                } else {
                    $('#run-failure').removeClass("hidden");
                    alert(result.Message);
                }
            });
        }
    });
});