$(document).ready(function () {

    var theme = getDemoTheme();


    $("#jqxMenu").jqxMenu({ source: source, width: '180', height: '300', mode: 'vertical', theme: theme });
    $("#jqxMenu").css('visibility', 'visible');

    $('#jqxTabs').jqxTabs({ height: 390, width: 600, theme: theme });


});