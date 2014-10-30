$(document).ready(function () {
    //$('.date').datepicker({ dateFormat: "dd/mm/yy" });
    $("#dashboard-scouts").tablesorter({ debug: true });

    $('i.information').click(function () {
        console.log(this);

        var data = $(this).data('tip');
        if (data != null || data != "") {
            $("<div />", { 'class': 'popup', 'text': data }).insertAfter(this).fadeIn().delay(3000).fadeOut();
        }

    });

    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });

    // TODO Move back into page /membership/index
    $('#inviteTab').click(function (e) {
        e.preventDefault();
        $('#invitations').empty();
        $('#invitations').html('<p>Loading content...<i class="fa fa-cog fa-spin"></i></p>');
        $(this).tab('show');
        setTimeout(function () {
            $.ajax({
                url: "/invitation/sentinvitations",
                cache: false,
                type: "get",
                dataType: "html",
                success: function (result) {
                    $("#invitations").html(result);
                }
            });
        }, 2000);
    });

    //$('.modal-link').click(function (e) {
    //    console.log(this);
    //    //e.preventDefault();
    //    $(this).attr('data-target', '#modal-container');
    //    $(this).attr('data-toggle', 'modal');

    //    $('.modal')
    //     .prop('class', 'modal fade') // revert to default
    //     .addClass($(this).data('direction'));

        
    //    $('.modal').modal('show');
    //});

});