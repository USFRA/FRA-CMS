$(document).ready(function () {
  if (window.CMS) {
  }
  else {
    window.CMS = CMS();
    window.CMS.init();
  }

  var cmsRoot = '/';

  function CMS() {
    var _dialog;
    function init() {
      $('select').not("#contentWrapper select").kendoDropDownList();
      //            var currentIconUrl;
      //            var currentIconTitle;

      //            $('#cmsHeaderPanel').on('hover', '.cms-status-green', function () {
      //                //hover in
      //                currentIconUrl = $(this).attr('src');
      //                currentIconTitle = $(this).attr('title');

      //                $(this).attr('src', '/assetsCms/images/statusLive.png');
      //                $(this).attr('title', 'View live version');
      //                $(this).attr('alt', 'View live version');

      //            }, function () {
      //                //hover out
      //                if (currentIconUrl) {
      //                    $(this).attr('src', currentIconUrl);
      //                }
      //            });

      $('#cmsHeaderPanel').on('click', '.cms-status-green', function () {
        var h = window.location.href;
        var start = h.indexOf('/', 7);
        var publicUrl = cmsRoot + h.substring(start + 1);

        window.open(publicUrl, '_blank');
      });
    }

    function setCookie(c_name, value, exdays) {
      var exdate = new Date();
      exdate.setDate(exdate.getDate() + exdays);
      var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
      document.cookie = c_name + "=" + c_value;
    }

    function getCookie(c_name) {
      var i, x, y, ARRcookies = document.cookie.split(";");
      for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
          return unescape(y);
        }
      }
    }

    function showPopup(options) {
      var _options = {
        width: 700,
        height: 600,
        iframe: false,
        modal: true,
        resizable: false,
        animation: {
          open: {
            effects: ''
          },
          close: {
            effects: ''
          }
        },
        close: function () {

        }
      };
      $.extend(_options, options);
      if (_dialog) {
        _dialog.destroy();
        $('#contentWrapper').after('<div id="popupWin" style="display: none;"></div>');
      }
      if (_options.iframe) {
        $('#popupWin').html('');
      }
      else {
        $('#popupWin').html(_options.content);
        _options.content = null;
      }

      _dialog = $('#popupWin').kendoWindow(_options).data("kendoWindow");
      _dialog.center().open();
      $(document).off('click.overlay').on('click.overlay', '.k-overlay', function () {
        _dialog.close();
      });

      //console.log('Popup window open.');
      return $('#popupWin');
    }
    function change_parent_url(url) {
      document.location = url;
    }

    return {
      init: init,
      setCookie: setCookie,
      getCookie: getCookie,
      showPopup: showPopup,
      changeParentUrl: change_parent_url,
    }
  }
});