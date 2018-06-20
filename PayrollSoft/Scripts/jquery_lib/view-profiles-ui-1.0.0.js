function _createFormElements(theme, title, cellsrenderer, dataAdapter) {

    $('#window').jqxWindow({
        theme: theme,
        resizable: true,
        width: 755,
        height: 370,
        minWidth: 300,
        minHeight: 300,
        title: title,
        initContent: function () {
            $("#jqxgrid").jqxGrid({

                width: 745,
                source: dataAdapter,
                theme: theme,
                pageable: true,
                autoheight: true,
                sortable: true,
                altrows: true,
                enabletooltips: true,
                editable: true,
                selectionmode: 'multiplecellsadvanced',
                columns: [
				                  { text: 'Product Name', datafield: 'ProductName', width: 250 },
				                  { text: 'Quantity per Unit', datafield: 'QuantityPerUnit', cellsalign: 'right', align: 'right', width: 120 },
				                  { text: 'Unit Price', datafield: 'UnitPrice', align: 'right', cellsalign: 'right', cellsformat: 'c2', width: 100 },
				                  { text: 'Units In Stock', datafield: 'UnitsInStock', cellsalign: 'right', cellsrenderer: cellsrenderer, width: 100 },
				                  { text: 'Discontinued', columntype: 'checkbox', datafield: 'Discontinued' },
				                  { text: 'Edit', datafield: 'Edit', width: 50, columntype: 'button', cellsrenderer: function () {
				                      return "Edit";
				                  } 
				                  },
				                  { text: 'Delete', datafield: 'Delete', width: 50, columntype: 'button', cellsrenderer: function () {
				                      return "Delete";
				                  } 
				                  }
				                ]
            });
        }
    });

    $('#window').jqxWindow('focus');

}



$(document).ready(function () {

    var theme = getDemoTheme();
    var title = 'Profiles';
    var url = "sampledata/products.xml";

    // prepare the data
    var source =
            {
                datatype: "xml",
                datafields: [
                    { name: 'ProductName', type: 'string' },
                    { name: 'QuantityPerUnit', type: 'int' },
                    { name: 'UnitPrice', type: 'float' },
                    { name: 'UnitsInStock', type: 'float' },
                    { name: 'Discontinued', type: 'bool' }
                ],
                root: "Products",
                record: "Product",
                id: 'ProductID',
                url: url
            };

    var cellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        if (value < 20) {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #ff0000;">' + value + '</span>';
        }
        else {
            return '<span style="margin: 4px; float: ' + columnproperties.cellsalign + '; color: #008000;">' + value + '</span>';
        }
    }

    var dataAdapter = new $.jqx.dataAdapter(source, {
        downloadComplete: function (data, status, xhr) { },
        loadComplete: function (data) { },
        loadError: function (xhr, status, error) { }
    });

    //$("#jqxMenu").jqxMenu({ width: '120', mode: 'vertical', theme: theme });
   //$("#jqxMenu").css('visibility', 'visible');

    //display the window

    _createFormElements(theme, title, cellsrenderer, dataAdapter);

});
            	