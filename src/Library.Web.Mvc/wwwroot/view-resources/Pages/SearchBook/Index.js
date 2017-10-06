(function () {
    $(function () {
        var _bookService = abp.services.app.book;
        var _$modal = $('#BookDetail');

        $('.show-bookdetail').click(function(e) {
            var bookId = $(this).attr("data-book-id");
            e.preventDefault();
            $.ajax({
                url: abp.appPath + 'SearchBooks/Bookdetail?bookId=' + bookId,
                type: 'POST',
                contentType: 'application/html',
                success: function (content) {
                    $('#BookDetail div.modal-content').html(content);
                },
                error: function(e){ }
            })
        });

        $('.show-bookreserve').click(function (e) {
            var bookId = $(this).attr("data-book-id");
            e.preventDefault();
            $.ajax({
                url: abp.appPath + 'SearchBooks/BookReserve?bookId=' + bookId,
                type: 'POST',
                contentType: 'application/html',
                success: function (content) {
                    $('#BookReserve div.modal-content').html(content);
                },
                error: function(e){ }
            })
        })
    });
})();
