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
    console.log(request);
    $.ajax({
        url: '/Result/ResultDownloadFile',
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (returnValue) {

            window.location = '/Result/DownloadFile?file=' + returnValue;
        }
    });

});


function GetTextboxValue() {
    var self = this;
    self.ExamTypes = $('#DrpExamTypes').val();
    self.Registration = $('#TxtSearchResult').val();
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
                    $('#lblMessage').html("Your registration number not found !");
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
                        d[i].RowNumber, d[i].RegistrationNumber, d[i].FullName, d[i].SubjectName, d[i].Grade, d[i].Remarks, "<a  href='#'  onclick='DeleteUserProfile(" + d[i].ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:15px;height:18px;margin-left:2px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myStudentProfile' onclick='DisplayStudentProfile(" + d[i].ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;margin-left:2px;'></a>"

                        ]);

                    }
                }
            }
            else {

                $("#lblMessage").show();
                $('#lblMessage').html("Please purchase token!");
                setTimeout(function () { $("#lblMessage").hide(); }, 10000);
            }
        }
    });
};