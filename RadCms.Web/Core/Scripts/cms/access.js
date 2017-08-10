$(document).ready(function () {
    $("#setAccess").click(function () {
        $.ajax({
            url: "/Permission/permissioncms/ChangeAccess/",
            type: "post",
            async: true,
            data: JSON.stringify({ userId: userId, action: "setAccess", canLib: $("#canLib:checked").val(), isAdmin: $("#isAdmin:checked").val() }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                window.location.reload();
            }
        });
    });


    $("#clearAccess").click(function () {
        $.ajax({
            url: "/Permission/permissioncms/ChangeAccess/",
            type: "post",
            async: true,
            data: JSON.stringify({ userId: userId, action: "clearAccess" }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                window.location.reload();
            }
        });
    });

    $("#removeAccess").click(function () {
        $.ajax({
            url: "/Permission/permissioncms/ChangeAccess/",
            type: "post",
            async: true,
            data: JSON.stringify({ userId: userId, action: "removeAccess" }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //window.history.go(-1);
            }
        });
    });
})
