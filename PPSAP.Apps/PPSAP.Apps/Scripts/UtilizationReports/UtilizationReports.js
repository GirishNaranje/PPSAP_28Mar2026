$(document).ready(function () {
    var year = -1;

    var currentDate = new Date();
    var startDate = new Date(currentDate);
    startDate.setFullYear(currentDate.getFullYear() - 1);
    var endDate = currentDate;

    var pgyYear = "";
    $.ajax({
        url: '/UtilizationReports/AtAGlance',
        type: 'POST',
        data: { year: year, examStartDate: startDate, examCompletedDate: endDate },
        success: function (result) {
            $('#TotalUserCount').text(result);
        }
    });

    var pgyYear = "";
    $.ajax({
        data: { year: year },
        type: 'POST',
        url: '/UtilizationReports/UtilizationReports' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate,
        beforeSend: function () {
            $('.loader').show(); // Show loading indicator before AJAX request
        },
        success: function (result) {
            var tableBody = $('#userTableBody');
            tableBody.empty(); // Clear existing table rows
            for (var i = 0; i < result.length; i++) {
                var user = result[i];
                var row = '<tr>' +
                    '<td>' + user.UserName + '</td>' +
                    '<td>' + user.CustomerId + '</td>' +
                    '<td>' + user.QuestionAnswered + '</td>' +
                    '<td>' + user.QuestionsCustom + '</td>' +
                    '<td>' + user.QuestionsSimulated + '</td>' +
                    '<td>' + user.QuestionsQuick + '</td>' +
                    '<td>' + user.QuestionsChallenged + '</td>' +
                    '<td>' + user.QuestionsSpacedRepetition + '</td>' +
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
    window.location.href = '/UtilizationReports/ExportToExcel' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;


});