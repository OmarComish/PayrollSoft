 function addEventListeners() {
     $('#showWindowButton').mousedown(function () {
     $('#window').jqxWindow('open');
     $('#window').jqxWindow('focus');
 });
}
function createElements(theme) {
    $('#window').jqxWindow({
                theme: theme, resizable: true,
                width: 500,
                height: 400,
                minWidth: 300,
                minHeight: 300
 });
$('#window').jqxWindow('focus');

$('#showWindowButton').jqxButton({ theme: theme, width: '100px' });
}

$(document).ready(function () {
            var theme = $.data(document.body, 'theme', theme);
            if (theme == undefined) theme = '';
            addEventListeners();
            createElements(theme);
            $("#jqxWidget").css('visibility', 'visible');
});