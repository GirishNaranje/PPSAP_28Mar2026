$(document).ready(function () {
    $("#Login_btnLogin").click(function (e) {

        e.preventDefault();
        var UserEmail = $("#Login_Email").val();
        var password = $("#Login_password").val();
        alert("UserEmail: " + UserEmail)
        alert("password: " + password)
        $.ajax({
            data: { UserEmail: UserEmail, Password: password },
            url: '/Login/SignIn',
            success: function (result) {
                if (result) {
                    // Redirect the user to another page
                    alert("result: " + result);

                } else {
                    alert("else");
                }
            }
        });
    });

    $(".toggle-password").click(function () {
        var passwordInput = $("#passwordInput");
        var type = passwordInput.attr("type") === "password" ? "text" : "password";
        passwordInput.attr("type", type);
        $(this).find('i').toggleClass('fa-eye fa-eye-slash');
    });

    $(".toggle-password").click(function () {
        var passwordInput = $("#passwordInput_SignUp");
        var type = passwordInput.attr("type") === "password" ? "text" : "password";
        passwordInput.attr("type", type);
        $(this).find('i').toggleClass('fa-eye fa-eye-slash');
    });
});
