<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="anteprimapagina.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.anteprimapagina" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="js/swfobject.js"></script>

    <script type="text/javascript" src="js/flexBridge.js"></script>

    <script type="text/javascript">

        $(document).ready(function() {
 
		/* var newH =  $(body).height();
		setFlashHeight(newH); */
 
        });
        function stretch_portal_content() {
            if ($(window).height() > $('body').height()) {
vara n =$(window).height() - ($('body').height() - $(body).height());
alert("n = "+n);                $(body).height(n);
            }
        }
        
        

    </script>   
	<style type="text/css">
        html, body
        {
            background: #ff0000;
            height: 100%;
            margin: 0px;
            margin: 0;
            padding: 0;
        }
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
