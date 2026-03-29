$(document).ready(function () {
    var subspecilty = NotebookViewModel.subSpecialtyId;
    var chapterName = NotebookViewModel.chapterName;
    $("#Model11").hide();
    $.ajax({
        type: 'GET',
        data: {},
        url: '/NoteBook/GetSectionList',
        success: function (result) {
            var section = '<option value="0">Select Section</option>';
            for (var i = 0; i < result.length; i++) {
                if (subspecilty > 0) {
                    if (subspecilty == result[i].SubSpecialtyId) {
                        section += '<option value="' + result[i].SubSpecialtyId + '" selected>Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                    }
                    else {
                        section += '<option value="' + result[i].SubSpecialtyId + '">Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                    }
                } else {
                    section += '<option value="' + result[i].SubSpecialtyId + '">Section ' + result[i].BCSCSectionNumber + ': ' + result[i].BCSCSectionTitle + '</option>';
                }

            }
            $('#SectionList').html(section);
            LoadChapter(subspecilty, chapterName);
        }
    });

    function LoadChapter(sectionid, chapterName) {
        var specialtyId = 0;
        if (sectionid > 0) {
            specialtyId = sectionid;
        } else {
            specialtyId = $('#SectionList option:selected').val();
        }
        specialtyId > 0 ? $('#ChapterList').removeAttr('disabled') : $('#ChapterList').attr('disabled', 'disabled');

        $.ajax({
            type: 'POST',
            data: { subSpecialtyId: specialtyId },
            url: '/NoteBook/GetChapterList',
            success: function (result) {
                var chapter = '<option value="0">Select Chapter</option>';
                for (var i = 0; i < result.length; i++) {
                    if (chapterName != '') {
                        if (chapterName == result[i].ChapterName) {
                            chapter += '<option value="' + result[i].ChapterName + '"selected>' + result[i].Chapter + ': '  + result[i].ChapterName + '</option>';
                        }
                        else {
                            chapter += '<option value="' + result[i].ChapterName + '">' + result[i].Chapter + ': '  + result[i].ChapterName + '</option>';
                        }
                    } else {
                        chapter += '<option value="' + result[i].ChapterName + '">' + result[i].Chapter + ': '  + result[i].ChapterName + '</option>';
                    }
                }
                $('#ChapterList').html(chapter);
                LoadTopics(chapterName, chapterName);
            }
        });
    }

    function LoadTopics(chapterName, TopicName) {
        var chapterName = '';
        //if (chapterName.trim() === '') {
        //    chapterName = chapterName;
        //    alert("if: "+chapterName);
        //} else {
            chapterName = $('#ChapterList option:selected').val();
            //alert("else: "+chapterName);
        //}
        //chapterName > 0 ? $('#TopicList').removeAttr('disabled') : $('#TopicList').attr('disabled', 'disabled');
        //chapterName.length > 0 ? $('#TopicList').prop('disabled', false) : $('#TopicList').prop('disabled', true);
    
        $.ajax({
            type: 'POST',
            data: { chapterName: chapterName },
            url: '/NoteBook/GetTopicList',
            success: function (result) {
                var topic = '<option value="0">Select Topic</option>';
                for (var i = 0; i < result.length; i++) {
                    if (TopicName != '') {
                        if (TopicName == result[i].TopicName) {
                            topic += '<option value="' + result[i].TopicId + '"selected>' + result[i].TopicNo + ': ' + result[i].TopicName + '</option>';
                        }
                        else {
                            topic += '<option value="' + result[i].TopicId + '">' + result[i].TopicNo + ': ' + result[i].TopicName + '</option>';
                        }
                    } else {
                        topic += '<option value="' + result[i].TopicId + '">' + result[i].TopicNo + ': ' + result[i].TopicName + '</option>';
                    }
                }
                $('#TopicList').html(topic);
            }
        });
    
    }

    $('#SectionList').on('change', function () {
        LoadChapter(sectionid = 0, chapterName = null);
    });

    $('#ChapterList').on('change', function () {
        LoadTopics(chapterName = '', chapterName = null);
    });

    
    $('#TopicList').on('change', function () {
        var Topicid = $('#TopicList option:selected').val();
        if (Topicid <= 0) {
            //alert("if, Topicid: " + Topicid);
            Topicid = sectionid;
            $("#Model11").hide();
        } else {
            //alert("else 2, Topicid: " + Topicid);
            $("#Model11").show();
        } 
    });

    $('textarea').on('input', function () {
        var choiceId = $(this).attr('id');
        var choiceText = $(this).val();

        $('#correct_option option[value="' + choiceId + '"]').text(choiceText);
    });

    $("a.btn-primary").click(function (event) {
        event.preventDefault();

        var questionDetails = {
            questionTitle: "",
            choices: [],
            correctChoice: "",
            discussion: "",
            references: "",
            topicId:""
        };

        var section = $("#SectionList option:selected").text();
        var chapter = $("#ChapterList option:selected").text();
        var topic = $("#TopicList option:selected").text();

        questionDetails.questionTitle = $("#question_title").val();
        questionDetails.choices = [$("#choice1").val(), $("#choice2").val(), $("#choice3").val(), $("#choice4").val()]
        questionDetails.correctChoice = $("#correct_option option:selected").text();
        questionDetails.discussion = $("#discussion").val();
        questionDetails.references = section + " > " + chapter + " > " + topic;
        questionDetails.topicId = $('#TopicList option:selected').val();        

        $.ajax({
            type: 'POST',
            data: { questionDetails: questionDetails },
            url: '/AllQuestions/AddQuestion_aftersubmit',
            success: function (result) {
                alert("Successfully Added Question");
                window.location.reload();
            },
            error: function () {
                alert("Error occurred while adding question");
            }
        });
    });

    $("#clearForm").click(function () {
        $("#questionForm")[0].reset();
    });

});



