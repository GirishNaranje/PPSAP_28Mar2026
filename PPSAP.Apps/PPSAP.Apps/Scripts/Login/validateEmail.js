$(document).ready(function () {
    $('#SignIn_Button').on('click', function (e) {
        e.preventDefault(); // Prevent form submission

        var email = $('#UserEmail1').val().trim();
        if (!email) {
            $('#UserEmail-error').text('Email is required');
            return;
        }

        if (!isValidEmail(email)) {
            $('#UserEmail-error').text('Invalid email format');
            return;
        }

        var password = $('#passwordInput').val().trim();
        if (!password) {
            $('#UserPassword-error').text('Password is required');
            return;
        }
        var validationResult = isValidPassword(password);
        if (validationResult !== true) {
            $('#UserPassword-error').text(validationResult);
            return;
        }

        $('#passwordInput').closest('form').submit();
    });
});

function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function isValidPassword(password) {
    // Minimum 8 characters
    if (password.length < 8) {
        return "Password must be at least 8 characters long.";
    }

    // Special characters at least 1
    if (!/[!@#$%^&*()_+[\]{};':"\\|,.<>/?]+/.test(password)) {
        return "Password must contain at least 1 special character.";
    }

    // Numerical characters at least 1
    if (!/\d+/.test(password)) {
        return "Password must contain at least 1 numerical character.";
    }

    // Alphabetical characters at least 1
    if (!/[a-zA-Z]+/.test(password)) {
        return "Password must contain at least 1 alphabetical character.";
    }

    // All conditions met
    return true;
}


function clearFields() {
    document.getElementById("UserEmail1").value = "";
    document.getElementById("passwordInput").value = "";
    var errorMessageSpan = document.getElementById("UserEmail-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
    var errorMessageSpan = document.getElementById("UserPassword-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
}

function clearFields_SignUp() {
    document.getElementById("FName").value = "";
    document.getElementById("LName").value = "";
    document.getElementById("Emailid_SignUp").value = "";
    document.getElementById("passwordInput_SignUp").value = "";
    var errorMessageSpan = document.getElementById("FName-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
    var errorMessageSpan = document.getElementById("LName-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
    var errorMessageSpan = document.getElementById("Emailid_SignUp-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
    var errorMessageSpan = document.getElementById("passwordInput_SignUp-error");
    if (errorMessageSpan) {
        errorMessageSpan.textContent = ""; // or innerHTML = "";
    }
}


$(document).ready(function () {
    $('#Create_Button').on('click', function (e) {
        e.preventDefault(); // Prevent form submission

        var FName = $('#FName').val().trim();
        if (!FName) {
            $('#FName-error').text('First Name is required');
            return;
        }

        var LName = $('#LName').val().trim();
        if (!LName) {
            $('#LName-error').text('Last Name is required');
            return;
        }

        var Emailid_SignUp = $('#Emailid_SignUp').val().trim();
        if (!Emailid_SignUp) {
            $('#Emailid_SignUp-error').text('Email is required');
            return;
        }

        if (!isValidEmail(Emailid_SignUp)) {
            $('#Emailid_SignUp-error').text('Invalid email format');
            return;
        }

        var passwordInput = $('#passwordInput_SignUp').val().trim();
        if (!passwordInput) {
            $('#passwordInput_SignUp-error').text('Password is required');
            return;
        }
        var validationResult = isValidPassword(passwordInput);
        if (validationResult !== true) {
            $('#passwordInput_SignUp-error').text(validationResult);
            return;
        }
        /*if (!isValidPassword_SignUp(passwordInput)) {
            $('#passwordInput_SignUp-error').text('Invalid Password format');
            return;
        }*/

        $('#passwordInput_SignUp').closest('form').submit();
    });
});

/*function isValidEmail(email) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}*/

/*function isValidPassword_SignUp(passwordInput) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}*/