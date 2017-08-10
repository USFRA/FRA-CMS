var args = parent.tinymce.activeEditor.windowManager.getParams();
var winJ = args['jquery'];
var win = args['window'];
var field = '#' + args['input'];
var input = winJ(field);


$(document).ready(function () {
  $('#fileUpload').on('change', function () {
    if ($(this).val()) {
      var form = $(this.form);
      $.ajax(form.prop('action'), {
        data: form.find('input[type=hidden]').serializeArray(),
        dataType: 'json',
        files: form.find(':file'),
        iframe: true,
        processData: false
      }).always(function () {
        console.log('process finished');
      }).done(function (data) {
        window.location.reload();
      });
    }
  });

  $('.file-list-wrapper li').click(function () {
    var count = $('.file-list-wrapper li.selected').length;

    if (count == 1 && !$(this).hasClass('selected') ) {
      $('.file-list-wrapper li.selected').removeClass('selected');
    }

    $(this).toggleClass('selected');
  });

  $('#insertBtn').click(function () {
    if ($('.file-list-wrapper li.selected').length == 1) {
      var thumbUrl = $('.file-list-wrapper li.selected img').attr('src');
      var param = thumbUrl.substring(thumbUrl.indexOf('?'));
      input.val('/ImageBrowser/Image' + param);
      win.tinyMCE.activeEditor.windowManager.close(window);
    }
  });

  $('#cancelBtn').click(function () {
    win.tinyMCE.activeEditor.windowManager.close(window);
  });
});