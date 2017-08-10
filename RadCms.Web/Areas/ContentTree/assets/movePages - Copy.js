$(document).ready(function () {

    var selectedItem;

    $.get(baseUrl + "ContentTree/TreeCms/Node?_no_cache=" + new Date(), function (treeData) {
        var treeview = $('#tree').kendoTreeView({
            dragAndDrop: true,
            dataSource: treeData,
            template: kendo.template($('#treeview-template').html()),
            //select: onSelect,
            drop: onDrop
        }).data('kendoTreeView');
        treeview.expandTo(expandTo);
        selectedItem = treeview.findByUid(treeview.dataSource.get(expandTo).uid);
        treeview.select(selectedItem);

        $('#tree').on('click', '.tree-button', function (e) {
            var button = $(this);
            if (button.hasClass('rename-section')) {
                e.preventDefault();
                updateSectionName(button.attr('data-id'));
            }
            else if (button.hasClass('create-section')) {
                e.preventDefault();
                moveToNewSection(button.attr('data-id'));
            }
            else if (button.hasClass('remove-section')) {
                e.preventDefault();
                removeSection(button.attr('data-id'));
            }
        });
    });
    $('#renameSectionBtn').click(function () {
        var node = selectedItem;
        if (node && node.hasChildren == true) {
            var id = node.id.substring(1);
            updateSectionName(id, newSectionName);
        }
    });
    $('#moveToSectionBtn').click(function () {
        var node = selectedItem;
        if (node && node.hasChildren == false) {
            moveToNewSection(node.id);
        }
    });
    function updateSectionName(id) {
        var newSectionName = prompt("Please enter new section name");
        if (newSectionName) {
            $.ajax({
                url: baseUrl + "ContentTree/Sectioncms/RenameSection",
                type: "POST",
                async: false,
                data: JSON.stringify({ id: id,
                    name: newSectionName
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                    //                    var tree = $("#tree").data("kendoTreeView");
                    //                    tree.dataSource.read();
                    //                    tree.expandPath([node.id]);
                }
            });
        }
    }

    function removeSection(id) {
        if (confirm("Are you sure to delete the entire section?")) {
            $.ajax({
                url: baseUrl + "ContentTree/TreeCms/RemoveSection",
                type: "POST",
                async: false,
                data: JSON.stringify({
                    id: id
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    location.reload();
                }
            });
        }
    }

    function moveToNewSection(id) {
        var newSectionName = prompt("Please enter new section name");
        if (newSectionName) {
            $.ajax({
                url: baseUrl + "ContentTree/TreeCms/MoveToSection",
                type: "POST",
                async: false,
                data: JSON.stringify({ sourceNode: id,
                    newSectionName: newSectionName
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
//                    var tree = $("#tree").data("kendoTreeView");
//                    tree.dataSource.read();
//                    tree.expandPath([node.id]);
                    location.reload();
                }
            });
        }
    }

    function onSelect(e) {
        console.log('select: ' + this.text(e.node));
        selectedItem = this.dataItem(e.node);
        if (!this.dataItem(e.node).hasChildren) {
            $('#moveToSectionBtn').show();
            $('#renameSectionBtn').hide();
        }
        else {
            $('#moveToSectionBtn').hide();
            $('#renameSectionBtn').show();
        }
    }
    function onDrop(e) {
        var hitMode = e.dropPosition;
        var sourceNode = this.dataItem(e.sourceNode);
        var node = this.dataItem(e.destinationNode);
        console.log('dropped: ' + hitMode + ' ' + node.title);
        var answer = confirm('Are you sure you want to move "'
            + sourceNode.title + '" ' + hitMode + ' "' + node.title + '"?');
        e.setValid(answer);
        if (answer) {
            $.ajax({
                url: baseUrl + "ContentTree/TreeCms/Move",
                type: "POST",
                async: false,
                data: JSON.stringify({ targetNode: node.id,
                    sourceNode: sourceNode.id, hitMode: hitMode
                }),
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //                    var tree = $("#tree").data("kendoTreeView");
                    //                    tree.dataSource.read();
                    location.reload();
                }
            });
        }
    }
});