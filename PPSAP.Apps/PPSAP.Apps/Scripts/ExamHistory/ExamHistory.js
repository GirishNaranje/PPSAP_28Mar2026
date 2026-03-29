(function () {
    var Status = $('#Status option:selected').val();
    var Filter = "All";
    var i = 0;
    $('#ExamHistoryTable').DataTable({
        "language": {
            "lengthMenu": "Show _MENU_ exams",
            "sEmptyTable": "No Records found",
            "info": "Showing _START_ - _END_ of _TOTAL_ entries",
            "paginate": {
                "previous": "Prev"
            }
        },
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "scrollX": false,
        "responsive": true,
        "ajax": {
            "data": { Status: Status },
            "url": '/ExamHistory/ExamHistoryDetails',
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            {
                data: null, "bSortable": false, render: function (data, type, row) {
                    if (data.ExamStatus == 2 && data.ExamTimeType == true) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>' + '<i class="fa fa-clock-o marleft10" style="color:#337ab7"></i>';
                    }
                    if (data.ExamStatus == 2 && data.ExamMode == 1) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>' + '<i class="fa fa-flag-checkered marleft10" style="color:#337ab7"></i>';
                    }
                    if (data.ExamStatus == 2) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>'
                    }
                    else {
                        if (data.ExamTimeType == true) {
                            return data.ExamName + '<i class="fa fa-clock-o marleft10"></i>'
                        }
                        if (data.ExamMode == 1) {
                            return data.ExamName + '<i class="fa fa-flag-checkered marleft10"></i>'
                        }
                        return data.ExamName;
                    }
                }
            },

            { "data": "ExamType", "bSortable": false },
            {

                data: null, "bSortable": false, render: function (data, type, row) {
                    var width = 0;
                    width = Math.round(data.QuestionAttempt / data.NoofQuestions * 100);
                    if (data.ExamStatus == 2) {

                        return '<div class="progress-custom"><div class="progress"><div class="progress-bar progress-green"  style="width: 100%;" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div></div></div>' + '<div class="progress-value" style="font-size: 12px;">' + data.QuestionAttempt + ' /' + data.NoofQuestions + '</div>';
                    }
                    else {
                        return '<div class="progress-custom"><div class="progress"><div class="progress-bar progress-orange"  style="width: ' + width + '%;" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div></div></div>' + '<div class="progress-value" style="font-size: 12px;">' + data.QuestionAttempt + ' /' + data.NoofQuestions + '</div>';
                    }
                }
            },
            {
                data: null, "bSortable": false, render: function (data, type, row) {
                    return data.Score + '%';
                }
            },
            {
                "data": "ExamCreatedDate",
                targets: 0,
                "bSortable": false,
                "render": function (data) {
                    return moment(data, "DD/MM/YYYY").format('MMM DD YYYY');
                }
            },


            {
                data: null, "bSortable": false, render: function (data, type, row) {

                    if (data.ExamStatus == 2) {
                        return new moment(data.ExamLastAttemptDate, "DD/MM/YYYY").format('MMM DD YYYY');

                    }
                    else {
                        return ""
                    }

                }
            },
            {
                data: null, "bSortable": false, render: function (data, type, row) {
                    if (data.ExamStatus == 1) {
                        return '<a href="/Assessment/Index?ExamId=' + data.ExamId + '&Questionid=' + data.LastQuestionId + '" class="editAsset marright10"><i class="fa fa-arrow-right text-center"></i></a> <a href="#" examId=' + data.ExamId + ' class="editAsset" id="delete";><i class="fa fa-trash-o text-center deleteLink"></i></a>';
                    }
                    if (data.ExamStatus == 2) {
                        return '<a href="#" examId=' + data.ExamId + ' class="editAsset marright10" id="share"><i class="fa fa-envelope text-center"></i></a> <a href="#" examId=' + data.ExamId + ' class="editAsset" id="delete";><i class="fa fa-trash-o text-center"></i></a>';

                    }
                    if (data.ExamStatus == 0) {
                        return null;
                    }
                }
            }
        ],
    });
})();

$("#Reset").on('click', function () {
    getConfirmations();
    function getConfirmations() {
        var retVal = $("#resetExam").modal("show");
        $(".deleted").click(function () {
            $.ajax({
                type: 'POST',
                url: '/ExamHistory/ResetExam',
                success: function (result) {
                    if (result == 0) {
                        window.location.reload();
                    }
                }
            });
        });
    }
});

$("#NoteBook").on('click', function () {
    document.location = Url.Action("Index", "NoteBook");
});

(function () {
$('#Status').on('change', function () {
    $('#ExamHistoryTable').dataTable().fnDestroy();
    var Status = $('#Status option:selected').val();
    var Filter = $('#Status option:selected').val();
    $('#ExamHistoryTable').DataTable({
        "language": {
            "lengthMenu": "Show _MENU_ exams",
            "sEmptyTable": "No Records found",
            "info": "Showing _START_ - _END_ of _TOTAL_ entries",
            "paginate": {
                "previous": "Prev"
            }
        },
        "processing": true,
        "serverSide": true,
        "filter": false,
        "orderMulti": false,
        "scrollX": false,
        "responsive": true,
        "ajax": {
            "data": { Status: Status },
            "url": '/ExamHistory/ExamHistoryDetails',
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            {

                data: null, "bSortable": false, render: function (data, type, row) {

                    if (data.ExamStatus == 2 && data.ExamTimeType == true) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>' + '<i class="fa fa-clock-o marleft10" style="color:#337ab7"></i>';

                    }
                    if (data.ExamStatus == 2 && data.ExamMode == 1) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>' + '<i class="fa fa-flag-checkered marleft10" style="color:#337ab7"></i>';
                    }
                    if (data.ExamStatus == 2) {
                        return '<a href="/ViewAssessment/FilterByQuestions?ExamId=' + data.ExamId + '&Filter=' + Filter + '" class="editAsset">' + data.ExamName + '</a>'
                    }
                    else {
                        if (data.ExamTimeType == true) {
                            return data.ExamName + '<i class="fa fa-clock-o marleft10"></i>'
                        }
                        if (data.ExamMode == 1) {
                            return data.ExamName + '<i class="fa fa-flag-checkered marleft10"></i>'
                        }
                        return data.ExamName;
                    }
                }
            },

            { "data": "ExamType", "bSortable": false },
            {
                data: null, "bSortable": false, render: function (data, type, row) {
                    var width = 0;
                    width = Math.round(data.QuestionAttempt / data.NoofQuestions * 100);
                    if (data.ExamStatus == 2) {
                        return '<div class="progress-custom"><div class="progress"><div class="progress-bar progress-green"  style="width: 100%;" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div></div></div>' + '<div class="progress-value" style="font-size: 12px;">' + data.QuestionAttempt + ' /' + data.NoofQuestions + '</div>';
                    }
                    else {
                        return '<div class="progress-custom"><div class="progress"><div class="progress-bar progress-orange"  style="width: ' + width + '%;" role="progressbar" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div></div></div>' + '<div class="progress-value" style="font-size: 12px;">' + data.QuestionAttempt + ' /' + data.NoofQuestions + '</div>';
                    }
                }
            },
            {
                data: null, "bSortable": false, render: function (data, type, row) {

                    return data.Score + '%';
                }
            },
            {
                "data": "ExamCreatedDate",
                targets: 0,
                "bSortable": false,
                "render": function (data) {
                    return new moment(data, "DD/MM/YYYY").format('MMM DD YYYY');

                }
            },

            {
                data: null, "bSortable": false, render: function (data, type, row) {

                    if (data.ExamStatus == 2) {
                        return new moment(data.ExamLastAttemptDate, "DD/MM/YYYY").format('MMM DD YYYY');

                    }
                    else {
                        return " "
                    }
                }
            },

            {
                data: null, "bSortable": false, render: function (data, type, row) {

                    if (data.ExamStatus == 1) {
                        return '<a href="/Assessment/Index?ExamId=' + data.ExamId + '&Questionid=' + data.LastQuestionId + '" class="editAsset marright10"><i class="fa fa-arrow-right text-center"></i></a>  <a href="#"  examId=' + data.ExamId + ' class="editAsset" id="delete"><i class="fa fa-trash-o text-center deleteLink"></i></a>';

                    }
                    if (data.ExamStatus == 2) {
                        return '<a href="#" examId=' + data.ExamId + ' class="editAsset marright10" id="share"><i class="fa fa-envelope text-center"></i></a>  <a href="#"  examId=' + data.ExamId + ' class="editAsset" id="delete"  ><i class="fa fa-trash-o text-center"></i></a>';

                    }
                    if (data.ExamStatus == 0) {
                        return null;
                    }
                }
            }
        ],
    });
});
}) ();

/*$('#Status').on('change', function () {
    var selectedValue = $('#Status').val();
    console.log("selectedValue" + selectedValue);
    $.ajax({
        "type": "POST",
        data: { Status: selectedValue },
        url: "/ExamHistory/ExamHistoryDetails",
        success: function (result) {
            console.log("success");
        }
    });
});*/




$('#ExamHistoryTable tbody').on('click', '#share', function () {
    var myBookId = $(this).attr('examId');
    $(".modal-body #feedbackexamId").val(myBookId);
    $("#error").css("display", "none");
    $("#error1").css("display", "none");
    $(".modal-body #email").val('');
    $('#DescModal').modal("show");
});

$('#ExamHistoryTable tbody').on('click', '#delete', function () {
    var ExamId = $(this).attr('examId');
    getConfirmation(ExamId);
    function getConfirmation(ExamId) {
        var retVal = $("#deleteButton").modal("show");
        $(".deleted").click(function () {
            window.location.href = '/ExamHistory/DeleteExamHistoryDetails?ExamId=' + ExamId;
        });
    }
});

function trim(str, chars) {
    return ltrim(rtrim(str, chars), chars);
}
function ltrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}
function rtrim(str, chars) {
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}
//I modified the regex.
function validateEmail(field) {
    //var regex=/\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b/i;
    var regex = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    return (regex.test(trim(field))) ? true : false;
}
function validateMultipleEmailsCommaSeparated(value) {
    var result = value.split(",");
    for (var i = 0; i < result.length; i++)
        if (!validateEmail(result[i]))
            return false;
    return true;
}

$(".sharefeedback").click(function () {
    var dateTime = formatDate(new Date());
    var data = $(".modal-body #email").val();
    var status = true;
    if (data != "") {
        $("#error1").css("display", "none");
        status = validateMultipleEmailsCommaSeparated(data);
    }
    else {
        $("#error").css("display", "none");
        $("#error1").css("display", "block");
        return false;
    }
    //var examId = $("#share").attr('examId');
    var examId = $('#feedbackexamId').val();
    if (status) {
        $.ajax({
            type: 'POST',
            data: { examId: examId, emaild: data, dateTime: dateTime },
            url: '/ExamHistory/ShareResult',
            success: function (result) {
                if (result) {
                    $('#Feedback').popover('hide');
                    $("#myForm").html('');
                    $("#shareResultconfirmDialog").modal();
                }
                else {
                    $("#shareResultErrorDialog").modal();
                }
            }
        });

        $("#error").css("display", "none");


    } else {

        $("#error").css("display", "block");
        return false;
    }
    //return false;
});

function formatDate(date) {
    var monthNames = [
        "1", "2", "3",
        "4", "5", "6", "7",
        "8", "9", "10",
        "11", "12"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var month = monthNames[monthIndex]
    return month + '-' + day + '-' + year;
}




