$(function () {
    //Create a new academic inst. from form
    //$('#create-new-academic').click(function (ac) {
    //    ac.preventDefault();
    //    $('#academmic-institution-modal').modal();
    //});
    $('#datetimepicker1').datetimepicker({
        locale: 'he',
        format: 'DD/MM/YYYY'
    });
    $('#tutor-submit').click(function (e) {
        e.preventDefault();
        if ($('#tutor-form').valid()) {
        var model = {
            EmploymentStatus: undefined,
            Gender: undefined,
            Address: undefined,
            Birthday: undefined,
            PhoneNumber: undefined,
            AcademicInstitution: {
                Id: undefined,
                Name: undefined,
                Area: undefined,
                InstitutionType: undefined
            },
            AcademicMajor: {
                Id: undefined,
                Name: undefined,
                AcademicCluster: undefined
            },
            AcademicMajor1: {
                Id: undefined,
                Name: undefined,
                AcademicCluster: undefined
            },

            PhysicsLevel: undefined,
            MathLevel: undefined,
            EnglishLevel: undefined,
            AcademicYear: undefined,
            SemesterNumber: undefined,
            User: {
                IdNumber: undefined,
                FirstName: undefined,
                LastName: undefined,
                Area: undefined,
                Password: undefined,
                IsActive: undefined,
                Email: undefined

            },
            DroppedOut: undefined
        };

        model.User.IdNumber = $('#Tutor_User_IdNumber').val();
        model.User.FirstName = $('#Tutor_User_FirstName').val();
        model.User.LastName = $('#Tutor_User_LastName').val();
        model.User.Area = $('#Tutor_User_Area').val();
        model.User.Password = $('#Tutor_User_Password').val();
        model.User.IsActive = $('#Tutor_User_IsActive').is(":checked");
        model.User.Email = $('#Tutor_User_Email').val();
        model.Gender = $('#Tutor_Gender').val();
        model.Address = $('#Tutor_Address').val();
        model.Birthday = $('#Tutor_Birthday').val();
        model.PhoneNumber = $('#Tutor_PhoneNumber').val();
        model.AcademicInstitution.Id = $('#Tutor_AcademicInstitution').val();
        model.AcademicMajor.Id = $('#Tutor_AcademicMajor').val();
        model.AcademicMajor1.Id = $('#Tutor_AcademicMajor1').val();
        model.PhysicsLevel = $('#Tutor_PhysicsLevel').val();
        model.MathLevel = $('#Tutor_MathLevel').val();
        model.EnglishLevel = $('#Tutor_EnglishLevel').val();
        model.AcademicYear = $('#Tutor_AcademicYear').val();
        model.SemesterNumber = $('#Tutor_SemesterNumber').val();
        model.DroppedOut = $('#Tutor_DroppedOut').is(":checked");

        var isCreate = $('#is-create').val();
        var url = '';
        if (isCreate == "true") {
            url = '/Rackaz/Tutor/Create';
        } else {
            url = '/Rackaz/Tutor/Edit/' + $('#UserId').val();
        }


        $.post(url, model, function (data) {
            alert(data.Message);
            if (data.Success) {
                window.location.href = "/Rackaz/Tutor";
            }
        });
        } else {
            $('#tutor-form').validate();
        }
    });
});