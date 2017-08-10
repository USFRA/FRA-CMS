$(document).ready(function () {
    $(".htmlEditor").kendoEditor({
        encoded: false,
        imageBrowser: {
            path: "/" + 0,
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
                    url: "/ImageLibrary/ImageBrowser/Destroy",
                    type: "POST"
                },
                create: {
                    url: "/ImageLibrary/ImageBrowser/Create",
                    type: "POST"
                },
                thumbnailUrl: "/ImageLibrary/ImageBrowser/Thumbnail",
                uploadUrl: "/ImageLibrary/ImageBrowser/Upload",
                imageUrl: "/ImageLibrary/ImageBrowser/Image?path={0}"
            }
        }
    });
});