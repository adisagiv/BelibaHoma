$(function() {
    $('#submit')
        .click(function(e) {
            e.preventDefault();
            var form = $('form').serialize();
            $.post('/Login/Login',
                form,
                function(result) {
                    if (result.Success) {
                        window.location.href = result.Data;
                    } else {
                        alert(result.Message);
                    }
                });
        });

});