function _addEventListeners() {

        $('#showWindowButton').mousedown(function () {
            $('#window').jqxWindow('open');
            $('#window').jqxWindow('focus');
        });
    }

    function _createFormElements(theme) {

        $('#window').jqxWindow({
            theme: theme, resizable: true,
            width: 500,
            height: 400,
            minWidth: 300,
            minHeight: 300,
            title: 'New Employee'
        });
        $('#window').jqxWindow('focus');
        $('#showWindowButton').jqxButton({ theme: theme, width: '100px' });
    }

    $(document).ready(function () {

        var theme = getDemoTheme();
        if (theme == undefined) theme = '';
        _addEventListeners();
        _createFormElements(theme);
        $("#jqxWidget").css('visibility', 'visible');
    });