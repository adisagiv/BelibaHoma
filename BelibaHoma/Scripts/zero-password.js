
$(function () {
    $(".zero-password").click(function (event) {
        var userId = $(this).data("userid");
        if (confirm("האם אתה בטוח שאתה רוצה לאפס את הסיסמא")) {
            $.post("/Rackaz/User/ZeroPassword", { id: userId }, function(status) {
                alert(status.Message);
            });
        }
    });
    

});



