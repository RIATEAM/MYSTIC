<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageView.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.PageView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
    <script type="text/javascript" src="js/jquery-1.4.min.js"></script>

    <script type="text/javascript" src="js/flexBridge.js"></script>

    <script type="text/javascript" src="js/swfobject.js"></script>

    <style type="text/css">
    html, body
        {
            background: #222C33;
            height: 100%;
            margin: 0;
            padding: 0;
        }
        body
        {
            overflow: visible;
            color: #eee;
            </style>

    <script type="text/javascript">
        var pageid;
        $(document).ready(function() {
           
        });
        
        function getFlexApp(appName) {
            if (navigator.appName.indexOf("Microsoft") != -1) {
                return window[appName];
            }
            else {
                return document[appName];
            }
        }
        function callFlex() {
            var flex = $("#ContentEditorPageViewFx")[0]; //getFlexApp("ContentEditorPageViewFx");
            flex.Flex_setPageid(pageid);
        }
  

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID="hItemID" runat="server" />
        <asp:HiddenField ID="hPageid" runat="server" />
        <asp:HiddenField ID="hType" runat="server" />

        <script type="text/javascript">
            //swfobject.registerObject("IrideFx", "10.0.0", "swf/expressInstall.swf");

            var flashvars = {};
            flashvars.enableDebug = "false";
            flashvars.pageid = $("#hPageid").attr("value");
            flashvars.itemID = $("#hItemID").attr("value");
            flashvars.type = $("#hType").attr("value");
            flashvars.rootURL = "http://localhost:1285/editor/Fx/";
            // flashvars.contentsURL = "http://10.12.150.114/contenutiadm/";
            flashvars.contentsURL = "http://localhost:1285/fileserver/";
            flashvars.isMockTest = "false";

            var params = {};
            params.scale = "showall"; 

            var attributes = {};
            attributes.id = "ContentEditorPageViewFx";
            attributes.styleclass = "ContentEditorPageViewFx";
            swfobject.embedSWF("swf/ContentEditorPageViewFx.swf", "myAlternativeContent", "100%", "100%", "10.0.0", "swf/expressInstall.swf", flashvars, params, attributes);
        </script>

        <div id="myAlternativeContent">
            <h1>
                Alternative content</h1>
            <p>
                <a href="http://www.adobe.com/go/getflashplayer">
                    <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"
                        alt="Get Adobe Flash player" border="0" /></a>
            </p>
        </div>
    </div>
    
    </form>
    <script type="text/javascript">

        $("#contentFrame").height(document.body.offsetHeight);
 
    </script>
</body>
</html>
