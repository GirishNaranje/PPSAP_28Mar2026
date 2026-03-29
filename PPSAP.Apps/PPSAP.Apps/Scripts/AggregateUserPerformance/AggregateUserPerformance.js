$(document).ready(function () {
    var year = -1;

    /*if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        year = $('#Year option:selected').val();
    }
    else {
        year = $('#Year option:selected').text();
    }*/

    $.ajax({
        data: { year: year },
        type: 'POST',
        url: '/AggregateUserPerformance/AdminReportDetails',
        beforeSend: function () {
            $('.loader').show(); // Show loading indicator before AJAX request
        },
        success: function (result) {
            var tableBody = $('#AdminReport tbody');
            tableBody.empty(); // Clear existing table rows
            for (var i = 0; i < result.length; i++) {
                var user = result[i];
                var row = '<tr>' +
                    '<td>' + user.SubspecialtyName + '</td>' +
                    '<td>' + user.Correct + '</td>' +
                    '<td>' + user.InCorrect + '</td>' +
                    '<td>' + user.Score + '</td>' +
                    '<td>' + user.UserId + '</td>' +
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
    window.location.href = '/AggregateUserPerformance/ExportToExcel' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;
});