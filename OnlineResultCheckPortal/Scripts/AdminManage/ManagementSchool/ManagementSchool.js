$(document).ready(function () {
    StudentProfileDisplay();
    SchoolProfile();
});

//This function use to Displaying Student Profile.

function StudentProfileDisplay()
{
 
    $.ajax({
        url: '/ManageSchoolManagement/StudentProfile/',
        type: 'get',
        dataType: 'json',
        success: function (d) {
            var oTable = $('#StudentDetailsTable').dataTable();
            oTable.fnClearTable();
            if (d.length > 0) {
             
                $.each(d, function (i, row) {
                   
                    var ProfileImage;
                    if (row.Picture == null) {
                        ProfileImage = "NotAvailable.jpg";


                    }
                    else {
                        ProfileImage = row.Picture;

                    }

                    if (row.SchoolName == null) {
                      
                        $('#StudentDetailsTable').dataTable().fnAddData([
                         row.RowNumber, row.fullName, row.EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\"  style=' width:50px; height:50px;margin-top:7%;' />", "<input type='checkbox' id='checkbox' style='margin-top:7%;margin-left: 8px;' name='StudentId' value='" + row.ID + "'>"

                        ]);
                    }
                    else {
                        //$("#checkbox").attr("checked", "checked");
                        $('#StudentDetailsTable').dataTable().fnAddData([
                       row.RowNumber, row.fullName, row.EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\"  style=' width:50px; height:50px;margin-top:7%;' />", "<input type='checkbox' id='checkbox'  style='margin-top:7%;margin-left: 8px;' name='StudentId' value='" + row.ID + "' checked>"

                        ]);
                    }
                });


            }
            else {
                $('StudentDetailsTable').html('No Data Found!');
            }
        }
    });
}

function SchoolProfile() {

    $.ajax({
        url: '/ManageSchoolManagement/SchoolProfile/',
        type: 'get',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {
                var oTable = $('#StudentTable').dataTable();
                oTable.fnClearTable();
                $.each(d, function (i, row) {

                    $('#StudentTable').dataTable().fnAddData([
                       row.SchoolName, row.SchoolAddress, row.Zone, row.Zipcode, "<span class='badge'>" + row.AllCountSchool + "</span>", "<a href='#''  onclick='EditUserProfile(" + row.school + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:18px;height:20px;margin-left:7px;'></a>&nbsp;|&nbsp;<a href='#''  onclick='GetDisplayStudentProfile(" + row.school + ")' class='glyphicon glyphicon-eye-open' data-toggle='modal' data-target='#myModalDetails' alt='Mountain View' style='width:18px;height:20px;margin-left:7px;'></a>"

                   ]);
                });

            }
            else {
                $('StudentDetadfilsTable').html('No Data Found!');
            }
        }
    });
}

//This function use to save school name in table.

function SaveSchoolNameInStudent(controller)
{
   
    var request = new BindandGetValue();
    var studentID = []
    $('input:checkbox[name=StudentId]:checked').each(function () {
        if (this.checked)
            studentID = studentID + $(this).val() + ',';
    });

    request.StudentId = studentID;
  
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
            SchoolProfile();
           
            $('#AddNewRegisterform').bootstrapValidator('resetForm', true);
            $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;
        }
    });
}

function BindandGetValue()
{
    var self = this;
    self.School = $('#ddlSchoolName').val();
  
}



function GetDisplayStudentProfile(SchoolID) {

    $.ajax({
        url: '/ManageSchoolManagement/AddSchoolDisplayStudentProfile/',
        type: 'get',
        dataType: 'json',
        data: { 'SchoolID': SchoolID },
        success: function (d) {
            var oTable = $('#StudentAllViewDetailsTable').dataTable();
            oTable.fnClearTable();
            if (d.length > 0) {

                $.each(d, function (i, row) {

                    var ProfileImage;
                    if (row.Picture == null) {
                        ProfileImage = "NotAvailable.jpg";


                    }
                    else {
                        ProfileImage = row.Picture;

                    }

                    if (row.IsApproved == true) {
                        $('#StudentAllViewDetailsTable').dataTable().fnAddData([
                         row.RowNumber, row.fullName, row.EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\"  style=' width:50px; height:50px;margin-top:7%;' />", row.SchoolName

                        ]);
                    }
                    else {
                        $('#StudentAllViewDetailsTable').dataTable().fnAddData([
                       row.RowNumber, row.fullName, row.EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\"  style=' width:50px; height:50px;margin-top:7%;' />", row.SchoolName

                        ]);
                    }
                });


            }
            else {
                $('StudentDetailsTable').html('No Data Found!');
            }
        }
    });
}