$(document).ready(function () {
    UserProfile()
    $('[data-toggle="tooltip"]').tooltip();
});




//This function use to Get User list..
function UserProfile() {

    $.ajax({
        url: '/PurchaseToken/UserProfile/',
        type: 'GET',
        enctype: "multipart/form-data",
        dataType: "json",
        success: function (d) {
            var oTable = $('#UserDetailsTable').dataTable();
            oTable.fnClearTable();
            //Append for loop row to html table
            for (var i = 0; i < d.length; i++) {
                var ProfileImage;
                if (d[i].Picture == null) {
                    ProfileImage = "NotAvailable.jpg";


                }
                else {
                    ProfileImage = d[i].Picture;

                }

                if (d[i].PurchaseToken == true) {
                    $('#UserDetailsTable').dataTable().fnAddData([
                     d[i].RowNumber,d[i].StudentID, d[i].FirstName, d[i].LastName, d[i].EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\" class='img-thumbnail' style=' width:50px; height:50px;margin-top:7%;' />", "<a href='#' onclick='UnPurchaseToken(" + d[i].ID + ")'><img src='/Images/symbol_check.PNG' data-toggle='tooltip' title='Purchased token' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>", 

                    ]);
                }
                else {
                    $('#UserDetailsTable').dataTable().fnAddData([
                    d[i].RowNumber, d[i].StudentID, d[i].FirstName, d[i].LastName, d[i].EmailID, "<img src=\"/StudentPhoto/" + ProfileImage + "\" class='img-thumbnail'  style=' width:50px; height:50px;margin-top:7%;' />", "<a href='#' id='Approved' onclick='ApprovedPurchaseToken(" + d[i].ID + ")'/><img src='/Images/iconvalide.PNG' data-toggle='tooltip' title='Not purchase token!' alt='Mountain View' style='width:20px;height:20px;margin-left:24px;'></a>",
                    ]);
                }
            }


        }
    });

}


//This function use to Approved Register User.
function ApprovedPurchaseToken(userID) {
    if (confirm("Are you sure you want to Purchase token ?")) {
        $.ajax({
            url: '/PurchaseToken/StudentPurchaseToken/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                UserProfile();
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);

            },
        });
    }
}

//This function use to Approved Register User.
function UnPurchaseToken(userID) {
    if (confirm("Are you sure you want to UnApproved?")) {
        $.ajax({
            url: '/PurchaseToken/StudentUnPurchaseToken/',
            dataType: 'json',
            type: "POST",
            data: { 'userID': userID },
            success: function (d) {
                UserProfile();

                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);

            },
        });
    }

}
