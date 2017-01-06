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
    
    $('#trainee-submit').click(function(e) {
        e.preventDefault();
        if ($('#trainee-form').valid()) {

            var model = {
                EmploymentStatus: undefined,
                Gender: undefined,
                MaritalStatus: undefined,
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
                AcademicMajor2: {
                    Id: undefined,
                    Name: undefined,
                    AcademicCluster: undefined
                },
                NeededHelpDescription: undefined,
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

            model.EmploymentStatus = $('#Trainee_EmploymentStatus').val();
            model.User.IdNumber = $('#Trainee_User_IdNumber').val();
            model.User.FirstName = $('#Trainee_User_FirstName').val();
            model.User.LastName = $('#Trainee_User_LastName').val();
            model.User.Area = $('#Trainee_User_Area').val();
            model.User.Password = $('#Trainee_User_Password').val();
            model.User.IsActive = $('#Trainee_User_IsActive').is(":checked");
            model.User.Email = $('#Trainee_User_Email').val();
            model.Gender = $('#Trainee_Gender').val();
            model.MaritalStatus = $('#Trainee_MaritalStatus').val();
            model.Address = $('#Trainee_Address').val();
            model.Birthday = $('#Trainee_Birthday').val();
            model.PhoneNumber = $('#Trainee_PhoneNumber').val();
            model.AcademicInstitution.Id = $('#Trainee_AcademicInstitution').val();
            model.AcademicMajor.Id = $('#Trainee_AcademicMajor').val();
            model.AcademicMajor1.Id = $('#Trainee_AcademicMajor1').val();
            model.AcademicMajor2.Id = $('#Trainee_AcademicMajor2').val();
            model.NeededHelpDescription = $('#Trainee_NeededHelpDescription').val();
            model.PhysicsLevel = $('#Trainee_PhysicsLevel').val();
            model.MathLevel = $('#Trainee_MathLevel').val();
            model.EnglishLevel = $('#Trainee_EnglishLevel').val();
            model.AcademicYear = $('#Trainee_AcademicYear').val();
            model.SemesterNumber = $('#Trainee_SemesterNumber').val();
            model.DroppedOut = $('#Trainee_DroppedOut').is(":checked");

            var isCreate = $('#is-create').val();
            var url = '';
            if (isCreate == "true") {
                url = '/Rackaz/Trainee/Create';
            } else {
                url = '/Rackaz/Trainee/Edit/' + $('#UserId').val();
            }


            $.post(url, model, function(data) {
                alert(data.Message);
                if (data.Success) {
                    window.location.href = "/Rackaz/Trainee";
                }
            });
        } else {
            $('#trainee-form').validate();
        }
    });
});