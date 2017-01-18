$(document).ready(function () {
 
    DisplayAdminProfile();
});
//This function use to Display AdminProfile details.
function DisplayAdminProfile() {
    //alert("test");
    //console.log(test);
    $.ajax({
        url: '/AdminProfile/GetAdminProfileDetails',
        type: 'GET',
        dataType: "json",
        success: function (d) {
            //Append for loop row to html table
            for (var i = 0; i < d.length; i++) {
                $('#AdminFullNames').html(d[i].FirstName + ' ' + d[i].LastName);
                $('#AdminFullName').html(d[i].FirstName + ' ' + d[i].LastName);
                $('#Gender').html(d[i].Gender);
                $('#FatherName').html(d[i].FatherName);
                $('#DateofBirth').html(d[i].DateofBirth);
                $('#EmailId').html(d[i].EmailID);
                $('#AdminAddress').html(d[i].Address);
                $('#ContactNo').html(d[i].ContactNo);
                $('#Contacts').html(d[i].ContactNo);
                $('#Email').html(d[i].EmailID);
                if (d[i].Photo == null) {
                    $('#ProfileImages').html("<img src=\"/Images/Empty.jpg\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;' />", "<img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>");
                    $('#AdminProfile').html("<img src=\"/StudentPhoto/NotAvailable.jpg\" alt=\"\" style='' />", "<img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>");
                }
                else {
                    $('#ProfileImages').html("<img src=\"/StudentPhoto/" + d[i].Photo + "\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;'  />");
                    $('#AdminProfile').html("<img src=\"/StudentPhoto/" + d[i].Photo + "\" alt=\"\"/>");

                }
            }

        },
    });

}
//This functon use Edit AdminProfile.
function EditAdminProfile(userID) {

    $.ajax({
        url: '/AdminProfile/EditAdminProfile/',
        dataType: 'json',
        type: "post",
        data: { 'userID': userID },
        success: function (data) {
            if (data.length > 0) {
                $.each(data, function (i, GetProfile) {
                    $("#txtEmail").attr("disabled", "disabled");
                    $('#txtFirstName').val(GetProfile.FirstName);
                    $('#txtLastName').val(GetProfile.LastName);
                    $('#txtEmail').val(GetProfile.EmailID);
                    $('#ddlroleId').val(GetProfile.RoleId);
                    $('#txtConformpassword').val(GetProfile.Password);
                    $('#ViewUserID').val(GetProfile.ID);
                    $('#txtPassword').val(GetProfile.Password);

                    //$('#txtAdminFirstName').val("");
                    //$('#txtAdminLastName').val("");
                    //$('#txtAdminFatherName').val("");
                    //$('#drpSchollName').val("");
                    //$('#txtStudentEmail').val("");
                    //$('#txtAddress').val("");
                    //$('#txtDateofBirth').val("");
                    //$('#txtContact').val("");
                    //$('#txtGender').val("");
                });

            }
        },
    });

}
//This function use to Update Student Details.

function UpdateAdminProfileDetails(Controller) {
    var request = new GetValueAdminProfileDetails();
    $.ajax({
        url: Controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
            DisplayAdminProfile();
            $('#txtAdminFirstName').val("");
            $('#txtAdminLastName').val("");
            $('#txtAdminFatherName').val("");
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
function GetValueAdminProfileDetails() {
    var self = this;
    self.FatherName = $('#txtAdminFatherName').val();
    self.School = $('#ddlSchollName').val();
    self.Address = $('#txtAddress').val();
    self.DOB = $('#txtDateofBirth').val();
    self.Contact = $('#txtContact').val();
    self.Gender = $('#txtGender').val();
    self.AcademicYear = $('#ddlAcademicYear').val();
    self.UserId = $('#ViewAdminID').val();
}