$(document).ready(function () {
    GetAdminPrfoile();

});


//This function use Get User profile..
function GetAdminPrfoile() {

    $.ajax({
        url: '/Admin/GetAdminPrfoile/',
        type: 'GET',
        dataType: "json",
        success: function (d) {
            //Append for loop row to html table 
            for (var i = 0; i < d.length; i++) {
                if (d[i].Photo == null) {
                    $('#Name').html(d[i].FirstName + ' ' + d[i].LastName);
                    $('#ProfileName').html("<img src=\"/Images/Empty.jpg\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;' />", "<img src='/Images/symbol_check.PNG' alt='Mountain View' style='width:30px;height:25px;margin-left:20px;'>");
                }
                else {
                    $('#Name').html(d[i].FirstName + ' ' + d[i].LastName);
                    $('#ProfileName').html("<img src=\"/StudentPhoto/" + d[i].Photo + "\" alt=\"\" style=' width:40px;border-radius: 100%; height:40px;margin-top:0%;'  />");
                  
                }
            }

        },
    });

}