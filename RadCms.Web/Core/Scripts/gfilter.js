$(document).ready(function () {
    var totalCount = 0;
    var pageNum = 1;
    var pageSize = 5;
    var hasPreviousPage = false;
    var hasNextPage = false;
    var totalPage = 0;


    $(".result").click(function (event) {
        event.preventDefault();

        var link = $("a", this).attr('href');
        if (link) {
            linkLowerCase = link.toLowerCase();

            if (linkLowerCase.indexOf("http://www.fra.dot.gov/elib/document/") != -1) {
                var documentId = linkLowerCase.substring(37);
                //alert(documentId.toString());
                window.location.href = "/search/redirectelib/"+documentId;                
            }
            else {
                //alert(link.toString());
                window.location.href = link;
            }
        }
        return false;
    });
});