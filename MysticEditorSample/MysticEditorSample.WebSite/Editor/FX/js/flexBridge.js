var SELECTEDPAGEID = 0;
DEBUG = false;
function getFlashVars(){
	var flashvars = {};
                    flashvars.enableDebug = "false";
                    flashvars.contentID = $("#hContentID").attr("value");
                    flashvars.itemID = $("#hItemID").attr("value");
                    flashvars.rootURL = "http://10.12.150.114/cms/editor/FX/";//"";
                    flashvars.isMockTest = "false";
	return 	flashvars
}	

function doLog(msg) {
	showAlert("FLEX:" +msg); 
		
		
}
function showAlert(msg) {
	if (DEBUG == true){
		//IF is IE 6
		 
		if(IsIE6()){
			alert(msg);//console.log(msg); 
		}else{
			alert(msg);//console.log(msg); 
		}
	}
		
		
}
function showEditing(pageid, itemid, type) {

	var url = "http://10.12.150.114/cms/editor/fx/PageView.aspx?idp=" + pageid + "&iditem=" + itemid + "&type=" + type;


 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
	/*var contentH =$('#contentFrame').contents().find('html').height();
	setFlashHeight(contentH);*/


}

function showWidgetEditing(widgetid, itemid, type) {

	var url = "http://10.12.150.114/cms/editor/fx/uleditor.aspx?idwel=" + widgetid + "&iditem=" + itemid + "&type=" + type;


 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
	/*var contentH =$('#contentFrame').contents().find('html').height();
	setFlashHeight(contentH);*/


}
function openPreview(pageid,itemid, type){
	/*var url = "PageView.aspx?pageid="+pageid+"&iditem="+itemid;*/
	var url ="http://10.12.150.114/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid + "&type=" + type;
//	alert("showEditing ="+url );
 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
 	
}

function showPreview(pageid,itemid, type) {
	var url ="http://10.12.150.114/cms/editor/fx/anteprimapagina.aspx?idp="+pageid+"&iditem="+itemid + "&type=" + type;
	/*var url ="http://10.12.150.114/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid;*/
	/*var url ="Preview.aspx?idp="+pageid+"&iditem="+itemid;*/
	
	
 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
		showAlert("showPreview ="+url );
		
}

function showWidgetPreview(widgetid,itemid, type) {
	var url ="http://10.12.150.114/cms/editor/fx/uleditor.aspx?idwel="+widgetid+"&iditem="+itemid + "&type=" + type;
	/*var url ="http://10.12.150.114/cms/editor/fx/PageView.aspx?idp="+pageid+"&iditem="+itemid;*/
	/*var url ="Preview.aspx?idp="+pageid+"&iditem="+itemid;*/
	
	
 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
		showAlert("showPreview ="+url );
		
}
function doPreviewAll(idc,itemid, type) {
	var url ="http://10.12.150.114/cms/editor/fx/anteprima.aspx?idc="+idc+"&iditem="+itemid + "&type=" + type;
	showAlert("doPreviewAll ="+url );
	window.open(url);	
}

function doPublishAll(idc,itemid, type) {
	var url ="http://10.12.150.114/cms/editor/fx/pubblica.aspx?idc="+idc+"&iditem="+itemid + "&type=" + type;
	showAlert("doPublishAll ="+url );
	window.open(url);	
}

function displatHTMLEditor(pageelementid,itemid,type) {
	var url = "RawHtmlEditor.aspx?idpel="+pageelementid+"&iditem="+itemid + "&type=" + type;
 
	/*$("#contentFrame").attr("src", url);*/
	this.location.href=url;
	showAlert("displatHTMLEditor ="+url );
}

/* FLEX callbacks*/

function setFlashHeight(newHeight) {
	 var menuH =$('#ContentEditorMenuFx').height();	
	 
	 var contentH =$('#parent.contentFrame').contents().find('html').height();
		 
	var newH = Math.max(newHeight,menuH) ; ;
	if(contentH!=null)
		newH =Math.max( newH,contentH) ; 
	showAlert("setFlashHeight ="+newH+"| menuH:" + menuH +" contentH:"+contentH );
	
	$('#parent.contentFrame').height(newH);
	$('#ContentEditorMenuFx').height(newH); 
	    
	  
}
function setHeight(height){
	$('#parent.contentFrame').height(height);
	$('#ContentEditorMenuFx').height(height);
//	alert(height);
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
 
	var menuH =$('#parent.contentFrame').width(width);
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

function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
  var rv = -1; // Return value assumes failure.
  if (navigator.appName == 'Microsoft Internet Explorer')
  {
    var ua = navigator.userAgent;
    var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    if (re.exec(ua) != null)
      rv = parseFloat( RegExp.$1 );
  }
  return rv;
}
function checkVersion()
{
  var msg = "You're not using Internet Explorer.";
  var ver = getInternetExplorerVersion();

  if ( ver > -1 )
  {
    if ( ver >= 8.0 ) 
      msg = "You're using a recent copy of Internet Explorer."
    else
      msg = "You should upgrade your copy of Internet Explorer.";
  }
  //alert( msg );
}
function IsIE6()
{
  
  var ver = getInternetExplorerVersion();

 return ver > -1 && ver < 7;

}


