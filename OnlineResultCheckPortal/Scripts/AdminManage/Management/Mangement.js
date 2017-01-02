$(document).ready(function () {
    AdminMangement();
    $(document).ajaxStart(function () {
        $("#wait").css("display", "block");
    });
    $(document).ajaxComplete(function () {
        $("#wait").css("display", "none");
    });
    $(".Approved").click(function () {
        $("#txt").load("demo_ajax_load.asp");
    });
    $('[data-toggle="tooltip"]').tooltip();
});

//This function use to get admin detalis.
function AdminMangement() {
   
    $.ajax({
        url: '/Management/AdminMangement/',
        type: 'GET',
        dataType: "json",
        success: function (d) {
            var oTable = $('#AdminDetailsTable').dataTable();
            oTable.fnClearTable();
            //Append for loop row to html table
            for (var i = 0; i < d.length; i++) {
                var ProfileImage;
                if (d[i].Photo == null) {
                    ProfileImage = "NotAvailable.jpg";

                }
                else {
                    ProfileImage = d[i].Photo;

                }
                if (d[i].IsApproved == true) {
                    $('#AdminDetailsTable').dataTable().fnAddData([

                     d[i].RowNumber, d[i].FirstName, d[i].LastName, d[i].EmailID, d[i].RoleName, "<img src=\"/StudentPhoto/" + ProfileImage + "\"  alt=\"\" class='img-thumbnail' style=' width:40px; height:40px;margin-top:7%;' />", "<a data-toggle='tooltip' data-placement='top' title='Admin Approval' href='#' onclick='UnApprovedAdmin(" + d[i].UserId + ")'><img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'></a>", "<a href='#''  onclick='EditAdminMangement(" + d[i].UserId + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myAdminProfile'  data-placement='top' title='Edit' alt='Mountain View' style='width:18px;height:20px;margin-left:5px;'></a>&nbsp;|&nbsp;<a  href='#' data-toggle='tooltip' data-placement='top' title='Delete' onclick='DeleteAdminMangement(" + d[i].UserId + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:3px;color: red;'></a>"

                    ]);
                }
                
                    else {
                          $('#AdminDetailsTable').dataTable().fnAddData([
                                           d[i].RowNumber, d[i].FirstName, d[i].LastName, d[i].EmailID, d[i].RoleName, "<img src=\"/StudentPhoto/" + ProfileImage + "\" class='img-thumbnail' alt=\"\" style=' width:40px; height:40px;margin-top:7%;'  />", "<a href='#' id='Approved' onclick='ApprovedAdmin(" + d[i].UserId + ")'/><img src='/Images/iconvalide.PNG' alt='Mountain View' style='width:20px;height:20px;margin-left:24px;' data-toggle='tooltip' data-placement='top' title='Deactivate Admin Approval'></a>", "<a href='#''  onclick='EditAdminMangement(" + d[i].UserId + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myAdminProfile' alt='Mountain View' style='width:18px;height:20px;margin-left:5px;'></a>&nbsp;|&nbsp;<a  href='#' data-toggle='tooltip' data-placement='top'  onclick='DeleteAdminMangement(" + d[i].UserId + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:3px;color: red;'></a>"

                        ]);
                    }
                }
            }

    });

}


//This function use delete DeleteAdminMangement.
function DeleteAdminMangement(userID) {
    if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/Management/DeleteAdminMangement/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                AdminMangement();
                $("#lblMessage").show();
                $('#lblMessage').html(d);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
            },
        });
    }
}

//This function use to Edit Admin manage.
function EditAdminMangement(userID) {
   
    DropdownlistBind(userID);
    $.ajax({
        url:'/Management/GetEditManage',
        dataType: 'json',
        type: "post",
        data: { 'userID': userID },
        success: function (data) {
            if (data.length > 0) {
                
                $.each(data, function (i, GetProfile) {
                    $('#SchoolName').show();
                    $("#txtEmail").attr("disabled", "disabled");
                    $('#txtFirstName').val(GetProfile.FirstName);
                    $('#txtLastName').val(GetProfile.LastName);
                    $('#txtEmail').val(GetProfile.EmailID);
                    $('#ViewUserID').val(GetProfile.UserId);
                    $("#txtFatherName").val(GetProfile.FatherName);
                    $("#txtDateofBirth").val(GetProfile.DateofBirth);
                    $("#txtContact").val(GetProfile.ContactNo);
                    $("#txtAddress").val(GetProfile.Address);
                    $('#ddlRoleName').val(GetProfile.RoleID);
                    $('#ddlGender').val(GetProfile.Gender);
                    $('#txtConformPassword').val(GetProfile.Password);
                    $('#txtPassword').val(GetProfile.Password);
                    $('#ddlSchoolName').val(GetProfile.SchoolID);
                });

            }
        },
    });
}


//This function use to  Update User Register.
function UpdatUserAdminMangement(controller) {
   
    var request = new UserRegister();
    $.ajax({
        url: controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
            $("#lblUpdateMessages").show();
            $('#lblUpdateMessages').html(d);
            setTimeout(function () { $("#lblUpdateMessages").hide(); }, 5000);
            $('#AdminManagement').bootstrapValidator('resetForm', true);
            AdminMangement();
            $('#myAdminProfile').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;

            //document.forms["#AdminManagement"].reset();
            //$("#AdminManagement").resetForm();
        }
    });
};

//This function use to bind get textbox value.
function UserRegister() {

    //console.log(schoolId);
    var self = this;
    self.FirstName = $("#txtFirstName").val();
    self.LastName = $("#txtLastName").val();
    self.Email = $("#txtEmail").val();
    //self.FatherName = $("#txtFatherName").val();
    //self.DOB = $("#txtDateofBirth").val();
    self.Contact = $("#txtContact").val();
    self.Address = $("#txtAddress").val();
    self.RoleId = $('#ddlRoleName').val();
    self.Gender = $('#ddlGender').val();
    self.ID = $('#ViewUserID').val();
    self.School = $('#ddlSchoolName').val();
    self.ConfirmPassword = $('#txtConformPassword').val();
}

//This function use to bind dropdown list and check box.

function DropdownlistBind(userID)
{

    $('#SchoolNameDisplay').val('').empty();
    $.ajax({
        url: '/Management/GetSchoolName/',
        dataType: 'json',
        type: "POST",
        data: { 'userID': userID },
        success: function (d) {
                $('#show').show();
                var $data = $('<table></table>').addClass('table table-bordered table-mod-2 table-responsive');
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');

                    $row.append($('<td class="hidden-xs hidden-sm"/>').html(row.SchoolName));

                    $data.append($row);
                });

                $('#SchoolNameDisplay').append($data);
         
        },
    });
}


//This function use to Approved admin User.
function ApprovedAdmin(userID) {
    if (confirm("Are you sure you want to Approved ?")) {
        $.ajax({
            url: '/Management/ApprovedAdmin/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                AdminMangement();
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 5000);

            },
        });
    }
}

//This function use to Approved admin User.
function UnApprovedAdmin(userID) {
    if (confirm("Are you sure you want to UnApproved ?")) {
        $.ajax({
            url: '/Management/UnApprovedAdmin/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                AdminMangement();
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 5000);

            },
        });
    }
}