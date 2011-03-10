<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript" src="js/jquery-1.4.min.js"></script>

    <script type="text/javascript" src="js/swfobject.js"></script>

    <script type="text/javascript" src="js/flexBridge.js"></script>
     <script type="text/javascript">
     $(document).ready(function() {

            /*stretch_portal_content();
            $(window).resize(stretch_portal_content);*/

            setTimeout('setSize()', 100);
        });
        function stretch_portal_content() {
            if ($(window).height() > $('body').height()) {
                $('#menuWrapper').height($(window).height() - ($('body').height() - $('#menuWrapper').height()));
            }
        }
        $('.menuButton').hover(
            function() { // Change the input image's source when we "roll on"
                $(this).attr({ src: 'enter_over.gif' });
            },
            function() { // Change the input image's source back to the default on "roll off"
                $(this).attr({ src: 'enter.gif' });
            }
        );
        function querySt(ji) {
            hu = window.location.search.substring(1);
            gy = hu.split("&");
            for (i = 0; i < gy.length; i++) {
                ft = gy[i].split("=");
                if (ft[0] == ji) {
                    return ft[1];
                }
            }
        }
        </script>
        
        <style type="text/css">
        html, body
        {
            background: #182229;
            height: 100%;
            margin: 0px;
            margin: 0;
            padding: 0;
        }
        body
        {
            font-family: "Trebuchet MS";
            color: #eee;
            border: 0; /* overflow: hidden; */
            background: url(   'bgPixel.png' ) repeat;
        }
        #maincontent
        {
            margin-left: 30%;
            width: 70%; /* TODO sisteamre larghezza calcolata */
        }
        .fakeMenu
        {
            height: 40px;
            width: 100%;
            text-align: right;
            vertical-align: middle;
        }
        .innertube
        {
            margin: 0px; /*Margins for inner DIV inside each DIV (to provide padding)*/
        }
        .fakeMenu a, a hover
        {
            height: 40px;
            color: #eee;
            text-decoration: none;
            font-weight: normal;
            font-size: 13px;
        }
        #contentFrame
        {
            background: #222C33;
            margin: 0;
            padding: 0;
            top: 40px;
            width: 100%;
        }
        .menuButton
        {
        }
        .flashDiv
        {
        }
        .alternativeContentStyle
        {
        }
        </style>
        
        <script src="js/frameset.js" type="text/javascript">
        // commento
    </script>
    
</head>

<body>
    <form id="scrollpos" name="scrollpos" runat="server"> 
          <input type="Hidden" name="menuheight" value="0" />
          <input type="Hidden" id="currentSelected" name="currentSelected" value="0" />
          <input type="Hidden" id="oldBack" name="oldBack" value="0" />
          <input type="Hidden" id="oldFont" name="oldFont" value="0" />
        <asp:HiddenField ID="hContentID" runat="server" />
        <asp:HiddenField ID="hItemID" runat="server" />
		<asp:HiddenField ID="hType" runat="server" />
        <script type="text/javascript">

                var flashvars = {};
                flashvars.enableDebug = "false";
                flashvars.contentID = $("#hContentID").attr("value");
                flashvars.itemID = $("#hItemID").attr("value");
				flashvars.type = $("#hType").attr("value"); 
                //flashvars.rootURL = "http://10.12.150.114/cms/editor/Fx/";
				flashvars.rootURL = "http://localhost:1285/editor/Fx/";
                // flashvars.contentsURL = "http://10.12.150.114/contenutiadm/";
                //flashvars.contentsURL = "http://10.12.150.114/fileserver/";
				flashvars.contentsURL = "http://localhost:1285/fileserver/";
                flashvars.isMockTest = "false";

                var params = {};
                params.scale = "showall";

                var attributes = {};
                attributes.id = "ContentEditorMenuFx";
                attributes.styleclass = "ContentEditorMenuFx";
                swfobject.embedSWF("swf/ContentEditorMenuFx.swf", "myAlternativeContent", "100%", "100%", "10.0.0", "swf/expressInstall.swf", flashvars, params, attributes);
        </script>

      <!--  <div id="maincontent">
            <div class="fakeMenu">
                <asp:LinkButton ID="btnPreviewAll" runat="server" Text="Anteprima Sito" />
                <asp:LinkButton ID="btnPublishAll" runat="server" Text="Pubblica Sito" />
            </div>
        </div>-->
           <div id="myAlternativeContent" class="alternativeContentStyle">
            <h1>
                Alternative content</h1>
            <p>
                <a href="http://www.adobe.com/go/getflashplayer">
                    <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif"
                        alt="Get Adobe Flash player" border="0" /></a>
            </p>
        </div>
     <!--   <div id="divsize" name="divsize">
        </div>-->
        </form>
        <script type="text/javascript" defer="defer">
        function showEditing(pageid, itemid, type) {

            var url = "http://localhost:1285/editor/fx/PageView.aspx?idp=" + pageid + "&iditem=" + itemid + "&type=" + type;


 /*$("parent.contentFrame").attr("location", url);  */
 parent.contentFrame.location = url;
            /*var contentH = $('#contentFrame').contents().find('html').height();
            setFlashHeight(contentH);*/

        }


        function setFlashHeightNO(newHeight) {
            var menuH = $('#ContentEditorMenuFx').height();


            var contentH = $('#contentFrame').contents().find('html').height();


            var newH = newHeight;
            if (contentH != null)
                newH = Math.max(newHeight, contentH);
            showAlert("setFlashHeight =" + newH + "| menuH:" + menuH + " contentH:" + contentH);

            $('#contentFrame').height(newH);
            $('#ContentEditorMenuFx').height(newH);
        }
    </script>
        
    
   
    
</body>
</html>
