$(document).ready(function () {
    PurchaseToken();
    $('[data-toggle="tooltip"]').tooltip();

});

function PurchaseToken() {
    var table= $("#UserDetailsTable").DataTable({
        searching: true,
        processing: true, // for show progress bar
        serverSide: true, // for process server side
        filter: false, // this is for disable filter (search box)
        orderMult: false, // for disable multiple column at once
        ajax: {
            url: '/PurchaseToken/PurchaseTokenDetails/',
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
                        var inner = null;
                       
                        if (row.PurchaseToken == true) {
                             
                            inner = "<a href='#' onclick='UnPurchaseToken(" + row.ID + ")'><img src='/Images/symbol_check.PNG' data-toggle='tooltip' title='Purchased token' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>"
                            
                        }
                        else {
                            
                             inner = "<a href='#' id='Approved' onclick='ApprovedPurchaseToken(" +row.ID+ ")'/><img src='/Images/iconvalide.PNG' data-toggle='tooltip' title='Not purchase token!' alt='Mountain View' style='width:20px;height:20px;margin-left:24px;'></a>"
                          
                        }
                        //var inner = "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile(" + row.ID + ")' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile(" + row.ID + ")'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile(" + row.ID + ")'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;'></a>"
                        //defaultContent: "<a href='#''  data-placement='top' title='Edit'  onclick='EditUserProfile()' class='glyphicon glyphicon-edit' data-toggle='modal' data-target='#myModal' alt='Mountain View' style='width:15px;height:20px;'></a>&nbsp;|&nbsp;<a  href='#'  data-toggle='tooltip' data-placement='top' title='Delete'  onclick='DeleteUserProfile()'class='glyphicon glyphicon-trash' alt='Mountain View' style='width:13px;height:18px;color: red;'></a>&nbsp;|&nbsp;<a href='#'  data-toggle='modal' data-target='#myViewStudentProfile' data-toggle='tooltip' data-placement='top' title='Update student profile' onclick='DisplayStudentProfile( )'/><img src='/Images/details-icon-png-cc-by-3-0--it-1.PNG' alt='Mountain View' style='width:17px;height:15px;'></a>"
                        return inner;
                    }
                }

        ]
    });
    setInterval(function () {
        table.ajax.reload();
    }, 20000)
}


//This function use to Get User list..
function UserProfile() {

    $.ajax({
        url: '/PurchaseToken/PurchaseTokenDetails/',
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
                     d[i].RowNumber, d[i].StudentID, d[i].fullName, d[i].CreateDate, d[i].Gender, d[i].LocalGovernment, d[i].State, d[i].SchoolName, "<img src=\"/StudentPhoto/" + ProfileImage + "\" class='img-thumbnail' style=' width:50px; height:50px;margin-top:7%;' />", "<a href='#' onclick='UnPurchaseToken(" + d[i].ID + ")'><img src='/Images/symbol_check.PNG' data-toggle='tooltip' title='Purchased token' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>",

                    ]);
                }
                else {
                    $('#UserDetailsTable').dataTable().fnAddData([
                    d[i].RowNumber, d[i].StudentID, d[i].fullName, d[i].CreateDate, d[i].Gender, d[i].LocalGovernment, d[i].State, d[i].SchoolName, "<img src=\"/StudentPhoto/" + ProfileImage + "\" class='img-thumbnail'  style=' width:50px; height:50px;margin-top:7%;' />", "<a href='#' id='Approved' onclick='ApprovedPurchaseToken(" + d[i].ID + ")'/><img src='/Images/iconvalide.PNG' data-toggle='tooltip' title='Not purchase token!' alt='Mountain View' style='width:20px;height:20px;margin-left:24px;'></a>",
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
               
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);
                //PurchaseToken();
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
                //UserProfile();
                $("#lblMessages").show();
                $('#lblMessages').html(d);
                setTimeout(function () { $("#lblMessages").hide(); }, 10000);
                //PurchaseToken();

            },
        });
    }

}
