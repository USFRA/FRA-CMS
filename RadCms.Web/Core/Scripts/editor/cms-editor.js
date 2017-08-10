$(document).ready(function () {

  $('.cms-editable').each(function (i, item) {
    $(item).wrapInner('<div class="cms-editable-container" id="'+$(item).attr('data-region')+'"></div>');
  });

  tinyMCE.init({
    selector: '.cms-editable-container',
    inline: true,
    menubar: false,
    plugins: [
      'advlist autolink lists link image preview anchor',
      'searchreplace visualblocks code fullscreen',
      'insertdatetime media contextmenu paste'
    ],
    toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | templates',
    file_browser_callback_types: 'file image media',
    file_browser_callback: myFileBrowser,
    setup: function (editor) {
      editor.addButton('template', {
        text: 'HTML Templates',
        icon: false,
        menu: [{
          text: 'Menu item 1',
          onclick: function () {
            editor.insertContent('123');
          }
        }, {
          text: 'Menu item 2',
          onclick: function () {
            editor.insertContent('321');
          }
        }]
      });
    }
  });

  $('#contentWrapper form').submit(function (e) {
    var content = {};
    
    for (var i = 0; i < tinyMCE.editors.length; i++) {
      var editor = tinyMCE.editors[i];
      var regionContent = editor.getContent();
      var regionId = editor.id;
      console.log(regionId);
      content[regionId] = regionContent;
    }

    $('#Content').val(JSON.stringify(content));
  });

});

function saveChange() {
  $('#submitChange').click();
}

function cancelChange() {
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