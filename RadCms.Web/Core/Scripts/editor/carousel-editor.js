$(document).ready(function () {
  $('#SlideId').kendoNumericTextBox({
    format: 'n0'
  });

  $('.cms-editable').each(function (i, item) {
    $(item).wrapInner('<div class="cms-editable-container" id="cEditor"></div>');
  });

  tinymce.init({
    selector: '.cms-editable-container',
    inline: true,
    menubar: false,
    plugins: [
      'advlist autolink lists link image preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media contextmenu paste'
    ],
    toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
    file_browser_callback_types: 'file image media',
    file_browser_callback: myFileBrowser
  });

  $('#SaveChange').click(saveChange);
  $('#CancelChange').click(cancelChange);
});

function saveChange(e) {
  e.preventDefault();
  var content = {};
  for (var i = 0; i < tinyMCE.editors.length; i++) {
    var editor = tinyMCE.editors[i];
    var regionContent = editor.getContent();
    var regionId = editor.id;
    content[regionId] = regionContent;
  }

  $('#SlideContent').val(content.cEditor);
  $('#submitChange').click();
}

function cancelChange(e) {
  e.preventDefault();
  $('#actForm').submit();
}

function myFileBrowser(field_name, url, type, win) {
  var cmsURL = window.location.origin + '/imagelibrary/imagebrowsercms/index?path=' + sectionId;

  if (cmsURL.indexOf("?") < 0) {
    cmsURL = cmsURL + "?type=" + type;
  }
  else {
    cmsURL = cmsURL + "&type=" + type;
  }

  tinyMCE.activeEditor.windowManager.open({
    file: cmsURL,
    title: 'File Browser',
    width: 800,
    height: 500,
    resizable: "yes",
    inline: "yes",
    close_previous: "no"
  }, {
    window: win,
    input: field_name,
    jquery: $
  });
  return false;
}