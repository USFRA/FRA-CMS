$(document).ready(function () {
  var _ds_report, _msg;

  var oTable = $('#report').DataTable({
    searching: false,
    lengthChange: false,
    ajax: {
      url: '/cmsapp/AuditReport/',
      type: 'post',
      dataSrc: 'report'
    },
    columns: [{
      data: 'user'
    }, {
      width: 120,
      data: 'pageCreations'
    }, {
      width: 120,
      data: 'pagePublications'
    }, {
      width: 120,
      data: 'libCreations'
    }, {
      width: 120,
      data: 'libPublications'
    }]
  });

  _ds_report = new kendo.data.DataSource({
    transport: {
      read: function (options) {
        options.success(_msg);
      }
    },
    pageSize: 15,
    schema: {
      data: 'report',
      total: 'total'
    },
    change: function () {
      var data = this.data();
      oTable.clear().draw();
      oTable.rows.add(data).draw();
    }
  });

  var startInput = $('#start');
  var endInput = $("#end");
  generate(startInput.val(), endInput.val());

  startInput.kendoDatePicker();
  endInput.kendoDatePicker();

  $("#generateReport").click(function () {
      generate(startInput.val(), endInput.val());
  });

  function showUser(username, start, end) {
    window.location.href = '/cmsapp/singleuser?username=' + username + '&start=' + start + '&end=' + end;
  }

  function generate(start, end) {
    $.ajax({
      url: "/cmsapp/AuditReport/",
      type: "post",
      async: true,
      data: JSON.stringify({ start: start, end: end }),
      contentType: 'application/json; charset=utf-8',
      success: function (msg) {
        _msg = msg;
        _ds_report.read();
      }
    });
  }
});
