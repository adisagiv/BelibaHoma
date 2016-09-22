$(function () {
    $("#UserRole").change(function (event) {
        var selectedOption = $("option:selected", this);

        if (selectedOption.val() == 1) {
            ShowAreas();
        } else {
            HideAreas();
        }
    });
        function ShowAreas() {
            var SelectArea = $("#SelectArea");
            SelectArea.slideDown().removeClass("hidden");
        }

        function HideAreas() {
            var SelectArea = $("#SelectArea");
            SelectArea.slideUp(function () {
                $(this).addClass("hidden");
            });
        }

});

