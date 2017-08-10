$(document).ready(function () {
    $.get('/footer/FooterTypes', function (types) {
        if (types[$('#header').val()] == 0) {
            //vertical
            $('.vertical').show();
            $('.horizontal').hide();
        }
        else {
            $('.vertical').hide();
            $('.horizontal').show();
        }
        $('#header').on('change', function () {
            if (types[$(this).val()] == 0) {
                //vertical
                $('.vertical').show();
                $('.horizontal').hide();
            }
            else {
                $('.vertical').hide();
                $('.horizontal').show();
            }
        });
    });
    var editor = $('.cms-editable').kendoEditor({
        tools: [
            "insertImage"
        ],
        imageBrowser: {
            path: "/0",
            messages: {
                dropFilesHere: 'Drop files here'
            },
            transport: {
                read: {
                    url: function () {
                        $(".k-filebrowser .k-breadcrumbs.k-textbox").css('display', 'none');
                        $(".k-filebrowser .k-addfolder").parent().css('display', 'none');
                        return '/ImageLibrary/ImageBrowser/Read';
                    }
                },
                destroy: {
                    url: '/ImageLibrary/ImageBrowser/Destroy',
                    type: "POST"
                },
                create: {
                    url: '/ImageLibrary/ImageBrowser/Create',
                    type: "POST"
                },
                thumbnailUrl: '/ImageLibrary/ImageBrowser/Thumbnail',
                uploadUrl: '/ImageLibrary/ImageBrowser/Upload',
                imageUrl: '/ImageLibrary/ImageBrowser/Image?path={0}'
            }
        }
    }).data('kendoEditor');

    $('form').submit(function (e) {
        if ($(this).find('#header').val() == 'CONNECT WITH US') {
            $('#Title').val(editor.value());
        }
    });
});