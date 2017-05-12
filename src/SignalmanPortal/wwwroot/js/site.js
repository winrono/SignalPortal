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

    $('[deleteNoveltyLink]').click(function (e) {

        var that = this;

        e.preventDefault();
        $.ajax({

            url: $(this).attr("href"),
            success: function (isDeleted) {
                if (isDeleted) {
                    $(that.closest('.row')).fadeOut();
                }
                else {
                    alert('Новости нет в базе данных. Возможно, кто-то уже удалил её');
                }
            }

        });
    })

});