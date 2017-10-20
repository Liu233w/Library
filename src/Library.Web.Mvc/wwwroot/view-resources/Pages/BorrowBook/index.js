$(function() {

    $('#copyid').blur(function(e) {
        var copyid = $(this).val();
        console.log(copyid);

        e.preventDefault();
        $.ajax({
            url: abp.appPath + 'BorrowBook/book?copyid=' + copyid,
            type: 'POST',
            contentType: 'application/html',
            success: function(content) {
                $('#copy-info').html(content);
            },
            error: function() {
                $('#copy-info').html("<h3>Can't find</h3>");
            }
        });
    });

    // 防止按下回车触发提交事件
    $('#copyid').keydown(function(e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('#userid').focus();
        }
    });

    $('#userid').blur(function(e) {
        var readerid = $(this).val();
        console.log(readerid);

        e.preventDefault();
        $.ajax({
            url: abp.appPath + 'BorrowBook/reader?userNameOrEmail=' + readerid,
            type: 'POST',
            contentType: 'application/html',
            success: function(content) {
                $('#reader-info').html(content);
            },
            error: function() {
                $('#reader-info').html("<h3>Can't find</h3>");
            }
        });
    })

    $('#userid').keydown(function(e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $('#borrow').focus();
        }
    });
});