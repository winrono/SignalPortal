$(document).ready(function () {
    $('#main-navbar [title]').attr('data-toggle', 'tooltip');
    $('#main-navbar [title]').attr('data-placement', 'bottom');
    $('[data-toggle="tooltip"]').tooltip();
});