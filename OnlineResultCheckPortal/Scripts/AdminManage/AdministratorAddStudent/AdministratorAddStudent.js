$(document).ready(function () {
    UserDetails();
    $('[data-toggle="tooltip"]').tooltip();

});

function UserDetails() {
    var $myTable = $("#UserDetailsTable");
    $myTable.dataTable().fnDestroy();
    var table = $myTable.DataTable({
        searching: true,
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        ajax: {
            url: '/AdministratorAddStudent/UserProfile/',
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
                        var inner = "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile(" + row.ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile(" + row.ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>";
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
//        url: '/AdministratorAddStudent/UserProfile/',
//        type: 'GET',
//        enctype: "multipart/form-data",
//        dataType: "json",
//        success: function (d) {
//            var oTable = $('#UserDetailsTable').dataTable();
//            oTable.fnClearTable();
//            //Append for loop row to html table
//            for (var i = 0; i < d.length; i++) {
//                var ProfileImage;
            

//                if (d[i].IsApproved == true) {
//                    $('#UserDetailsTable').dataTable().fnAddData([
//                     d[i].RowNumber, d[i].StudentID, d[i].FirstName + ' ' + d[i].LastName, d[i].Dob, d[i].Gender, d[i].LocalGovernment, d[i].State,d[i].SchoolName,"<a href='#''  data-placement='top' title='Edit' onclick='EditUserProfile(" + d[i].ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete' id='delete' onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>"

//                    ]);
//                }
//                else {
//                    $('#UserDetailsTable').dataTable().fnAddData([
//                    d[i].RowNumber, d[i].StudentID, d[i].FirstName + ' ' + d[i].LastName, d[i].Dob, d[i].Gender, d[i].LocalGovernment, d[i].State,d[i].SchoolName,"<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile(" + d[i].ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>"

//                    ]);
//                }
//            }


//        }
//    });

//}

//This function use delete RegisterUser.
function DeleteUserProfile(userID) {
    if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/AdministratorAddStudent/DeleteUserProfile/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                $("#lblMessage").show();
                $('#lblMessage').html(d);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
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
    //alert("dd");
    $.ajax({
        url: '/AdministratorAddStudent/EditUserProfile/',
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