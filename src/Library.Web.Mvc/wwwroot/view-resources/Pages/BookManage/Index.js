(function() {
    $(function() {
        var _bookService = abp.services.app.book;
        var _$modal = $('#BookModal');

        $('#RefreshButton').click(function () {
            refreshUserList();
        });

        $('.delete-book').click(function () {
            var bookId = $(this).attr("data-book-id");
            var bookTitle = $(this).attr("data-book-title");

            deleteBook(bookId, bookTitle);
        });

        $('.edit-book').click(function (e) {
            // abp.ui.setBusy(_$modal);

            var bookId = $(this).attr("data-book-id");

            e.preventDefault();
            $.ajax({
                url: abp.appPath + 'BookManage/EditBook?bookId=' + bookId,
                type: 'POST',
                contentType: 'application/html',
                success: function (content) {
                    $('#BookModal div.modal-content').html(content);
                },
                error: function (e) { }
            });
        });

        _$modal.on('shown.bs.modal', function () {
            _$modal.find('input:not([type=hidden]):first').focus();
        });

        function refreshBookList() {
            location.reload(true); //reload page to see new user!
        }

        function deleteBook(bookId, bookName) {
            abp.message.confirm(
                "Delete book '" + bookName + "'?",
                function (isConfirmed) {
                    if (isConfirmed) {
                        _bookService.delete({
                            id: bookId
                        }).done(function () {
                            refreshBookList();
                        });
                    }
                }
            );
        }
    });
})();
