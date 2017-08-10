$(document).ready(function () {

    $('.text-box').addClass('k-textbox');

    var cssEditor = new Behave({
        textarea: document.getElementById('Style'),
        replaceTab: true,
        softTabs: true,
        tabSize: 4,
        autoOpen: true,
        overwrite: true,
        autoStrip: true,
        autoIndent: true
    });

    var layoutEditor = new Behave({
        textarea: document.getElementById('Template'),
        replaceTab: true,
        softTabs: true,
        tabSize: 4,
        autoOpen: true,
        overwrite: true,
        autoStrip: true,
        autoIndent: true
    });
});