function auditReport() {
  window.location.href = baseUrl + "cmsapp/auditreport/";
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

  $.getJSON("/Containers/cmscms/getdrafts", function (msg) {
    if (msg.drafts.length > 0) {
      $('#myDraftsGrid').show();
      $('#myDraftsMessage').hide();
      loadMyDrafts(msg.drafts);
    }
    else {
      $('#myDraftsGrid').hide();
      $('#myDraftsMessage').show();
    }

    if (msg.otherDrafts.length > 0) {
      $('#otherDraftsGrid').show();
      $('#otherDraftsMessage').hide();
      loadOtherDrafts(msg.otherDrafts);
    }
    else {
      $('#otherDraftsGrid').hide();
      $('#otherDraftsMessage').show();
    }
  });

  function loadMyDrafts(data) {
    $('#myDraftsGrid').DataTable({
      searching: false,
      lengthChange: false,
      data: data,
      columns: [{
        data: 'Status',
        render: function (data) {
          if (data == 3) {
            return '<span title="Editing" style="display: block; margin: 0 auto;" class="cms-status cms-status-red"></span>';
          }
          else {
            return '<span title="Not published draft" style="display: block; margin: 0 auto;" class="cms-status cms-status-yellow"></span>';
          }
          return data;
        }
      }, {
        data: 'Title',
        render: function (data, type, row) {
          return '<a href="' + baseUrl + row.Url + '" target="_blank">' + data + '</a>';
        }
      }, {
        data: 'Modified',
        render: function (data) {
          return moment(data).format('M/D/Y h:mm A') + status;
        }
      }, {
        data: 'ModifiedBy'
      }]
    });
  }

  function loadOtherDrafts(data) {
    $('#otherDraftsGrid').DataTable({
      searching: false,
      lengthChange: false,
      data: data,
      columns: [{
        data: 'Status',
        render: function (data) {
          if (data == 3) {
            return '<span title="Editing" style="display: block; margin: 0 auto;" class="cms-status cms-status-red"></span>';
          }
          else {
            return '<span title="Not published draft" style="display: block; margin: 0 auto;" class="cms-status cms-status-yellow"></span>';
          }
          return data;
        }
      }, {
        data: 'Title',
        render: function (data, type, row) {
          return '<a href="' + baseUrl + row.Url + '" target="_blank">' + data + '</a>';
        }
      }, {
        data: 'Modified',
        render: function (data) {
          return moment(data).format('M/D/Y h:mm A') + status;
        }
      }, {
        data: 'ModifiedBy'
      }]
    });
  }

  function loadDataForTab(index) {
    switch (index) {
      case 1:
        break;
      case 2:
        break;
      case 4:
        break;
      case 5:
        break;
      case 6:
        break;
      case 3:
      default:
        refreshActiveFrame();
        break;
    }
  }
  function refreshActiveFrame() {
    var $iframe = $('#cmsHomeContent .cms-state-active iframe');
    $iframe.attr('src', $iframe.attr('src'));
  }
});