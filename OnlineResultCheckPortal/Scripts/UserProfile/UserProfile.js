$(document).ready(function () {
    GetStudentPrfoile();
    DisplayStudentProfile();
});

//This function use Get Student profile..
function GetStudentPrfoile() {

    $.ajax({
        url: '/Student/StudentProfile',
        type: 'GET',
        dataType: "json",
        success: function (d) {
            //Append for loop row to html table
            for (var i = 0; i < d.length; i++) {
               
                   
                    $('#StudentName').html(d[i].FirstName + ' ' + d[i].LastName);
                    $('#Gender').html(d[i].Gender);
                    $('#DateofBirth').html(d[i].Dob);
                    $('#Contact').html(d[i].Contact);
                    $('#Email').html(d[i].EmailID);
                    $('#Address').html(d[i].Address);
                    $('#Contacts').html(d[i].Contact);
                    $('#EmailId').html(d[i].EmailID);
                    $('#Addres').html(d[i].Address); 
                    $('#SchoolName').html(d[i].SchoolName);
                    if (d[i].Picture == null) {
                    $('#ProfileName').html(d[i].FirstName + ' ' + d[i].LastName);
                    $('#StudentProfile').html("<img src=\"/StudentPhoto/NotAvailable.jpg\" alt=\"\" />", "<img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>");
                    $('#ProfilImage').html("<img src=\"/StudentPhoto/NotAvailable.jpg\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;' />", "<img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>");
                }
                    else {
                        $('#ProfileName').html(d[i].FirstName + ' ' + d[i].LastName);
                        $('#ProfilImage').html("<img src=\"/StudentPhoto/" + d[i].Picture + "\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;'  />");
                        $('#StudentProfile').html("<img src=\"/StudentPhoto/" + d[i].Picture + "\" alt=\"\"/>");
                }
            }

        },
    });

}

//This function use to Display student details.
function DisplayStudentProfile() {
    $.ajax({
        url: '/Student/StudentProfile',
        dataType: 'json',
        type: "POST",
        success: function (d) {
            if (d.length > 0) {
                $.each(d, function (i, GetProfile) {
                    $('#txtStudentFirstName').val(GetProfile.FirstName);
                    $('#txtStudentLastName').val(GetProfile.LastName);
                    $('#txtStudentFatherName').val(GetProfile.FatherName);
                    $('#drpSchollName').val(GetProfile.RoleName);
                    $('#txtStudentEmail').val(GetProfile.EmailID);
                    $('#txtAddress').val(GetProfile.Address);
                    $('#txtDateofBirth').val(GetProfile.Dob);
                    $('#txtContact').val(GetProfile.Contact);
                    $('#txtGender').val(GetProfile.Gender);
                    $('#ddlSchollName').val(GetProfile.School);
                   
                    $('#ViewStudentID').val(GetProfile.ID);

                });
            }
        },
    });
}



//function SaveUserProfileAllDetails(controller) {
//    var request = new UserProfile();
//    $.ajax({
//        url: controller,
//        dataType: 'json',
//        contentType: "application/json",
//        type: "POST",
//        data: JSON.stringify(request),
//        success: function (response) {
//            $("#resultUser").html(response.result);
//            window.location.href = '/UserProfile/Index';
//        }
//    });





//}

function UserProfile() {
    var self = this;
    self.UserId = $("#UserId").val();
    
    self.FirstName = $("#FirstName").val();
    self.LastName = $("#LastName").val();
    //self.Login = $("#Login").val();
    self.IsApproved = $("input[name=IsApproved]:checked").val();
    self.Gender = $("input[name=Gender]:checked").val();
    // self.gender = $("#Gender").val();
    self.FatherName = $("#FatherName").val();
    self.Contact = $("#Contact").val();
    self.DOB = $("#DOB").val();
    self.Email = $("#Email").val();
    self.Address = $("#Address").val();
    self.AcademicYear = $("#AcademicYear").val();
}


//This function use to Update Student Details.

function UpdateStudentDetails(Controller) {
    var request = new GetValueStudentDetails();
    $.ajax({
        url: Controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
            GetStudentPrfoile();
            $('#txtStudentFirstName').val("");
            $('#txtStudentLastName').val("");
            $('#txtStudentFatherName').val("");
            $('#drpSchollName').val("");
            $('#txtStudentEmail').val("");
            $('#txtAddress').val("");
            $('#txtDateofBirth').val("");
            $('#txtContact').val("");
            $('#txtGender').val("");
            $("#lblUpdatMessages").show();
            $('#lblUpdatMessages').html(d);
            setTimeout(function () { $("#lblUpdatMessages").hide(); }, 5000);

        }
    });

}
//This function use to bind textbox get value.
//function GetValueStudentDetails() {
//    var self = this;
//    self.FirstName = $('#txtStudentFirstName').val();
//    self.LastName = $('#txtStudentLastName').val();
//    self.Gender = $('#txtGender').val();
//    self.FatherName = $('#txtStudentFatherName').val();
//    self.School = $('#ddlSchollName').val();
//    self.Address = $('#txtAddress').val();
//    self.DOB = $('#txtDateofBirth').val();
//    self.Contact = $('#txtContact').val();
//    self.Gender = $('#txtGender').val();
//    self.UserId = $('#ViewStudentID').val();
//}