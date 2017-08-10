$(document).ready(function () {

  $('a').click(function (event) {
    event.preventDefault();

    if (window.parent) {
      window.parent.parent.CMS.changeParentUrl($(this).attr('href'));
    }

    return false;
  });

  $(document).ready(function () {
    $('#historyGrid').DataTable({
      lengthChange: false,
      searching: false,
      ajax: {
        url: '/Containers/pagecms/GetHistories/' + pageId,
        dataSrc: 'histories'
      },
      columns: [{
        data: 'Title'
      }, {
        data: 'Published',
        render: function (data, type, row) {
          var status = '';
          if (row.IsPublished) {
            status = ' [LIVE]';
          }
          return moment(data).format('M/D/Y h:mm A') + status;
        }
      }, {
        data: 'PublishedBy'
      }, {
        data: 'VerId',
        render: function (data) {
          return '<a href="/Containers/pagecms/version/' + data + '" target="_blank">View</a>';
        }
      }]
    });
  });
});