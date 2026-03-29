$(document).ready(function () {

    $.ajax({
        type: 'POST',
        url: '/NotificationMessageSetting/GetAllNotificationMessageSetting',
        beforeSend: function () {
            $('#loadingIndicator').show(); // Show loading indicator before AJAX request
        },
        success: function (result) {
            var tableBody = $('#NotificationMessageSettingTable tbody');
            tableBody.empty(); // Clear existing table rows
            for (var i = 0; i < result.length; i++) {
                var user = result[i];
                var isUnableDisplay = user.IsUnable ? 'Yes' : 'No'; // Conditionally display Yes or No
                var row = '<tr>' +
                    '<td>' + user.NotificationMessage + '</td>' +
                    '<td>' + user.NoOfTime + '</td>' +
                    /*'<td>' + isUnableDisplay + '</td>' +*/
                    '<td>' +
                    '<button class="btn btn-primary edit-btn" data-id="' + user.NotificationMessageSettingId + '">Edit</button>' + // Edit button
                    '<button class="btn btn-danger delete-btn" data-id="' + user.NotificationMessageSettingId + '">Delete</button>' + // Delete button
                    '</td>' +
                    '</tr>';
                tableBody.append(row);
            }
            $('#recordCountMessage').text('Showing 1-' + result.length + ' of ' + result.length + ' records');
        },
        complete: function () {
            $('#loadingIndicator').hide(); // Hide loading indicator after AJAX request is completed
        }
    });    

    // Attach event listener to edit buttons using event delegation
    $('#NotificationMessageSettingTable').on('click', '.edit-btn', function () {
        var id = $(this).data('id');
        openMessageSettingForm(id);
    });

    $("#AddNewMsg").click(function () {
        openMessageSettingForm(0);
    });

    // Function to open the message setting form
    function openMessageSettingForm(id) {
        $.get("/NotificationMessageSetting/GetMessageSettingForm?id=" + id).done(function (response) {
            $("#MessageSettingBody").html(response);
            $('#MessageSettingModal').modal('show');
        });
    }
});



/*$("#AddNewMsg").click(function () {
    $.ajax({
        url: "/NotificationMessageSetting/GetMessageSettingForm",
        data: { id : 0 },
        dataType: "json", // expected data type from the server
        success: function (response) {
            // Handle the successful response from the server
            console.log("Data received:", response);
            // Example: Update HTML element with received data
            $("#result").text(response.message);
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error("Error:", status, error);
        }
    });
    //openMessageSettingForm(0); // Call the function with the desired ID
})*/;

// Attach event listener to edit buttons using event delegation
$('#NotificationMessageSettingTable').on('click', '.delete-btn', function () {
    var id = $(this).data('id');
    deleteMessageSetting(id);
});

function deleteMessageSetting(_id) {
    $.ajax({
        type: 'post',
        async: false,
        data: { id: _id },
        url: '/NotificationMessageSetting/DeleteMessageSetting',
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            if (res.isSucess) {
                createAlert('Success', '', res.msg, 'success', true, true, 'pageMessages');
                //createNotification(res.msg, "success");
            } else {
                createAlert('Error', '', res.msg, 'danger', true, true, 'pageMessages');
                // createNotification(res.msg, "error");
            }
            setTimeout(function () {
                window.location.reload();
            }, 1000);
        },
        error: function (err) {
            createAlert('Error', '', 'Error while processing message settings.', 'danger', true, true, 'pageMessages');
            //createNotification("Error while processing message settings.", "error");
            console.log(err);
        }
    })
}

saveNotificationMessageSetting = form => {
    var isEnable = $("#IsUnable_Value").is(":checked") ? 1 : 0;
    var notificationMessageSettingId = $("#NotificationMessageSettingId").val();
    var notificationMessage = $("#NotificationMessage").val();
    var noOfTime = $("#NoOfTime").val();
    var onholdScreenTime = 0;
    $.validator.unobtrusive.parse(form);
    /*if ($(form).valid()) {*/
        try {
            $.ajax({
                type: 'post',
                async: false,
                data: JSON.stringify({ NotificationMessageSettingId: notificationMessageSettingId, NotificationMessage: notificationMessage, NoOfTime: noOfTime, OnholdScreenTime: onholdScreenTime, IsEnable: isEnable }),
                url: '/NotificationMessageSetting/SaveNotificationMessageSetting',
                // contentType: "application/x-www-form-urlencoded",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.isSucess) {
                        if (notificationMessageSettingId > 0) {
                            createAlert('Success', '', 'Message settings edit successfully.', 'success', true, true, 'pageMessages');
                            //createNotification("Message settings edit successfully.", "success");
                        } else {
                            createAlert('Success', '', 'Message settings save successfully.', 'success', true, true, 'pageMessages');
                            //createNotification("Message settings save successfully.", "success");
                        }

                        $('#MessageSettingModal').modal('hide');
                        $('#NotificationMessageSettingTable').DataTable().ajax.reload()
                    }
                    else {
                        // $('#form-modal .modal-body').html(res.html);
                    }
                },
                error: function (err) {
                    createAlert('Error', '', 'Error while processing message settings.', 'danger', true, true, 'pageMessages');
                    //createNotification("Error while processing message settings.", "error");
                    console.log(err);
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    /*} else {
        return false;
    }*/
}
function limit(element, max_chars) {
    if (element.value.length > max_chars) {
        element.value = element.value.substr(0, max_chars);
    }
}