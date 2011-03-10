var SELECTEDPAGEID = 0;
DEBUG = true;
function getFlashVars(){
	var flashvars = {};
                    flashvars.enableDebug = "false";
                    flashvars.contentID = $("#hContentID").attr("value");
                    flashvars.itemID = $("#hItemID").attr("value");
                    flashvars.rootURL = "http://www.editor.welsy.org/cms/editor/FX/";//"";
                    flashvars.isMockTest = "false";
	return 	flashvars
}	

function doLog(msg) {
	showAlert("FLEX:" +msg); 
		
		
}
function showAlert(msg) {
	if (DEBUG == true)
	alert(msg);//	console.log(msg);//alert(msg);
		
		
}
function showEditing2(pageid,itemid) {
	
	/*var url = "PageView.aspx?pageid="+pageid+"&iditem="+itemid;*/
	var url ="http://www.editor.welsy.org/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid;
	
	
	/*var url ="http://www.editor.welsy.org/cms/editor/fx/anteprimapagina.aspx?idp="+pageid+"&iditem="+itemid;*/
	 showAlert("showEditing ="+url );
	  $("#contentFrame").attr("src", url);   
	 
}
function openPreview(pageid,itemid){
	/*var url = "PageView.aspx?pageid="+pageid+"&iditem="+itemid;*/
	var url ="http://www.editor.welsy.org/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid;
//	alert("showEditing ="+url );
	$("#contentFrame").attr("src", url); 
 	
}

function showPreview(pageid,itemid) {
	var url ="http://www.editor.welsy.org/cms/editor/fx/anteprimapagina.aspx?idp="+pageid+"&iditem="+itemid;
	/*var url ="http://www.editor.welsy.org/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid;*/
	/*var url ="Preview.aspx?idp="+pageid+"&iditem="+itemid;*/
	
	
	$("#contentFrame").attr("src", url);  
 
 
		showAlert("showPreview ="+url );
}
function displatHTMLEditor(pageelementid,itemid) {
	var url = "RawHtmlEditor.aspx?idpel="+pageelementid+"&iditem="+itemid;
 
	/*$("#contentFrame").attr("src", url);*/
	this.location.href=url;
	showAlert("displatHTMLEditor ="+url );
}

/* FLEX callbacks*/

function setFlashHeight(newHeight) {
	 var menuH =$('#ContentEditorMenuFx').height();
	
	 
	 var contentH =$('#contentFrame').contents().find('html').height();
	
	 
	var newH = newHeight;
	if(contentH!=null)
		newH =Math.max(newHeight,contentH) ; 
	showAlert("setFlashHeight ="+newH+"| menuH:" + menuH +" contentH:"+contentH );
	
	$('#contentFrame').height(newH);
	$('#ContentEditorMenuFx').height(newH); 
	    
	  
}
function setHeight(height){
	$('#contentFrame').height(height);
	$('#ContentEditorMenuFx').height(height);
	alert(height);
}
function setFlashWidth(newWidth) {
 
	 $('#ContentEditorMenuFx').width(newWidth);
	  var oldWidth =$('#maincontent').width();
	 /* if(newWidth>60)
		$('#maincontent').width(oldWidth+newWidth);
	  else
		$('#maincontent').width(oldWidth-newWidth);
	showAlert("setFlashWidth ="+newWidth+"| oldWidth:" + oldWidth +" $('#maincontent').width():"+$('#maincontent').width() );*/
}

function setHTMLWrapperWidth(width) {
 
	var menuH =$('#contentFrame').width(width);
 	showAlert("setHTMLWrapperWidth ="+width );
}




function setCurrentAction(action,pageid,itemid) {
 
 	showAlert("setCurrentAction ="+action );
	if(action=="EDIT"){
		showEditing(pageid);
	}
	else{
		showPreview(pageid);
	}
}
 

function showFancyBox(url, title, width, height) {

	var frameUrl = url;
	width = 600;
	height = 500;
	showAlert("showFancyBox : " + frameUrl + " ( " + width + "," + height + ")");
 
	/*$.fancybox({
		'padding': 0,
		'margin': 0,

		'transitionIn': 'elastic',
		'transitionOut': 'elastic',
		'title': title,
		'width': width,
		'height': height,
		'href': frameUrl,
		'type': 'iframe',
		'orig': $("#fakeLightboxOrigin")[0]
	});*/

	$.colorbox({ width: "80%", height: "80%", iframe: true });
	return false;
}


function fadeIn(width, Pageid) {
	SELECTEDPAGEID = Pageid;
	$('#fakeLightboxWrapper').width(width);
	showAlert("SELECTEDPAGEID:" + SELECTEDPAGEID);
	$('#fakeLightboxWrapper').fadeIn('slow', function() {
		// Animation complete
		 
	});
}
function fadeOut() {

	$('#fakeLightboxWrapper').fadeOut('slow', function() {
		// Animation complete

	});
}


/*************** UTILS ***********/
function querySt(ji) {
	hu = window.location.search.substring(1);
	gy = hu.split("&");
	for (i=0;i<gy.length;i++) {
		ft = gy[i].split("=");
		if (ft[0] == ji) {
			return ft[1];
		}
	}
}
function getFlexApp(appName)
{
  if (navigator.appName.indexOf ("Microsoft") !=-1)
  {
    return window[appName];
  }
  else
  {
    return document[appName];
  }
}
      function getFlashMovie(movieName) {
            var isIE = navigator.appName.indexOf("Microsoft") != -1;
            return (isIE) ? window[movieName] : document[movieName];
        }
        function formSend() {
            var text = document.htmlForm.sendField.value; getFlashMovie("ExternalInterfaceExample").sendTextToFlash(text);
        }
        function getTextFromFlash(str) {
            document.htmlForm.receivedField.value = "From Flash: " + str; return str + " received";
        } 

