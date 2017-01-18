$(document).ready(function () {
    //$('#example').DataTable();
});

//This function use to delete chat message..
function RegistrationSave(controller) {
    alert('d');
    var request = new UserRegister();
    $.ajax({
        url: controller,
        dataType: 'json',
        contentType: "application/json",
        type: "POST",
        data: JSON.stringify(request),
        success: function (d) {
        },
    });
}

function UserRegister() {
    var self = this;
    self.FirstName = $("#txtUserName").val();
    self.Lastname = $("#txtUserSurname").val();
    self.EmailID = $("#txtLogin").val();
    self.ConfirmPassword = $("#txtConfirmPassword").val();

}