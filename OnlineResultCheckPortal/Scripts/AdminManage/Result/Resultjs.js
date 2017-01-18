$(document).ready(function () {
   
    $(document).ajaxStart(function () {
        $("#wait").css("display", "block");

    });
    $(document).ajaxComplete(function () {
        $("#wait").css("display", "none");
    });
    $("#SearchButtton").click(function () {
        $("#txt").load("demo_ajax_load.asp");

    });


    });

$('#ResultsDownload').click(function () {

    var request = new GetTextboxValue();
    $.ajax({
        url: '/Result/ResultDownloadFile',
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (returnValue) {
            for (i = 0; i < returnValue.length; i++) {
                var checkfolder = returnValue[i].CheckID;
                var excelfile = returnValue[i].ReportCardFile;
                if (excelfile ==null) {
                    alert("Your report card not uploaded.");
                }
                else {

                    window.location = '/Result/DownloadFile?file=' + excelfile + '&checkfolder=' + checkfolder;
                }
            }
          
        }
    });

});


function GetTextboxValue() {
    var self = this;
    self.ExamTypes = $('#DrpExamTypes').val();
    self.Registration = $('#TxtSearchResult').val();
    self.TokenNumber = $('#TxtToken').val();
    self.SchoolID = $('#ddlSchool').val();
}

function SearchResult(Controller) {
    var request = new GetTextboxValue();
    $.ajax({
        url: Controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {

            if (d.length > 0) {
                if (d == "1") {
                    $("#lblMessage").show();
                    $('#lblMessage').html("Token Id already provided.");
                    setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                }
                else if(d=="2")
                {
                    $("#lblMessage").show();
                    $('#lblMessage').html("Your token number is not verified.");
                    setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                }
                else if (d == "3") {
                    $("#lblMessage").show();
                    $('#lblMessage').html("Not valid Token Number.");
                    setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                }
                else if (d == "4") {
                    $('#UserDetailsTables').hide();
                    $("#lblMessage").show();
                    $('#lblMessage').html("Your result not uploaded.");
                    setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                }
                else if (d == "5") {
                    $("#lblMessage").show();
                    $('#lblMessage').html("Please purchase new token.");
                    setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                }
                else {

                    var oTable = $('#UserDetailsTable').dataTable();
                    oTable.fnClearTable();
                    $('#UserDetailsTables').show();
                    //Append for loop row to html table
                    for (var i = 0; i < d.length; i++) {
                        $('#fileResult').val(d[i].RegistrationNumber);
                        $('#UserDetailsTable').dataTable().fnAddData([
                        d[i].RowNumber,d[i].RegistrationNumber, d[i].FullName, d[i].SubjectName, d[i].Grade, d[i].Remarks //"<a  href='#'  onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:2px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myStudentProfile' onclick='DisplayStudentProfile(" + d[i].ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;margin-left:2px;'></a>"

                        ]);

                    }
                }
            }
            else {

                $("#lblMessage").show();
                $('#lblMessage').html("Registration number is not valid.");
                setTimeout(function () { $("#lblMessage").hide(); }, 10000);
            }
        }
    });
};