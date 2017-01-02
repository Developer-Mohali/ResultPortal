$(document).ready(function () {
    //click firing the function
    $('[data-toggle="tooltip"]').tooltip();
 
//Call The List Function
    TokenList();
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
            TokenList();
            $('#TokenForm').bootstrapValidator('resetForm', true);
            $("#lblUpdateMessages").show();
            $('#lblUpdateMessages').html(data);
            setTimeout(function () { $("#lblUpdateMessages").hide(); }, 5000);
            $('#myModal').modal('toggle'); //or  $('#IDModal').modal('hide');
            return false;
        }
    });
}


//This function use to get value in textbox.
function TokenAdded()
{
    var self = this;
    self.ID = $('#TokenID').val();
    self.TokenName = $("#TokenName").val();
    self.TokenPrice = $("#TokenPrice").val();
    self.NumberOfTimeUse = $("#NumberOfTimeUse").val();
    self.TokenDescription = $("#TokenDescription").val();
    self.TokenPicture = $("#TokenPicture").val();
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
            var oTable = $('#TokenDetailsTable').dataTable();
            oTable.fnClearTable();
            
            for (i = 0; i < data.length; i++)
            {
                $('#TokenDetailsTable').dataTable().fnAddData([
                data[i].RowNumber, data[i].TokenName, data[i].TokenPrice, data[i].NumberOfTimeUse, data[i].TokenDescription, data[i].TokenPicture, "<a data-toggle='modal' data-placement='top' title='Edit' data-target='#myModal' onclick='EditToken(" + data[i].ID + ")' href='#'  class='glyphicon glyphicon-edit' ></a>&nbsp;|&nbsp; <a onclick='DeleteTokenRecords(" + data[i].ID + ")' data-toggle='tooltip' data-placement='top' title='Delete' href='#' class='glyphicon glyphicon-trash' style='color: red;'></a>"
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
                $('#TokenID').val(data.ID);
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
                TokenList();
                $("#lblMessage").show();
                $('#lblMessage').html(data);
                setTimeout(function () { $("#lblMessage").hide(); }, 5000);
            }
        });
    }
}