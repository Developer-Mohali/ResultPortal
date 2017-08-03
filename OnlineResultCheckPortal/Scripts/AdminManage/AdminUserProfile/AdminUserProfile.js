$(document).ready(function () {

    UserDetails();
    //$('#UserDetailsTable').dataTable().fnClearTable();
    //$('#UserDetailsTable').dataTable().fnDestroy();


});



function UserDetails() {
    var $myTable = $("#UserDetailsTable");
    $myTable.dataTable().fnDestroy();
    var table = $myTable.DataTable({
        "serverSide": true,
        "processing": true,
        "filter": true,
        ajax: {
            url: "/Admin/UserProfile/",
            type: "POST",
            datatype: "json"
        },
        columns: [
                { data: "RowNumber", "autoWidth": true },
                { data: "StudentID", "autoWidth": true },
                { data: "fullName", "autoWidth": true },
                { data: "CreateDate", "autoWidth": true },
                { data: "Gender", "autoWidth": true },
                { data: "LocalGovernment", "autoWidth": true },
                { data: "State", "autoWidth": true },
                { data: "SchoolName", "autoWidth": true },
                {
                    data: null,
                    className: "center",
                    "render": function (data, type, row) {
                        var inner = "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile(" + row.ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:14px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile(" + row.ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:12px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile(" + row.ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:14px;height:15px;'></a>"
                        //defaultContent: "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile()' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile()'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile( )'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;'></a>"
                        return inner;

                    }
                }

        ]
    });
   
}


//This function use to Get User list..
//function UserProfile() {

//    $.ajax({
//        searching: true,
//        scrollY: false,
//        paging: true,
//        processing: true, // for show progress bar
//        serverSide: true, // for process server side
//        filter: false, // this is for disable filter (search box)
//        orderMult: false, // for disable multiple column at once
//        url: '/Admin/UserProfile/',
//        type: 'GET',
//        enctype: "multipart/form-data",
//        dataType: "json",
//        success: function (d) {
//            var oTable = $('#UserDetailsTable').dataTable();
//            oTable.fnClearTable();

//            //Append for loop row to html table
//            for (var i = 0; i < d.length; i++) {


//                if (d[i].IsApproved == true) {
//                    $('#UserDetailsTable').dataTable().fnAddData([

//                     d[i].RowNumber, d[i].StudentID, d[i].fullName, d[i].Dob, d[i].Gender, d[i].LocalGovernment, d[i].State, d[i].SchoolName, "<a href='#''  data-placement='top' title='Edit' onclick='EditUserProfile(" + d[i].ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete' onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#' data-toggle='modal' data-placement='top' title='Update student profile' data-target='#myViewStudentProfile' onclick='DisplayStudentProfile(" + d[i].ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:16px;'></a>"

//                    ]);
//                }
//                else {
//                    $('#UserDetailsTable').dataTable().fnAddData([
//                    d[i].RowNumber, d[i].StudentID, d[i].fullName, d[i].Dob, d[i].Gender, d[i].LocalGovernment, d[i].State, d[i].SchoolName, "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile(" + d[i].ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile(" + d[i].ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;'></a>"

//                    ]);
//                }
//            }


//        }

//    });

//}

//This function use to Approved Register User.
function Approved(userID) {
    if (confirm("Are you sure you want to Approved ?")) {
        $.ajax({
            url: '/Admin/ApprovedStudent/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {

                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);

            },
        });
    }
}

//This function use to Approved Register User.
function UnApproved(userID) {
    if (confirm("Are you sure you want to UnApproved?")) {

        $.ajax({
            url: '/Admin/UnApprovedStudent/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);

            },
        });
    }

}

//This function use delete RegisterUser.
function DeleteUserProfile(userID) {
    if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/Admin/DeleteUserProfile/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                $("#lblMessage").show();
                $('#lblMessage').html(d);
                setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                UserDetails();
            },
        });
    }
}
//This function use to  Update User Register.
function UpdatUserProfile(controller) {

    var request = new UserRegister();
    $.ajax({
        url: controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
            $('#AddNewRegisterform').bootstrapValidator('resetForm', true);
            $("#lblupdatMessages").show();
            $('#lblupdatMessages').html(d);
            setTimeout(function () { $("#lblupdatMessages").hide(); }, 5000);
            $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;

        }
    });
}

//This functon use Edit User register.
function EditUserProfile(userID) {

    $.ajax({
        url: '/Admin/EditUserProfile/',
        dataType: 'json',
        type: "post",
        data: { 'userID': userID },
        success: function (data) {
            if (data.length > 0) {

                $.each(data, function (i, GetProfile) {
                    $("#txtRegistration").attr("disabled", "disabled");
                    $('#txtFirstName').val(GetProfile.FirstName);
                    $('#txtLastName').val(GetProfile.LastName);
                    $("#txtdob").val(GetProfile.Dob);
                    $("#txtAddress").val(GetProfile.Address);
                    $('#ddlSchoolName').val(GetProfile.School);
                    $('#txtRegistration').val(GetProfile.StudentID);
                    $('#txtLocalGoverment').val(GetProfile.LocalGovernment);
                    $('#txtState').val(GetProfile.State);
                    $('#DrpGender').val(GetProfile.Gender);
                    $('#ddlAcademicYears').val(GetProfile.AcademicYear);
                    $('#ViewUserID').val(GetProfile.ID);
                });

            }
        },
    });

}

//This functon use View User register.
function DisplayStudentProfile(userID) {
    $.ajax({
        url: '/Admin/EditUserProfile/',
        dataType: 'json',
        type: "post",
        data: { 'userID': userID },
        success: function (data) {
            if (data.length > 0) {

                $.each(data, function (i, GetProfile) {
                    $('#lblFirstName').html(GetProfile.FirstName);
                    $('#lblLastName').html(GetProfile.LastName);
                    $("#lbldob").html(GetProfile.Dob);
                    $("#lblAddress").html(GetProfile.Address);
                    $('#lblschool').html(GetProfile.SchoolName);
                    $('#lblRegistration').html(GetProfile.StudentID);
                    $('#lbllocalGovernment').html(GetProfile.LocalGovernment);
                    $('#lblstate').html(GetProfile.State);
                    $('#lblgender').html(GetProfile.Gender);
                    $('#lblAcademic').html(GetProfile.AcademicName);

                });

            }
        },
    });

}

//This function use to bind textbox.
function UserRegister() {
    var self = this;
    self.FirstName = $("#txtFirstName").val();
    self.Lastname = $("#txtLastName").val();
    self.DOB = $("#txtdob").val();
    self.Address = $("#txtAddress").val();
    self.School = $('#ddlSchoolName').val();
    self.StudentId = $('#txtRegistration').val();
    self.LocalGoverment = $('#txtLocalGoverment').val();
    self.State = $('#txtState').val();
    self.Gender = $('#DrpGender').val();
    self.AcademicYear = $('#ddlAcademicYears').val();
    self.ID = $('#ViewUserID').val();

}
//This function use to Display student details.
//function DisplayStudentProfile(userID)
//{
//    $.ajax({
//        url: '/Admin/GetStudentDetails/',
//        dataType: 'json',
//        type: "POST",
//        data: { 'userID': userID },
//        success: function (d) {

//            if (d.length > 0) {
//                $.each(d, function (i, GetProfile) {
//                    $('#txtStudentFirstName').val(GetProfile.FirstName);
//                    $('#txtStudentLastName').val(GetProfile.LastName);
//                    $('#txtStudentFatherName').val(GetProfile.FatherName);
//                    $('#drpSchollName').val(GetProfile.RoleName);
//                    $('#txtStudentEmail').val(GetProfile.EmailID);
//                    $('#txtAddress').val(GetProfile.Address); 
//                    $('#txtDateofBirth').val(GetProfile.Dob); 
//                    $('#txtContact').val(GetProfile.Contact);
//                    $('#txtGender').val(GetProfile.Gender);
//                    $('#ddlSchollName').val(GetProfile.School);
//                    $('#ddlAcademicYear').val(GetProfile.SchoolID);
//                    $('#ViewStudentID').val(GetProfile.ID);

//                });
//            }
//        },
//    });
//}

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

            $('#StudentDetails').bootstrapValidator('resetForm', true);
            $("#lblUpdatMessages").show();
            $('#lblUpdatMessages').html(d);
            setTimeout(function () { $("#lblUpdatMessages").hide(); }, 5000);
            $('#myStudentProfile').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;

        }
    });

}
//This function use to bind textbox get value.
function GetValueStudentDetails() {
    var self = this;
    self.FatherName = $('#txtStudentFatherName').val();
    self.School = $('#ddlSchollName').val();
    self.Address = $('#txtAddress').val();
    self.DOB = $('#txtDateofBirth').val();
    self.Contact = $('#txtContact').val();
    self.Gender = $('#txtGender').val();
    self.AcademicYear = $('#ddlAcademicYear').val();
    self.UserId = $('#ViewStudentID').val();
}