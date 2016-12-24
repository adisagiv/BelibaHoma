$(function () {
    $('#submit-and-run').click(function (e) {
        e.preventDefault();
        if (window.confirm("הרצת האלגוריתם גוררת איפוס השיבוצים הקיימים באזור.\nהאם ברצונך להמשיך?")) {
            var area = $('#Area').val();
            //if okay
            $.post('/Rackaz/TutorTrainee/RunAlgorithm', { Area: area }, function (result) {
                debugger;
            });
        }
    });
});