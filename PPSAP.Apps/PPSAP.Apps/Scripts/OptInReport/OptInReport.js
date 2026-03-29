$(document).ready(function () {

    var year = -1;

    /*if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        year = $('#Year option:selected').val();
    }
    else {
        year = $('#Year option:selected').text();
    }*/

    $.ajax({
        url: '/OptInReports/GetOptIn',
        type: 'POST',
        data: { year: year },
        success: function (result) {
            $('#TotalUserCount').text(result.TotalUserCount);
            $('#TotalAcceptedUserCount').text(result.TotalOptInAcceptCount);
            $('#TotalDeclineUserCount').text(result.TotalOptInDeclineCount);
        }
    });

    var pgyYear = "";
    $.ajax({
        data: { year: year },
        type: 'GET',
        url: '/OptInReports/OptInReports',
        beforeSend: function () {
            $('.loader').show(); // Show loading indicator before AJAX request
        },
        success: function (result) {
            var tableBody = $('#OptInReport tbody');
            tableBody.empty(); // Clear existing table rows
            for (var i = 0; i < result.length; i++) {
                var user = result[i];
                var row = '<tr>' +
                    '<td>' + user.UserId + '</td>' +
                    '<td>' + user.UserName + '</td>' +
                    '<td>' + user.OptIn + '</td>' +
                    '</tr>';
                tableBody.append(row);
            }
            $('#recordCountMessage').text('Showing 1-' + result.length + ' of ' + result.length);
        },
        complete: function () {
            $('.loader').hide(); // Hide loading indicator after AJAX request is completed
        }
    });

});

$("#Export").on('click', function (event) {
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    var pgyYear = -1;

    /*if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        pgyYear = $('#Year option:selected').val();
    }
    else {
        pgyYear = $('#Year option:selected').text();
    }*/

    if (report.trim() == '') {
        startDate = null;
        endDate = null;
    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];
    }
    window.location.href = '/OptInReports/ExportToExcel' + '?year=' + pgyYear;
});