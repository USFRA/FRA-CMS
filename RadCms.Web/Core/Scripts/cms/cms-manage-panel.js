function auditReport() {
    window.location.href = "/cmsapp/auditreport/";
}

$(document).ready(function () {
    var cmsHomeNotification = $('#cmsHomeNotification').kendoNotification({
        appendTo: "#cmsHomeNotifications"
    }).data("kendoNotification");
    $('#cmsHomeMenu').on('click', 'li', function () {
        if ($(this).hasClass('cms-state-active')) {
        }
        else {
            $('.cms-state-active', $(this).parent()).removeClass('cms-state-active');
            $(this).addClass('cms-state-active');

            var index = $(this).index() + 1;
            $('#cmsHomeContent>div.cms-state-active').removeClass('cms-state-active');
            $('#cmsHomeContent>div:nth-child(' + index + ')').addClass('cms-state-active');

            loadDataForTab(index);
        }
    });
    $('#cmsHomeContent .tabstrip').kendoTabStrip({
        animation: false
    });
    $('#cmsHomeMenu li:nth-child(' + tab + ')').click();

    function loadDataForTab(index) {
        switch (index) {
            case 1:
                showSelectLayout();
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }
    function showSelectLayout() {
        $.get('/Template/layoutcms/Items?sectionType=' + type, function (view) {
            $('#layoutView').html(view);

            $('#selectLayoutBtn').click(function () {
                var layoutId = $('.layoutOption.selected', '#layoutList').attr('data-id');
                if (layoutId) {
                    window.parent.location = '/Containers/pagecms/create/' + window.parent.sectionId + '?layoutId=' + layoutId;
                }
                else {
                    alert("Please select a layout.");
                }
            });
            var container = $('#layoutList');
            $('.layoutOption', container).click(function () {
                if ($(this).hasClass('selected')) {
                    return;
                }
                $('.layoutOption.selected', container).removeClass('selected');
                $(this).toggleClass('selected');
            });
        });
    }
});