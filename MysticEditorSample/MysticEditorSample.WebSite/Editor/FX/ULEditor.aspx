<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ULEditor.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.ULEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="js/jquery-1.4.2.min.js"></script>

    <script src="js\framesetPage.js" type="text/javascript">
          // commento
    </script>

    <style type="text/css">
        .style1
        {
            width: 300px;
        }
    </style>
    <style type="text/css">
        .corpo
        {
            border: #B7BABC 1px solid;
            background: #FFFFFF;
        }
        .labelValue
        {
            color: #333333;
            font-size: 13px;
            font-weight: bold;
            font-family: Trebuchet MS;
        }
        .topMenu
        {
            height: 40px;
            background: #EEEEEE;
        }
        html, body
        {
            background: #EEEEEE;
            height: 100%;
            margin: 0;
            padding: 0;
        }
        .TextInput
        {
            font-family: Trebuchet MS;
            border: #DDDDDD 1px solid;
            padding-bottom: 5;
            padding-top: 5;
            padding-left: 5;
            padding-right: 5;
            color: #333333;
            font-size: 12px;
            height: 36px;
            width: 500px;
        }
        .ButtonInput
        {
            font-family: Trebuchet MS;
            border: #DDDDDD 1px solid;
            padding-bottom: 5;
            padding-top: 5;
            padding-left: 5;
            padding-right: 5;
            color: #333333;
            font-size: 12px;
            height: 36px;
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
    <form id="scrollpos" name="scrollpos">
    <input type="Hidden" name="y" value="0" />
    </form>
    <form runat="server">
    <div class="topMenu" align="right">
        <asp:ImageButton ID="ImageButton1" ImageUrl="~/Editor/img/salva.png" runat="server" Text="Salva"
            OnClick="Save_Click" />
    </div>
    <div class="corpo">
        <table style="width: 100%;">
            <tr>
                <td style="width: 70px" align="center">
                    <asp:Label class="labelValue" ID="Label2" runat="server" Text="Link"></asp:Label>
                </td>
                <td class="style1">
                    <asp:TextBox class="TextInput" ID="Link" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="width: 70px" align="center">
                    <asp:Label class="labelValue" ID="Label3" runat="server" Text="Upload"></asp:Label>
                </td>
                <td class="style1">
                    <asp:FileUpload class="TextInput" ID="FileUpload" runat="server" />
                </td>
                <td>
                    <asp:Button class="ButtonInput" ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divsize">
    </div>
    </form>
</body>

</html>
