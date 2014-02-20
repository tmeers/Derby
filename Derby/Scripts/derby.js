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
});