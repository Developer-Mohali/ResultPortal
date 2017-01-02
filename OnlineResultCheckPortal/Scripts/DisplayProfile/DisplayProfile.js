$(document).ready(function () {

    //Assign Value Location to Hidden Field
    $("#OpenTabs").val('Display');

    //Check Account ID 
    NeedStudentID();
    $('#AcademicYear').change(function () {

        $.getJSON('/Display/School/' + $('#AcademicYear').val(), function (data) {
            var items = '<option>----Select School----</option>';
            $.each(data, function (i, state) {
                items += "<option value='" + school.Value + "'>" + school.Text + "</option>";
            });
            $('#School').html(items);
        });
    });
});
function SaveStudentProfileDisplay(controller) {


    var request = new Student();


    $.ajax({
        url: controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        cache: false,
        data: JSON.stringify(request),
        success: function (response) {

            ShowMessage(response.message);
            //Load Top Addrees at Tab
            LoadStudent();
        }
    });


}
function Student() {
    var self = this;

    self.ID = $("#SchoolID").val();
    self.FirstName = $("#FirstName").val();
    self.LastName = $('#LastName').val();;
    self.FatherName = $("#FatherName").val();
    self.Email = $("#Email").val();
    self.Address = $("#Address").val();
    self.Contact = $("#Contact").val();
    self.AcademicYear = $("#AcademicYear").val();
    self.School = $("#School").val();
}