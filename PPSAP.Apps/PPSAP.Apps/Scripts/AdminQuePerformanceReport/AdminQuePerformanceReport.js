$(document).ready(function () {
    var year = -1;
    var Records = 25;
    var PageNo = 1;

    $("#showOptions").change(function () {
        Records = $(this).val();
        console.log("Selected value:", Records);
        makeAjaxCall();
    });

    $("#pageOptions").change(function () { // Corrected ID here
        PageNo = $(this).val();
        console.log("Selected value (PageNo):", PageNo);
        makeAjaxCall();
    });


    // Initial AJAX call without dropdown value
    makeAjaxCall();

    function makeAjaxCall() {
        // Making AJAX call with updated Records value
        $.ajax({
            data: { year: year, NoOfRecords: Records, PageNo: PageNo },
            type: 'POST',
            url: '/AdminQuestionPerformance/AdminQuestionPerformanceDetails',
            beforeSend: function () {
                $('.loader').show(); // Show loading indicator before AJAX request
            },
            success: function (result) {
                var tableBody = $('#questionPerformanceTableBody');
                var countElement = $('#countDisplay'); // Assuming you have an element with id 'countDisplay' to display the count
                tableBody.empty(); // Clear existing table rows
                for (var i = 0; i < result.length; i++) {
                    var question = result[i];
                    var row = '<tr>' +
                        '<td>' + question.Stem + '</td>' +
                        '<td>' + question.QuestionId + '</td>' +
                        '<td>' + question.Subspecialty + '</td>' +
                        '<td>' + question.Chapter + '</td>' +
                        '<td>' + question.Topic + '</td>' +
                        '<td>' + question.UserAnsweredcorrectly + '</td>' +
                        '<td>' + question.UserAnsweredincorrectly + '</td>' +
                        '<td>' + question.UsersRepeatedSRMode + '</td>' +
                        '<td>' + question.Percentagecorrectly + '</td>' +
                        '<td>' + question.PercentageIncorrectly + '</td>' +
                        '<td>' + question.CorrectAnswer + '</td>' +
                        '<td>' + question.Distractor_1 + '</td>' +
                        '<td>' + question.User_chose_distractor_1 + '</td>' +
                        '<td>' + question.Distractor_2 + '</td>' +
                        '<td>' + question.User_chose_distractor_2 + '</td>' +
                        '<td>' + question.Distractor_3 + '</td>' +
                        '<td>' + question.User_chose_distractor_3 + '</td>' +
                        '<td>' + question.Avg_times_incorrect_before_correct + '</td>' +
                        '<td>' + question.Users_correct_first_try + '</td>' +
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
    var pgyYear = -1;

    if (report.trim() == '') {
        startDate = null;
        endDate = null;
    }
    else {
        var dateParts = report.split('To');
        startDate = dateParts[0];
        endDate = dateParts[1];
    }
    window.location.href = '/AdminQuestionPerformance/ExportToExcel' + '?year=' + pgyYear + '&examStartDate=' + startDate + '&examCompletedDate=' + endDate;

});
