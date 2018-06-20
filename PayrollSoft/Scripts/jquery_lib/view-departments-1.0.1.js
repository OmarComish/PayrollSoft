function _createFormElements(title,theme){

      $('#window').jqxWindow({
        theme: theme,
        resizable: true,
        width: 755,
        height: 370,
        minWidth: 300,
        minHeight: 300,
        title: title
       });

      $('#window').jqxWindow('focus');
}


$(document).ready(function(){
   var theme = getDemoTheme();
   var title ='Departments';
   
   _createFormElements(theme,title);

});