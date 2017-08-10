$(document).ready(function () {
  if (!window.CMS) {
    window.CMS = {};
  }
  window.CMS.PAGE = CMSPage();
  CMS.PAGE.init();

  function CMSPage() {
    function init() {
    }

    function addNewPage() {
      $.get('/Template/layoutcms/Items', function (view) {
        CMS.showPopup({
          content: view,
          iframe: false,
          width: 500,
          height: 500
        });

        $('#selectLayoutBtn').click(function () {
          var layoutId = $('.layoutOption.selected', '#layoutList').attr('data-id');
          window.location = '/Containers/pagecms/create/' + sectionId + '?layoutId=' + layoutId;
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

    function unpublish() {
      var confirmWindow = CMS.showPopup({
        title: 'Confirm',
        iframe: false,
        width: 150,
        height: 100,
        content: '<p>Are you sure?</p><button class="unpublish-confirm k-button">Yes</button><button class="k-button unpublish-cancel">No</button>'
      });
      console.log(confirmWindow.find(".unpublish-confirm, .unpublish-cancel"));
      confirmWindow
          .find(".unpublish-confirm, .unpublish-cancel")
              .click(function () {
                if ($(this).hasClass("unpublish-confirm")) {
                  $('#act').val('unpublish');
                  $('#actForm').submit();
                }

                confirmWindow.data("kendoWindow").close();
              })
              .end();
    }
    function unlock() {
      var answer = confirm("This page is in editing mode. Are you sure you want to unlock it?");
      if (answer) {
        $('#act').val('unlock');
        $('#actForm').submit();
      }
      return false;
    }

    function edit() {
      $('#act').val('edit');
      $('#actForm').submit();
      return false;
    }

    function publish() {
      $('#act').val('publish');
      $('#actForm').submit();
    }

    function movePages() {
      CMS.showPopup({ content: "/ContentTree/Sectioncms/MovePages/" + sectionId, iframe: true, title: false });
    }

    function showHistory() {
      CMS.showPopup({ content: '/Containers/Pagecms/History/' + currentPageId, iframe: true, title: false });
    }

    function analyzeLinks() {
      CMS.showPopup({ content: '/Containers/Pagecms/AnalyzeLinks/' + currentPageId, iframe: true, title: false });
    }

    function restore() {
      $('#act').val('restore');
      $('#actForm').submit();
    }

    function cancelHistory() {
      window.location.href = "/Containers/Page/GoTo/" + currentPageId;
    }
   
    function viewPermission() {
    }

    function managePermissions() {
      CMS.showPopup({ content: '/Permission/PermissionCms/Manage?id=' + currentPageId, title: false, iframe: true });
    }

    function deletePage() {
      //Cannot delete default page in a section!
      $.post("/Containers/PageCms/CanDeletePage", { id: currentPageId }, function (data) {
        if (data.Message == "no") {
          alert('Cannot delete last page in this section!');
        }
        else {
          var answer = confirm("Are you sure you want to delete this page?")
          if (answer) {
            $('#act').val('delete');
            $('#actForm').submit();
          }
        }
      });
    }

    function editWebParts() {
      var t = $("#editWebPartsBtn span");

      if (t.hasClass('cms-icon-webpart')) {

        $('.webpart').each(function (index) {
          var overlay = $('<div class="webpartCover" style="position: absolute;z-index:500"><img src="/Core/assetsCms/images/editWebPartButton.png" alt="Edit Webpart" /></div>').hide().appendTo('body');
          var wp = $(this);
          overlay.width(wp.outerWidth());
          overlay.height(wp.outerHeight());
          var position = wp.offset();
          overlay.css(position);
          overlay.show();

          var webpartId = $(this).attr('webpartId');
          overlay.click(function (event) {
            event.preventDefault();

            var id = webpartId;
            var managePath = "/" + id;

            CMS.showPopup({
              content: managePath,
              title: false,
              iframe: true,
              width: 1000,
              height: 600,
              close: function () {
                window.location.reload();
              }
            });
            return false;
          });
        });

        $(t).removeClass('cms-icon-webpart').addClass('cms-icon-noWebpart');
        $(t).attr("src", "/Core/assetsCms/images/hideWebParts.png");
      }
      else {
        $('.webpartCover').remove();
        $(t).removeClass('cms-icon-noWebpart').addClass('cms-icon-webpart');
      }
    }

    return {
      init: init,
      addNewPage: addNewPage,
      unpublish: unpublish,
      unlock: unlock,
      edit: edit,
      editWebParts: editWebParts,
      publish: publish,
      movePages: movePages,
      showHistory: showHistory,
      analyzeLinks: analyzeLinks,
      restore: restore,
      cancelHistory: cancelHistory,
      //toggleWebparts: toggleWebparts,
      viewPermission: viewPermission,
      managePermissions: managePermissions,
      deletePage: deletePage
    }
  }
});

