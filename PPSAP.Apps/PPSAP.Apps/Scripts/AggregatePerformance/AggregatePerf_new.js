$(document).ready(function () {
    // Get current date
    var currentDate = new Date();

    // Get date one year ago
    var oneYearAgo = new Date();
    oneYearAgo.setFullYear(oneYearAgo.getFullYear() - 1);

    // Format dates as per your requirements
    var examStartDate = formatDate(oneYearAgo);
    var examCompletedDate = formatDate(currentDate);

    // Function to format date as 'YYYY-MM-DD'
    function formatDate(date) {
        var year = date.getFullYear();
        var month = (date.getMonth() + 1).toString().padStart(2, '0');
        var day = date.getDate().toString().padStart(2, '0');
        return year + '-' + month + '-' + day;
    }

    //alert(examStartDate +" - "+ examCompletedDate);

    // Call the function to populate the table
    fetchData(examStartDate, examCompletedDate);

    function fetchData(examStartDate, examCompletedDate) {
        $.ajax({
            data: { examStartDate: examStartDate, examCompletedDate: examCompletedDate },
            type: 'POST',
            url: '/AggregatePerformance/ReportDetails',
            beforeSend: function () {
                $('.loader').show(); // Show loading indicator before AJAX request
            },
            success: function (result) {
                var tableBody = $('#ResidentReport tbody');
                tableBody.empty(); // Clear existing table rows
                for (var i = 0; i < result.length; i++) {
                    var user = result[i];
                    var row = '<tr>' +
                        '<td>' + user.SubspecialtyName + '</td>' +
                        '<td>' + user.Score + '</td>' +
                        '<td>' + user.Correct + '</td>' +
                        '<td>' + user.InCorrect + '</td>' +
                        '</tr>';
                    tableBody.append(row);
                }
                $('#recordCountMessage').text('Showing 1-' + result.length + ' of ' + result.length);
            },
            complete: function () {
                $('.loader').hide(); // Hide loading indicator after AJAX request is completed
            }
        });
    }

});

$("#Export").on('click', function (event) {
    var startDate;
    var endDate;
    var report = $('#reportrange').text();
    var pgyYear;

    if ($('#Year option:selected').text() == 'All' || $('#Year option:selected').text() == 'Non-Resident') {
        pgyYear = $('#Year option:selected').val();
    }
    else {
        pgyYear = $('#Year option:selected').text();
    }

    if (report.trim() == '') {
        startDate = null;
        endDate = null;
    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];
    }
    window.location.href = '/AggregatePerformance/ExportToExcel' + '?year=' + pgyYear;
});

