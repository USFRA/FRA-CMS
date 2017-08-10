if (typeof console === "undefined") {
    console = {};
}

if (typeof console.log === "undefined") {
    console.log = function () { };
}

if (typeof console.debug === "undefined") {
    console.debug = console.log;
}

if (typeof console.error === "undefined") {
    console.error = console.log;
}

$(document).ready(function () {
    $('#navToggle').click(function () {
        $('nav', $(this).parent()).slideToggle();
    });

    $("body").on('click', '.cms-overlay', function (e) {
        $(this).remove();
    });
    $('.embededVideo').click(function (e) {
        e.preventDefault();
        var layout = [];
        layout.push('<div class="cms-overlay" id="videoPopup">');
        layout.push('<div class="cms-overlay-background"></div>');
        layout.push('<iframe width="');
        layout.push($(this).attr('data-video-width'));
        layout.push('" height="');
        layout.push($(this).attr('data-video-height'));
        layout.push('" src="');
        layout.push(this.href);
        layout.push('" frameborder="0"></iframe></div>');
        $(layout.join('')).appendTo($(document.body));

        var topMargin = ($(window).height() - $('.cms-overlay iframe').height()) / 2;
        $('.cms-overlay iframe').css('margin-top', topMargin + 'px');
    });

    window.setTimeout(initHeaderNav, 500);

    function initHeaderNav() {
        $('#headerNavWrapper .nav-group').on('click', function (e) {
            e.preventDefault();
            if ($(this).hasClass('hover')) {
                $(this).parent().removeClass('hover');
                $(this).parent().find('.hover').removeClass('hover');
                return;
            }
            $(this).parent().parent().find('.hover').removeClass('hover');

            $(this).addClass('hover');
            $(this).parent().addClass('hover');
        });
    }
});