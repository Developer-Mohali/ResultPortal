$(document).ready(function () {
    TokenDetails();
   //click firing the function
    $('[data-toggle="tooltip"]').tooltip();
   
});


//This function use to add new token.
function addnewRecorToken(controller) {
    var request = new TokenAdded();
    $.ajax({
        url: controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (data) {
            if (data == "Already Exists") {
                $("#lblMessages").show();
                $('#lblMessages').html("Token ID already exists.");
                setTimeout(function () { $("#lblMessages").hide(); }, 5000);
            } else {
             
                $("#lblUpdateMessages").show();
                $('#lblUpdateMessages').html(data);
                $('#TokenForm').bootstrapValidator('resetForm', true);
                setTimeout(function () { $("#lblUpdateMessages").hide(); }, 5000);
                $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
                return false;
               
            }
        }
    });
}


//This function use to get value in textbox.
function TokenAdded()
{
    var self = this;
    self.ID = $("#TokenID").val();
    self.TokenID = $("#TokenId").val();
    self.TokenName = $("#TokenName").val();
    self.TokenPrice = $("#TokenPrice").val();
    self.NumberOfTimeUse = $("#NumberOfTimeUse").val();
    self.TokenDescription = $("#TokenDescription").val();
   
}

function TokenDetails() {
    var $myTable = $("#TokenTable");
    $myTable.dataTable().fnDestroy();
    var table = $myTable.DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Token/TokenList",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
                    { data: "RowNumber", "RowNumber": "RowNumber", "autoWidth": true },
                    { data: "TokenID", "TokenID": "TokenID", "autoWidth": true },
                    { data: "TokenName", "TokenName": "TokenName", "autoWidth": true },
                    { data: "TokenPrice", "TokenPrice": "TokenPrice", "autoWidth": true },
                    { data: "NumberOfTimeUse", "NumberOfTimeUse": "NumberOfTimeUse", "autoWidth": true },
                    { data: "TokenDescription", "TokenDescription": "TokenDescription", "autoWidth": true },

                    {
                        data: null,
                        className: "center",
                        "render": function (data, type, row) {
                            var inner = "<a data-toggle='modal' data-placement='top' title='Edit' data-target='#myModal' onclick='EditToken(" + row.ID + ")' href='#'  class='glyphicon glyphicon-edit' ></a>&nbsp;|&nbsp; <a onclick='DeleteTokenRecords(" + row.ID + ")' data-toggle='tooltip' data-placement='top' title='Delete' href='#' class='glyphicon glyphicon-trash' style='color: red;'></a>"
                            //defaultContent: "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile()' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile()'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile( )'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;'></a>"
                            return inner;
                   
                        }


                    }

        ],
       
    });
   
}



//Display To The Token List
function TokenList()
{
    $.ajax({
        url: '/Token/TokenList',
        dataType: 'json',
        type: "POST",
        contentType: "application/json",
        success: function (data) {
            var oTable = $('#TokenTable').dataTable();
            oTable.fnClearTable();
            
            for (i = 0; i < data.length; i++)
            {
                $('#TokenTable').dataTable().fnAddData([
                data[i].RowNumber,data[i].TokenID, data[i].TokenName, data[i].TokenPrice, data[i].NumberOfTimeUse, data[i].TokenDescription,"<a data-toggle='modal' data-placement='top' title='Edit' data-target='#myModal' onclick='EditToken(" + data[i].ID + ")' href='#'  class='glyphicon glyphicon-edit' ></a>&nbsp;|&nbsp; <a onclick='DeleteTokenRecords(" + data[i].ID + ")' data-toggle='tooltip' data-placement='top' title='Delete' href='#' class='glyphicon glyphicon-trash' style='color: red;'></a>"
            ])
            }
        }
    });
}

//Edit The Token List By json method & ID
function EditToken(tokenId)
{
    $.ajax({
        url: '/Token/EditList/',
        dataType: 'json',
        type: "POST",
        data: { 'tokenId': tokenId },
        success: function (data) {
            $.each(data, function (i, data) {
                $("#TokenId").attr("disabled", "disabled");
                $('#TokenID').val(data.ID);
                $('#TokenId').val(data.TokenID);
                $('#TokenName').val(data.TokenName);
                $('#TokenPrice').val(data.TokenPrice);
                $('#NumberOfTimeUse').val(data.NumberOfTimeUse);
                $('#TokenDescription').val(data.TokenDescription);
                $('#TokenPicture').val(data.TokenPicture);
            });

        }
    });
}

//this isdelete token by id
function DeleteTokenRecords(tokenId)
{
  if (confirm("Are you sure you want to delete Record?")) {
        $.ajax({
            url: '/Token/DeleteTokenDetails',
            dataType: 'json',
            type: "POST",
            data: { tokenId: tokenId },
            success: function (data) {
             
                $("#lblMessage").show();
                $('#lblMessage').html(data);
                setTimeout(function () { $("#lblMessage").hide(); }, 10000);
                TokenDetails();
            }
        });
    }
}


