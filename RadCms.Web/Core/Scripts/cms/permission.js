function pageBtn() {
    if ($("#pageId").val() == "") {
        $("#showPagePermissions").attr("disabled", "disabled");
    }
    else {
        $("#showPagePermissions").removeAttr("disabled");
    }
}

$(document).ready(function () {

    $("#showUserPermissions").click(function () {
        var adName = $("#userId").val();
        //alert(encodeURIComponent(escape(adName)));
        if (adName.length > 6) {
            window.location.href = "/Permission/permissioncms/UserPermissions?u=" + escape(adName);
        }
        else {
            alert("Choose a User first!");
        }
    });

    $("#userId").kendoDropDownList({
        change: function (e) {
            $("#userName").val(this.value());
        }
    });
    $('#sectionId').kendoDropDownList({
        change: function () {
            var sectionId = this.value();
            if (sectionId != "") {
                $("#showSectionPermissions").removeAttr("disabled");
                $("#showPagePermissions").attr("disabled", "disabled");
                $.ajax({
                  url: "/Permission/permissioncms/GetPages/",
                    type: "post",
                    async: true,
                    data: JSON.stringify({ sectionId: sectionId }),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        $("#page").hide();
                        var sel = [];
                        sel.push('<label for="pageId">Page:</label><select style="width: 30%;" id="pageId" title="All Pages" name="pageId" onchange="pageBtn()">');
                        sel.push('<option value="">All Sub Pages</option>')
                        for (var s in data.Pages) {
                            var item = data.Pages[s];
                            sel.push('<option ');
                            sel.push(' value="');
                            sel.push(item.Value);
                            sel.push('">');
                            sel.push(item.Text);
                            sel.push('</option>');
                        }
                        sel.push('</select>');
                        $("#page div").empty().append(sel.join(''));
                        $('#pageId').kendoDropDownList();
                        $("#page").show();
                        if (pageId != "") {
                            //$("#showPagePermissions").removeAttr("disabled");
                        }
                    }
                });
            }
            else {
                $("#showSectionPermissions").attr("disabled", "disabled");
                $("#page").hide();
            }
        }
    });

    $('#accessMode').kendoDropDownList();

    if (sectionId) {
        $("#sectionId").val(sectionId);
        $("#sectionId").change();
    }

    if (adName) {
        $("#userId").val(adName);
        $("#userId").change();
    }

    $("#form").submit(function () {
        if (!$("#userName").val() || $("#userName").val() == 'addot\\') {
            alert("User Name is required.")
            return false;
        }
        else {
            return true;
        }
    });

    $("#showSectionPermissions").click(function () {
      window.location.href = '/Permission/permissioncms/NaviPermission/' + $('#sectionId').val();
    })

    $("#showPagePermissions").click(function () {
      window.location.href = '/Permission/permissioncms/PagePermission/' + $('#pageId').val()
    })
})
