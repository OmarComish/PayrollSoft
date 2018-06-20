/*
jQWidgets v2.9.3 (2013-July-11)
Copyright (c) 2011-2013 jQWidgets.
License: http://jqwidgets.com/license/
*/

(function(a){a.jqx.jqxWidget("jqxDropDownButton","",{});a.extend(a.jqx._jqxDropDownButton.prototype,{defineInstance:function(){this.disabled=false;this.width=null;this.height=null;this.arrowSize=19;this.enableHover=true;if(this.openDelay==undefined){this.openDelay=250}if(this.closeDelay==undefined){this.closeDelay=300}this.animationType="default";this.enableBrowserBoundsDetection=false;this.dropDownHorizontalAlignment="left";this.popupZIndex=20000;this.autoOpen=false;this.rtl=false;this.initContent=null;this.aria={"aria-disabled":{name:"disabled",type:"boolean"}};this.events=["open","close","opening","closing"]},createInstance:function(h){this.isanimating=false;var c=a("<div tabIndex=0 style='background-color: transparent; -webkit-appearance: none; outline: none; width:100%; height: 100%; padding: 0px; margin: 0px; border: 0px; position: relative;'><div id='dropDownButtonWrapper' style='outline: none; background-color: transparent; border: none; float: left; width:100%; height: 100%; position: relative;'><div id='dropDownButtonContent' style='outline: none; background-color: transparent; border: none; float: left; position: relative;'/><div id='dropDownButtonArrow' style='background-color: transparent; border: none; float: right; position: relative;'><div></div></div></div></div>");a.jqx.aria(this);this.popupContent=this.host.children();this.host.attr("role","button");if(this.popupContent.length==0){this.popupContent=a("<div>"+this.host.text()+"</div>");this.popupContent.css("display","block");this.element.innerHTML=""}else{this.popupContent.detach()}var i=this;this.addHandler(this.host,"loadContent",function(e){i._arrange()});try{var f="dropDownButtonPopup"+this.element.id;var d=a(a.find("#"+f));if(d.length>0){d.remove()}a.jqx.aria(this,"aria-haspopup",true);a.jqx.aria(this,"aria-owns",f);var b=a("<div class='dropDownButton' style='overflow: hidden; left: 0px; top: 0px; position: absolute;' id='dropDownButtonPopup"+this.element.id+"'></div>");b.addClass(this.toThemeProperty("jqx-widget-content"));b.addClass(this.toThemeProperty("jqx-dropdownbutton-popup"));b.addClass(this.toThemeProperty("jqx-popup"));b.addClass(this.toThemeProperty("jqx-rc-all"));b.css("z-index",this.popupZIndex);if(a.jqx.browser.msie){b.addClass(this.toThemeProperty("jqx-noshadow"))}this.popupContent.appendTo(b);b.appendTo(document.body);this.container=b;if(this.animationType=="none"){this.container.css("visibility","hidden")}else{this.container.css("visibility","hidden")}}catch(g){}this.touch=a.jqx.mobile.isTouchDevice();this.dropDownButtonStructure=c;this.host.append(c);this.dropDownButtonWrapper=this.host.find("#dropDownButtonWrapper");this.dropDownButtonArrow=this.host.find("#dropDownButtonArrow");this.arrow=a(this.dropDownButtonArrow.children()[0]);this.dropDownButtonContent=this.host.find("#dropDownButtonContent");this.dropDownButtonContent.addClass(this.toThemeProperty("jqx-dropdownlist-content"));this.dropDownButtonWrapper.addClass(this.toThemeProperty("jqx-disableselect"));if(this.rtl){this.dropDownButtonContent.addClass(this.toThemeProperty("jqx-rtl"))}this.addHandler(this.dropDownButtonWrapper,"selectstart",function(){return false});this.dropDownButtonWrapper[0].id="dropDownButtonWrapper"+this.element.id;this.dropDownButtonArrow[0].id="dropDownButtonArrow"+this.element.id;this.dropDownButtonContent[0].id="dropDownButtonContent"+this.element.id;var k=this;this.propertyChangeMap.disabled=function(e,m,l,n){if(n){e.host.addClass(k.toThemeProperty("jqx-dropdownlist-state-disabled"));e.host.addClass(k.toThemeProperty("jqx-fill-state-disabled"));e.dropDownButtonContent.addClass(k.toThemeProperty("jqx-dropdownlist-content-disabled"))}else{e.host.removeClass(k.toThemeProperty("jqx-dropdownlist-state-disabled"));e.host.removeClass(k.toThemeProperty("jqx-fill-state-disabled"));e.dropDownButtonContent.removeClass(k.toThemeProperty("jqx-dropdownlist-content-disabled"))}a.jqx.aria(e,"aria-disabled",e.disabled)};if(this.disabled){this.host.addClass(this.toThemeProperty("jqx-dropdownlist-state-disabled"));this.host.addClass(this.toThemeProperty("jqx-fill-state-disabled"));this.dropDownButtonContent.addClass(this.toThemeProperty("jqx-dropdownlist-content-disabled"))}this.host.addClass(this.toThemeProperty("jqx-widget"));this.host.addClass(this.toThemeProperty("jqx-widget-content"));this.host.addClass(this.toThemeProperty("jqx-dropdownlist-state-normal"));this.host.addClass(this.toThemeProperty("jqx-rc-all"));this.host.addClass(this.toThemeProperty("jqx-fill-state-normal"));this.arrow.addClass(this.toThemeProperty("jqx-icon-arrow-down"));this.arrow.addClass(this.toThemeProperty("jqx-icon"));this._setSize();this.render();if(a.jqx.browser.msie&&a.jqx.browser.version<8){this.container.css("display","none");if(this.host.parents(".jqx-window").length>0){var j=this.host.parents(".jqx-window").css("z-index");b.css("z-index",j+10);this.container.css("z-index",j+10)}}if(!this._hiddenParentTimer){var i=this;if(a.jqx.isHidden(this.host)){this._hiddenParentTimer=setInterval(function(){try{if(!a.jqx.isHidden(i.host)){clearInterval(i._hiddenParentTimer);i._hiddenParentTimer=0;i._setSize()}}catch(e){}},10)}}},setContent:function(b){this.dropDownButtonContent.children().remove();this.dropDownButtonContent[0].innerHTML="";this.dropDownButtonContent.append(b)},val:function(b){if(arguments.length==0||typeof(b)=="object"){return this.dropDownButtonContent.text(b)}else{this.dropDownButtonContent.html(b)}},getContent:function(){if(this.dropDownButtonContent.children().length>0){return this.dropDownButtonContent.children()}return this.dropDownButtonContent.text()},_setSize:function(){if(this.width!=null&&this.width.toString().indexOf("px")!=-1){this.host.width(this.width)}else{if(this.width!=undefined&&!isNaN(this.width)){this.host.width(this.width)}}if(this.height!=null&&this.height.toString().indexOf("px")!=-1){this.host.height(this.height)}else{if(this.height!=undefined&&!isNaN(this.height)){this.host.height(this.height)}}var d=false;if(this.width!=null&&this.width.toString().indexOf("%")!=-1){d=true;this.host.width(this.width)}if(this.height!=null&&this.height.toString().indexOf("%")!=-1){d=true;this.host.height(this.height)}if(d){this.refresh(false);var c=this;var b=this.host.width();a.jqx.utilities.resize(this.host,function(){c._arrange()})}},isOpened:function(){var c=this;var b=a.data(document.body,"openedJQXButton"+this.element.id);if(b!=null&&b==c.popupContent){return true}return false},focus:function(){try{this.host.focus()}catch(b){}},render:function(){this.removeHandlers();var b=this;var c=false;if(!this.touch){this.host.hover(function(){if(!b.disabled&&b.enableHover){c=true;b.host.addClass(b.toThemeProperty("jqx-dropdownlist-state-hover"));b.arrow.addClass(b.toThemeProperty("jqx-icon-arrow-down-hover"));b.host.addClass(b.toThemeProperty("jqx-fill-state-hover"))}},function(){if(!b.disabled&&b.enableHover){b.host.removeClass(b.toThemeProperty("jqx-dropdownlist-state-hover"));b.host.removeClass(b.toThemeProperty("jqx-fill-state-hover"));b.arrow.removeClass(b.toThemeProperty("jqx-icon-arrow-down-hover"));c=false}})}if(b.autoOpen){this.addHandler(this.host,"mouseenter",function(){var d=b.isOpened();if(!d&&b.autoOpen){b.open();b.host.focus()}});this.addHandler(a(document),"mousemove."+b.element.id,function(g){var f=b.isOpened();if(f&&b.autoOpen){var k=b.host.coord();var j=k.top;var i=k.left;var h=b.container.coord();var d=h.left;var e=h.top;canClose=true;if(g.pageY>=j&&g.pageY<=j+b.host.height()){if(g.pageX>=i&&g.pageX<i+b.host.width()){canClose=false}}if(g.pageY>=e&&g.pageY<=e+b.container.height()){if(g.pageX>=d&&g.pageX<d+b.container.width()){canClose=false}}if(canClose){b.close()}}})}this.addHandler(this.dropDownButtonWrapper,"mousedown",function(e){if(!b.disabled){var d=b.container.css("visibility")=="visible";if(!b.isanimating){if(d){b.close()}else{b.open()}}}});if(this.touch){this.addHandler(a(document),a.jqx.mobile.getTouchEventName("touchstart")+"."+this.element.id,b.closeOpenedDropDown,{me:this,popup:this.container,id:this.element.id})}this.addHandler(a(document),"mousedown."+this.element.id,b.closeOpenedDropDown,{me:this,popup:this.container,id:this.element.id});this.addHandler(this.host,"keydown",function(e){var d=b.container.css("visibility")=="visible";if(b.host.css("display")=="none"){return true}if(e.keyCode=="13"){if(!b.isanimating){if(d){b.close()}else{b.open()}}}if(e.keyCode==115){if(!b.isanimating){if(!b.isOpened()){b.open()}else{if(b.isOpened()){b.close()}}}return false}if(e.altKey){if(b.host.css("display")=="block"){if(e.keyCode==38){if(b.isOpened()){b.close()}}else{if(e.keyCode==40){if(!b.isOpened()){b.open()}}}}}if(e.keyCode=="27"){if(!b.ishiding){b.close();if(b.tempSelectedIndex!=undefined){b.selectIndex(b.tempSelectedIndex)}}}});this.addHandler(this.host.find("div:first"),"focus",function(){b.host.addClass(b.toThemeProperty("jqx-dropdownlist-state-focus"));b.host.addClass(b.toThemeProperty("jqx-fill-state-focus"))});this.addHandler(this.host.find("div:first"),"blur",function(){b.host.removeClass(b.toThemeProperty("jqx-dropdownlist-state-focus"));b.host.removeClass(b.toThemeProperty("jqx-fill-state-focus"))})},removeHandlers:function(){var b=this;this.removeHandler(this.dropDownButtonWrapper,"mousedown");this.removeHandler(this.host,"keydown");this.removeHandler(this.host.find("div:first"),"focus");this.removeHandler(this.host.find("div:first"),"blur");this.removeHandler(this.host,"mouseenter");this.removeHandler(a(document),"mousemove."+b.element.id)},_findPos:function(c){while(c&&(c.type=="hidden"||c.nodeType!=1||a.expr.filters.hidden(c))){c=c.nextSibling}var b=a(c).coord(true);return[b.left,b.top]},testOffset:function(h,f,c){var g=h.outerWidth();var j=h.outerHeight();var i=a(window).width()+a(window).scrollLeft();var e=a(window).height()+a(window).scrollTop();if(f.left+g>i){if(g>this.host.width()){var d=this.host.coord().left;var b=g-this.host.width();f.left=d-b+2}}if(f.left<0){f.left=parseInt(this.host.coord().left)+"px"}if(f.top+j>e){f.top-=Math.abs(j+c)}return f},_getBodyOffset:function(){var c=0;var b=0;if(a("body").css("border-top-width")!="0px"){c=parseInt(a("body").css("border-top-width"));if(isNaN(c)){c=0}}if(a("body").css("border-left-width")!="0px"){b=parseInt(a("body").css("border-left-width"));if(isNaN(b)){b=0}}return{left:b,top:c}},open:function(){a.jqx.aria(this,"aria-expanded",true);var p=this;p._raiseEvent("2");var c=this.popupContent;var m=a(window).scrollTop();var i=a(window).scrollLeft();var k=parseInt(this._findPos(this.host[0])[1])+parseInt(this.host.outerHeight())-1+"px";var f,h=parseInt(Math.round(this.host.coord(true).left));f=h+"px";var o=a.jqx.mobile.isSafariMobileBrowser()||a.jqx.mobile.isWindowsPhone();var d=a.jqx.utilities.hasTransform(this.host);this.ishiding=false;this.tempSelectedIndex=this.selectedIndex;if(d||(o!=null&&o)){f=a.jqx.mobile.getLeftPos(this.element);k=a.jqx.mobile.getTopPos(this.element)+parseInt(this.host.outerHeight());if(a("body").css("border-top-width")!="0px"){k=parseInt(k)-this._getBodyOffset().top+"px"}if(a("body").css("border-left-width")!="0px"){f=parseInt(f)-this._getBodyOffset().left+"px"}}c.stop();this.host.addClass(this.toThemeProperty("jqx-dropdownlist-state-selected"));this.host.addClass(this.toThemeProperty("jqx-fill-state-pressed"));this.arrow.addClass(this.toThemeProperty("jqx-icon-arrow-down-selected"));var g=false;if(a.jqx.browser.msie&&a.jqx.browser.version<8){g=true}if(g){this.container.css("display","block")}this.container.css("left",f);this.container.css("top",k);var e=true;var q=false;if(this.dropDownHorizontalAlignment=="right"||this.rtl){var l=this.container.width();var b=Math.abs(l-this.host.width());if(l>this.host.width()){this.container.css("left",parseInt(Math.round(h))-b+"px")}else{this.container.css("left",parseInt(Math.round(h))+b+"px")}}if(this.enableBrowserBoundsDetection){var j=this.testOffset(c,{left:parseInt(this.container.css("left")),top:parseInt(k)},parseInt(this.host.outerHeight()));if(parseInt(this.container.css("top"))!=j.top){q=true;this.container.height(c.outerHeight());c.css("top",23);if(this.interval){clearInterval(this.interval)}this.interval=setInterval(function(){if(c.outerHeight()!=p.container.height()){var r=p.testOffset(c,{left:parseInt(p.container.css("left")),top:parseInt(k)},parseInt(p.host.outerHeight()));p.container.css("top",r.top);p.container.height(c.outerHeight())}},50)}else{c.css("top",0)}this.container.css("top",j.top);if(parseInt(this.container.css("left"))!=j.left){this.container.css("left",j.left)}}if(this.animationType=="none"){this.container.css("visibility","visible");a.data(document.body,"openedJQXButtonParent",p);a.data(document.body,"openedJQXButton"+this.element.id,c);c.css("margin-top",0);c.css("opacity",1);this._raiseEvent("0")}else{this.container.css("visibility","visible");var n=c.outerHeight();p.isanimating=true;if(this.animationType=="fade"){c.css("margin-top",0);c.css("opacity",0);c.animate({opacity:1},this.openDelay,function(){a.data(document.body,"openedJQXButtonParent",p);a.data(document.body,"openedJQXButton"+p.element.id,c);p.ishiding=false;p.isanimating=false;p._raiseEvent("0")})}else{c.css("opacity",1);if(q){c.css("margin-top",n)}else{c.css("margin-top",-n)}c.animate({"margin-top":0},this.openDelay,function(){a.data(document.body,"openedJQXButtonParent",p);a.data(document.body,"openedJQXButton"+p.element.id,c);p.ishiding=false;p.isanimating=false;p._raiseEvent("0")})}}if(!q){this.host.addClass(this.toThemeProperty("jqx-rc-b-expanded"));this.container.addClass(this.toThemeProperty("jqx-rc-t-expanded"))}else{this.host.addClass(this.toThemeProperty("jqx-rc-t-expanded"));this.container.addClass(this.toThemeProperty("jqx-rc-b-expanded"))}this.container.addClass(this.toThemeProperty("jqx-fill-state-focus"))},close:function(){a.jqx.aria(this,"aria-expanded",false);var e=this.popupContent;var d=this.container;var f=this;f._raiseEvent("3");var c=false;if(a.jqx.browser.msie&&a.jqx.browser.version<8){c=true}if(!this.isOpened()){return}a.data(document.body,"openedJQXButton"+this.element.id,null);if(this.animationType=="none"){this.container.css("visibility","hidden");if(c){this.container.css("display","none")}}else{if(!f.ishiding){f.isanimating=true;e.stop();var b=e.outerHeight();e.css("margin-top",0);var g=-b;if(parseInt(this.container.coord().top)<parseInt(this.host.coord().top)){g=b}if(this.animationType=="fade"){e.css({opacity:1});e.animate({opacity:0},this.closeDelay,function(){d.css("visibility","hidden");f.isanimating=false;f.ishiding=false;if(c){d.css("display","none")}})}else{e.animate({"margin-top":g},this.closeDelay,function(){d.css("visibility","hidden");f.isanimating=false;f.ishiding=false;if(c){d.css("display","none")}})}}}this.ishiding=true;this.host.removeClass(this.toThemeProperty("jqx-dropdownlist-state-selected"));this.host.removeClass(this.toThemeProperty("jqx-fill-state-pressed"));this.arrow.removeClass(this.toThemeProperty("jqx-icon-arrow-down-selected"));this.host.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));this.container.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));this.host.removeClass(this.toThemeProperty("jqx-rc-t-expanded"));this.container.removeClass(this.toThemeProperty("jqx-rc-b-expanded"));this.container.removeClass(this.toThemeProperty("jqx-fill-state-focus"));this._raiseEvent("1")},closeOpenedDropDown:function(e){var c=e.data.me;var b=a(e.target);if(a(e.target).ischildof(e.data.me.host)){return true}if(a(e.target).ischildof(e.data.me.popupContent)){return true}var f=c;var d=false;a.each(b.parents(),function(){if(this.className!="undefined"){if(this.className.indexOf("dropDownButton")!=-1){d=true;return false}}});if(!d){c.close()}return true},refresh:function(){this._arrange()},_arrange:function(){var f=parseInt(this.host.width());var b=parseInt(this.host.height());var e=this.arrowSize;var d=this.arrowSize;var g=3;var c=f-d-2*g;if(c>0){this.dropDownButtonContent.width(c+"px")}this.dropDownButtonContent.height(b);this.dropDownButtonContent.css("left",0);this.dropDownButtonContent.css("top",0);this.dropDownButtonArrow.width(d);this.dropDownButtonArrow.height(b);if(this.rtl){this.dropDownButtonArrow.css("float","left");this.dropDownButtonContent.css("float","right");this.dropDownButtonContent.css("left",-g)}},destroy:function(){this.removeHandler(this.dropDownButtonWrapper,"selectstart");this.removeHandler(this.dropDownButtonWrapper,"mousedown");this.removeHandler(this.host,"keydown");this.host.removeClass();this.removeHandler(a(document),"mousedown."+this.element.id,self.closeOpenedDropDown);this.host.remove();this.container.remove()},_raiseEvent:function(f,c){if(c==undefined){c={owner:null}}if(f==0&&!this.contentInitialized){if(this.initContent){this.initContent();this.contentInitialized=true}}var d=this.events[f];args=c;args.owner=this;var e=new jQuery.Event(d);e.owner=this;if(f==2||f==3||f==4){e.args=c}var b=this.host.trigger(e);return b},propertyChangedHandler:function(b,c,e,d){if(this.isInitialized==undefined||this.isInitialized==false){return}if(c=="rtl"){if(d){b.dropDownButtonArrow.css("float","left");b.dropDownButtonContent.css("float","right")}else{b.dropDownButtonArrow.css("float","right");b.dropDownButtonContent.css("float","left")}}if(c=="autoOpen"){b.render()}if(c=="theme"&&d!=null){a.jqx.utilities.setTheme(e,d,b.host)}if(c=="width"||c=="height"){b._setSize();b._arrange()}}})})(jQuery);