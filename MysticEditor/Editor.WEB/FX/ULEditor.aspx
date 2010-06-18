<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ULEditor.aspx.cs" Inherits="Editor.Web.FX.ULEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="js/flexBridge.js"></script>

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.4.2.min.js"></script>

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
        html, body
        {
            background: #222C33;
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
    <form id="form1" runat="server">
    <input type="Hidden" name="y" value="0" />
    <div align="right">
        <asp:Button ID="Save" runat="server" Text="Salva" OnClick="Save_Click" />
    </div>
    <div id="divsize">
        <table style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Link"></asp:Label>
                </td>
                <td class="style1">
                    <asp:TextBox ID="Link" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Upload"></asp:Label>
                </td>
                <td class="style1">
                    <asp:FileUpload ID="FileUpload" runat="server" />
                </td>
                <td>
                    <asp:Button ID="Upload" runat="server" Text="Upload" OnClick="Upload_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
