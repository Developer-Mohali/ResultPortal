$(document).ready(function () {
    DisplayManageJSCE();
});
//This function use delete Delete  ManageJSCE.
function DeleteManageJSCE(userID) {
    if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/ManageJSCE/DeleteManageJSCE',
            dataType: 'json',
            type: "GET",
            data: { 'userID': userID },
            success: function (d) {
                DisplayManageJSCE();
                $("#lblMessage").show();
                $('#lblMessage').html(d);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
            },
        });
    }
}
//This function use to bind get textbox value.
function UserRegisterJSCE() {
    var self = this;
    self.StudentID = $("#FirstName").val();
    self.JSCERegNumber = $("#txtJSCERegistrationNumber").val();
}
//This function use to  SaveUpdate User Register.
function SaveUpdateManageJSCEListDetails() {
   
    var request = new UserRegisterJSCE();
 
    $.ajax({
        url: '/ManageJSCE/SaveUpdateManageJSCEListDetails',
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
            if (d == "Already Exist") {
                $("#lblMessage").show();
                $('#lblMessage').html(d);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
            }
            else {
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 5000);
            }
            DisplayManageJSCE();
            $('#ValidationManageJSCE').bootstrapValidator('resetForm', true);
           
            $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;
           // window.location.href = '/ManageJSCE/Index';
        }
    });
}
function DisplayManageJSCE() {

    $.ajax({
        // type: "POST",
        url: '/ManageJSCE/DisplayManageJSCE',
      type: 'GET',
        dataType: "json",
        success: function (msg) {
            var oTable = $('#UserDetailsTable').dataTable();
            oTable.fnClearTable();
            //  var obj = jQuery.parseJSON(msg);
            if (msg.length > 0) {
                $.each(msg, function (i, row) {
                   // console.log("row:" + FirstName);
                    $('#UserDetailsTable').dataTable().fnAddData([
                    //"<a onclick='EditAdminManageJSCE(" + row.ID + ")'  href='#'' class='icon-pencil popup'></a> <a onclick='DeleteAdminMangement(" + row.ID + ")' href='#'' class='icon-trash'></a>",    
                    row.RowNumber,
                    row.fullName,
                    row.JSCERegNumber,
                    //row.StundentID,
                    row.CreatedDate,
                    //"<a  onclick='EditAdminManageJSCE(" + row.ID + ")'  href ='#'' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:18px;height:20px;margin-left:13px;'></a><a onclick='DeleteUserProfile(" + row.ID + ") href='#' 'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:12px;'></a><a href='#' data-toggle='modal' data-target='#myStudentProfile' onclick='DisplayStudentProfile(" + d[i].ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:20px;height:16px;margin-left:12px;'></a><a  href='#'  onclick='DeleteAdminMangement(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:12px;'></a>"
                     "<a onclick='EditManageJSCE(" + row.JSCEID + ")'  href='#'' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:18px;height:20px;;margin-left:13px;'></a> <a onclick='DeleteManageJSCE(" + row.JSCEID + ")' href='#'' class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:12px;'></a>",
                    ]);

                });
            }
            else {
         
               
            }
        }
    });
   
}
//This function use to Edit  manageJSCE.
function EditManageJSCE(userID) {
    
    $.ajax({
        url: '/ManageJSCE/EditManageJSCE',
        dataType: 'json',
        type: "POST",
       data: { 'userID': userID },
       success: function (data) {
           console.log(data)
            if (data.length > 0) {
                $.each(data, function (i, row) {
                    $('#FirstName').val(row.StundentID);
                    $('#txtJSCERegistrationNumber').val(row.JSCERegNumber);
                    //DisplayAdminManageJSCE();
                    //$('#FirstName').val(row.FirstName);
                    //$('#txtJSCERegistrationNumber').val(row.JSCERegistrationNumber);
                    //$('#ViewUserID').val(GetProfile.UserId);
                    $("#resultEditUser").html(response.result);
                });

            }
        },
    });
}