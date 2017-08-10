$(document).ready(function () {
  $('#tree').jstree({
    core: {
      multiple: false,
      animation: 1,
      check_callback: true,
      themes: { 'stripes': false },
      data: {
        url: function () {
          return baseUrl + "ContentTree/TreeCms/Node?_no_cache=" + new Date();
        }
      },
      check_callback: function (operation, node, node_parent, node_position) {
        if (operation == 'move_node') {
          if (node_parent.id.indexOf('P') == 0) {
            return false;
          }

          if (node_parent == null && node_position == 0) {
            return false;
          }
        }
        //else if (operation == 'delete_node') {
        //  return confirm("Are you sure to delete the entire section?");
        //}

        return true;
      }
    },
    "plugins": [
      "contextmenu", "dnd", "search",
      "state", "types", "wholerow", "unique"
    ],
    contextmenu: {
      items: function (node) {
        if (node.id.indexOf('N') == 0) {
          return {
            rename: {
              label: 'Rename',
              action: function () {
                $('#tree').jstree(true).edit(node);
              }
            }
            //,
            //deleteItem: {
            //  label: 'Delete',
            //  action: function () {
            //    $('#tree').jstree(true).delete_node(node);
            //  }
            //}
          }
        }
        else {
          return {
            openPage: {
              label: 'Open this page',
              action: function (obj) {
                window.open(node.original.linkUrl, '_blank');
              }
            }
          };
        }
      }
    }
  }).on('move_node.jstree', function (e, data) {
    $.ajax({
      url: baseUrl + "ContentTree/TreeCms/MoveJsTree",
      type: "POST",
      async: false,
      data: JSON.stringify({
        nodeId: data.node.id,
        parentId: data.parent,
        position: data.position
      }),
      contentType: 'application/json; charset=utf-8',
      success: function (data) {
        location.reload();
      }
    });
  }).on('rename_node.jstree', function (e, data) {
    updateSectionName(data.node.id.substring(1), data.text);
  })
    //.on('delete_node.jstree', function (e, data) {
    //removeSection(data.node.id.substring(1));
    //})
  ;


  function updateSectionName(id, newSectionName) {
    $.ajax({
      url: baseUrl + "ContentTree/Sectioncms/RenameSection",
      type: "POST",
      async: false,
      data: JSON.stringify({
        id: id,
        name: newSectionName
      }),
      contentType: 'application/json; charset=utf-8',
      success: function (data) {
        location.reload();
      }
    });
  }

  //function removeSection(id) {
  //  $.ajax({
  //    url: baseUrl + "ContentTree/TreeCms/RemoveSection",
  //    type: "POST",
  //    async: false,
  //    data: JSON.stringify({
  //      id: id
  //    }),
  //    contentType: 'application/json; charset=utf-8',
  //    success: function (data) {
  //      location.reload();
  //    }
  //  });
  //}
});