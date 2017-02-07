$(document).ready(function () {
    GetSchoolists();
    $('[data-toggle="tooltip"]').tooltip();
    //Call The Get List .//
   
});
   

function GetSchoolists()
{
    var table = $("#SchoolDetailsTable").DataTable({
        scrollY: false,
        paging: true,
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: true,  // this is for disable filter (search box)
        orderMult: false, // for disable multiple column at once
        ajax: {
            url: '/ManageSchool/GetSchoolLIst',
            type: "POST",
            datatype: "json"
        }, 
        columns: [
                { data: "RowNumber", "autoWidth": true },
                { data: "SchoolName", "autoWidth": true },
                { data: "Address", "autoWidth": true },
                { data: "Zipcode", "autoWidth": true },
                { data: "Zone", "autoWidth": true },
                {
                    data: null,
                    className: "center",
                    "render": function (data, type, row) {
                        var inner = null;
                        inner = "<a data-toggle='modal' data-target='#myModal' data-placement='top' title='Edit' onclick='EditSchoolDetail(" + row.ID + ")' href='#' class='glyphicon glyphicon-edit' ></a>&nbsp;|&nbsp;<a data-toggle='tooltip' data-placement='top' title='Delete' onclick='DeleteRecords(" + row.ID + ")' href='#'  class='glyphicon glyphicon-trash' style='color: red;'></a>"
                     
                        return inner;
                    }
                }

        ]
    });
    setInterval(function () {
        table.ajax.reload();
    }, 10000)
}

function AddSchool(Controller) {
    var request = new GetvalueAddSchool();
         $.ajax({
             url: Controller,
             dataType: 'json',
             contentType: "application/json",
             type: "POST",
             data: JSON.stringify(request),
             success: function (response) {
                 $('#ManageSchoolValidation').bootstrapValidator('resetForm', true);
                 $("#lblUpdateMessages").show();
                 $('#lblUpdateMessages').html(response);
                 setTimeout(function () { $("#lblUpdateMessages").hide(); }, 5000);
                 $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
                 return false;
             }
         });
     }

//Add the school details ...
function GetvalueAddSchool()
{
    var self = this;
    self.ID = $("#SchoolID").val()
    self.SchoolName = $("#SchoolName").val()
    self.Address = $("#Address").val()
    self.Zipcode = $("#Zipcode").val()
    self.Zone = $("#Zone").val()

}


//Display The School List.//

//function GetSchoolList()
//{
//    $.ajax({
//        url: '/ManageSchool/GetSchoolLIst',
//        dataType: 'json',
//        contentType: "application/json",
//        type: "POST",
//        success: function (data) {
//            var oTable = $('#SchoolDetailsTable').dataTable();
//            oTable.fnClearTable();
//            for (i = 0; i < data.length; i++)
//            {
//                $('#SchoolDetailsTable').dataTable().fnAddData([
//                data[i].RowNumber, data[i].SchoolName, data[i].Address, data[i].Zipcode, data[i].Zone, "<a data-toggle='modal' data-target='#myModal' data-placement='top' title='Edit' onclick='EditSchoolDetail(" + data[i].ID + ")' href='#' class='glyphicon glyphicon-edit' ></a>&nbsp;|&nbsp;<a data-toggle='tooltip' data-placement='top' title='Delete' onclick='DeleteRecords(" + data[i].ID + ")' href='#'  class='glyphicon glyphicon-trash' style='color: red;'></a>"

//                ]);
              
//            }
//        }
//    });

//}

//Edit School Details By Json Process.//

function EditSchoolDetail(schoolId)
{
    $.ajax({
        url: '/ManageSchool/EditSchoolDetails',
        dataType: 'json',
        type: "POST",
        data: { 'schoolId': schoolId },
        success: function (data) {
            $.each(data, function (i, data) {
                $('#SchoolID').val(data.ID);
                $('#SchoolName').val(data.SchoolName);
                $('#Address').val(data.Address);
                $('#Zipcode').val(data.Zipcode);
                $('#Zone').val(data.Zone);
            });

        },
    });

}




//Delete School Records By schoolId.//

function DeleteRecords(schoolId)
{
    if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/ManageSchool/DeleteSchoolRecords',
            dataType: 'json',
            type: "POST",
            data: { 'schoolId': schoolId },
            success: function (data) {
                $("#lblMessage").show();
                $('#lblMessage').html(data);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
            }
        });

    }
}