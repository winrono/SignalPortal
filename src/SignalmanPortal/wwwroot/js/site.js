$(document).ready(function () {
    $('#main-navbar [title]').attr('data-toggle', 'tooltip');
    $('#main-navbar [title]').attr('data-placement', 'bottom');
    $('[data-toggle="tooltip"]').tooltip();

    $('#adminList').on('shown.bs.dropdown', function () {
        $(this).tooltip('hide');
        $(this).tooltip('disable');
    });

    $('#adminList').on('hidden.bs.dropdown', function () {
        $(this).tooltip('enable');
    });

    $('body').on('click', '[deleteNoveltyLink], [deleteBookLink], [deleteBookCategoryLink]', function (e) {
        var that = this;

        e.preventDefault();
        $.ajax({

            url: $(this).attr("href"),
            success: function (isDeleted) {
                if (isDeleted) {
                    var parentRow = $(that.closest('.row, .tr-row'));
                    $(that.closest('.row, .tr-row')).fadeOut();

                    var input = parentRow.find('.removed-input');
                    if (input) {
                        input.val(true);
                    }
                }
                else {
                    alert('Информации нет в базе данных. Возможно, кто-то уже удалил её');
                }
            }

        });
    });
});