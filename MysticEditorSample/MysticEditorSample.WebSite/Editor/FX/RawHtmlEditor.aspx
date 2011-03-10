<%@ Page Language="C#" ValidateRequest="false" AutoEventWireup="true" CodeBehind="RawHtmlEditor.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.RawHtmlEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="SyrinxCkEditor" Namespace="Syrinx.Gui.AspNet" TagPrefix="syx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script type="text/javascript" src="ckeditor/ckeditor.js"></script>

    <script type="text/javascript" src="ckfinder/ckfinder.js"></script>

    <script type="text/javascript" src="js/flexBridge.js"></script>

    <script type="text/javascript" src="js/jquery-1.4.2.min.js"  ></script>

    <script src="js\framesetPage.js" type="text/javascript">
          // commento
    </script>

    <style type="text/css">
        html, body
        {
        font-family: 'Trebuchet MS';
            background: #EEEEEE;
            height: 100%;
            margin: 0;
            padding: 0;
        }
        .editorStyle
        {
            height: 100%;
            margin: 0;
            padding: 0;
        }
        #cke_90_frame
        {
            background: #ddd;
            height: 100%;
        }
        .testogrigiochiaro
        {
        	font-size: 10px;
        	
        }
        .titarancionebig
        {
        	font-size: 15px;
        	
        	color: #ff6600;
        	font-weight:bold;
        }
        .tablestyle
        {
        	height: 50px;
        	vertical-align:middle;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function() {
            h = $(window).height();
            h = h - 100;
           // console.log("ckeditor h =>" + h);
            setTimeout("setHeight()", 1000);
        });
        var h;
        function setHeight() {
            $("#cke_contents_edit").height(h);
          //  console.log("ckeditor h <=" + $("#cke_contents_edit").height());
        }
	
    </script>

</head>
<body onscroll="ScrollLeftToo()">
    <form id="scrollpos" name="scrollpos" runat="server">
    <input type="Hidden" name="y" value="0" />
    <table class="tablestyle" width="100%"  >
        <tr>
            <td style="padding-left:10px;" >
				<font class="testogrigiochiaro">Edita pagina</font>
				<br/> 
				<font class="titarancionebig">
                        <asp:label runat="server" id="title" /> 
                </font>
				<br/> 
				<font class="testogrigiochiaro">
                        <asp:label runat="server" id="path" /> </font>
			</td>
                
            <td align="right" style="padding-right:10px;padding-top:10px;" >
               
                    <asp:ImageButton ImageUrl="~/Editor/img/salva.png" ID="Save" runat="server" Text="Salva" OnClick="Save_Click" />
                    
                
            </td>
        </tr>
    </table>
    <div id="divsize">
        <syx:CkEditor ID="edit" CssClass="editorStyle" runat="server" resizable="false" maximize="true" />
    </div>

    <script type="text/javascript">

        var editor = CKEDITOR.instances.edit;

        CKFinder.SetupCKEditor(editor, 'ckfinder/');
    
    </script>

    </form>

</body>
</html>
