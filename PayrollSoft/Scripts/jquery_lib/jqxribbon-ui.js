//The Ribbon code...
$(document).ready(function(){

	        $('#fileItemButton').jqxDropDownButton({ arrowSize: 0, height: 26, dropDownWidth: 120, width: 50, theme: 'demoTheme' });
            $('#fileItemButton').jqxDropDownButton('setContent', '<span style="position: relative; line-height: 26px; margin-left:10px;">File</span>');
            $("#save, #saveAs, #open, #close, #exit").jqxButton({ theme: 'demoTheme' });

            // jqxRibbon code
            $("#jqxRibbon").jqxRibbon({
                width: 1200,
                height: 131,
                animationType: "none",
                selectionMode: "click",
                position: "top",
                theme: "demoTheme",
                mode: "default",
                selectedIndex: 1,
                initContent: function (item) {
                    switch (item) {
                        case 0:

                            break;
                        case 1:
                            $("#subscript, #superscript, #bold, #italic, #underline, #strikethrough").jqxToggleButton({ theme: 'demoTheme' });
                            $("#shrinkFont, #copyButton, #cutButton, #growFont, #formatPainter, #clearFormatting, #alignLeft, #alignCenter, #alignRight, #alignJustify, #bulletList, #numberedList, #decreaseIndent, #increaseIndent, #Sort, #lineSpacing, #showHide").jqxButton({ theme: 'demoTheme' });
                            $("#cutButton").jqxTooltip({ position: "mouse", content: "Cut (Ctrl + X)" });
                            $("#copyButton").jqxTooltip({ position: "mouse", content: "Copy (Ctrl + C)" });
                            $("#subscript").jqxTooltip({ position: "mouse", content: "Subscript" });
                            $("#superscript").jqxTooltip({ position: "mouse", content: "Superscript" });
                            $("#bold").jqxTooltip({ position: "mouse", content: "Bold (Ctrl + B)" });
                            $("#italic").jqxTooltip({ position: "mouse", content: "Italic (Ctrl + I)" });
                            $("#underline").jqxTooltip({ position: "mouse", content: "Underline (Ctrl + U)" });
                            $("#strikethrough").jqxTooltip({ position: "mouse", content: "Strikethrough" });
                            $("#alignLeft").jqxTooltip({ position: "mouse", content: "Align Text Left (Ctrl + L)" });
                            $("#alignCenter").jqxTooltip({ position: "mouse", content: "Center (Ctrl + E)" });
                            $("#alignRight").jqxTooltip({ position: "mouse", content: "Align Text Right (Ctrl + R)" });
                            $("#alignJustify").jqxTooltip({ position: "mouse", content: "Justify (Ctrl + J)" });
                            $("#bulletList").jqxTooltip({ position: "mouse", content: "Bulleted List" });
                            $("#numberedList").jqxTooltip({ position: "mouse", content: "Numbered List" });
                            $("#decreaseIndent").jqxTooltip({ position: "mouse", content: "Decrease Indent" });
                            $("#increaseIndent").jqxTooltip({ position: "mouse", content: "Increase Indent" });
                            $("#Sort").jqxTooltip({ position: "mouse", content: "Sort Direction" });
                            $("#lineSpacing").jqxTooltip({ position: "mouse", content: "Line and Paragraph Spacing" });
                            $("#showHide").jqxTooltip({ position: "mouse", content: "Show/Hide special characters" });
                            $("#bucketColor").jqxTooltip({ position: "mouse", content: "Fill style", theme: 'demoTheme' });
                            $("#bucketColor").jqxDropDownButton({
                                width: 42,
                                height: 19,
                                theme: "demoTheme",
                                dropDownWidth: 180,
                                initContent: function () {
                                    $("#bucketColorPicker").jqxColorPicker({
                                        color: "000000",
                                        colorMode: 'hue',
                                        width: 180,
                                        theme: 'demoTheme',
                                        height: 180
                                    }).on('colorchange', function (event) {
                                        $("#bucketColorPreview").css('background', '#' + event.args.color.hex);
                                    });
                                }
                            }).jqxDropDownButton('setContent', '<span style="position:relative; display:inline-block; top: 2px"><div class="icon paintcan"></div><span id="bucketColorPreview" style="display: block; position:absolute;  height: 3px; width: 16px; background:#000"></span></span>');



                            $("#font").jqxDropDownList({
                                source: [
                                    "<span style='font-family: Courier New;'>Courier New</span>",
                                    "<span style='font-family: Times New Roman;'>Times New Roman</span>",
                                    "<span style='font-family: Arial;'>Arial</span>"
                                ],
                                width: 120,
                                height: 21,
                                theme: 'demoTheme',
                                autoDropDownHeight: true,
                                selectedIndex: 1
                            });

                            $("#fontSize").jqxDropDownList({
                                source: [8, 9, 10, 11, 12, 14, 18, 20, 22, 24],
                                theme: 'demoTheme',
                                renderer: function (index, label, value) {
                                    return '<span style="font-size:' + value + 'px;">' + value + '</span>';
                                },
                                width: 70, height: 21, autoDropDownHeight: true, selectedIndex: 2
                            });
                            $("#subscript").on('click', function () {
                                if ($('#superscript').jqxToggleButton('toggled')) {
                                    $('#superscript').jqxToggleButton('toggled', false)
                                }
                            });
                            $("#superscript").on('click', function () {
                                if ($('#subscript').jqxToggleButton('toggled')) {
                                    $('#subscript').jqxToggleButton('toggled', false)
                                }
                            });
                            $("#changeCase").jqxDropDownList({
                                source: ['Sentence Case', 'lowercase', 'UPPERCASE', 'Capitalize Each Word'],
                                theme: 'demoTheme',
                                selectionRenderer: function (object, index, label) {
                                    return '<div class="icon change-case-16" style="top:3px;  position:relative"></div>';
                                },
                                dropDownWidth: 150,
                                autoDropDownHeight: true, selectedIndex: 0, width: 40

                            });

                            $("#fontColor").jqxDropDownButton({
                                width: 100,
                                theme: 'demoTheme',
                                dropDownWidth: 180,
                                height: 21, initContent: function () {
                                    $("#fontColorPicker").jqxColorPicker({
                                        color: "000000",
                                        colorMode: 'hue',
                                        theme: 'demoTheme',
                                        width: 180,
                                        height: 180
                                    }).on('colorchange', function (event) {
                                        $("#fontColorPreview").css('background', '#' + event.args.color.hex);
                                    });
                                }
                            }).jqxDropDownButton('setContent', '<span style="position:relative; display:inline; top: 2px"><div class="icon FontDialogImage"></div><span id="fontColorPreview" style="display: block; position:absolute;  height: 3px; width: 16px; background:#000"></span></span><span style="position:relative; display: inline; top:3px">Font Color</span>');

                            $("#highlightColor").jqxDropDownButton({
                                width: 130,
                                dropDownWidth: 180,
                                theme: 'demoTheme',
                                height: 21,
                                initContent: function () {
                                    $("#highlightColorPicker").jqxColorPicker({
                                        color: "FF0000",
                                        theme: 'demoTheme',
                                        colorMode: 'hue',
                                        width: 180,
                                        height: 180
                                    }).on('colorchange', function (event) {
                                        $("#highlightColorPreview").css('background', '#' + event.args.color.hex);
                                    });
                                }
                            }).jqxDropDownButton('setContent', '<span style="position:relative; display:inline; top: 2px"><div class="icon pencil"></div><span id="highlightColorPreview" style="display: block; position:absolute;  height: 3px; width: 16px; background:#F00"></span></span><span style="position:relative; display: inline; top:3px">Highlight Color</span>');


                            var pasteData = [
                                { label: 'Paste', imageClass: 'icon page_paste' },
                                { label: 'Paste Special', imageClass: 'icon paste_plain' },
                                { label: 'Paste text', imageClass: 'icon paste_word' },
                                { label: 'Paste link', imageClass: 'icon PasteImage' }
                            ];
                            $('#pasteButton').jqxButton({
                                width: 35,
                                theme: 'demoTheme',
                                height: 56
                            }).on('click', function (event) {
                                var text = $(this).find('.pasteText').html();
                                console.log(text + ' clicked');
                            }).on('mousedown', function (event) {
                                event.preventDefault();
                            });
                            $("#pasteDropDown").jqxDropDownList({
                                source: pasteData,
                                width: 22,
                                height: 10,
                                theme: 'demoTheme',
                                animationType: 'none',
                                dropDownWidth: '110px',
                                autoDropDownHeight: true,
                                renderer: function (index, label, value) {
                                    var imgUrl = pasteData[index].imgUrl;
                                    var labelEl = '<span style="font-size: 10px">' + label + '</span>';
                                    var icon = '<span class="' + pasteData[index].imageClass + '" style=""></span>';
                                    return '<span>' + icon + labelEl + '</span>';
                                },
                                selectionRenderer: function (object, index, label) {
                                    return "";
                                },
                                selectedIndex: 0
                            }).on('select', function (event) {
                                var index = event.args.index;
                                var icon = '<span class="' + pasteData[index].imageClass + '" style="zoom: 1.5"></span>';
                                $('#pasteButton').html(icon + '<span class="pasteText">' + pasteData[index].label + '</span>');
                                $('#pasteButton').jqxButton('render');
                            });
                            break;
                        case 2:
                            $("#help, #about, #update").jqxButton({ width: 26, height: 26, theme: 'demoTheme' });
                            break;
                    }
                }
            });

            $('#jqxRibbon').jqxRibbon('disableAt', 0);
});