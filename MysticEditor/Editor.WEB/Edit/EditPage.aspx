<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="Editor.Web.Edit.EditPage" %>

<%@ Register Assembly="SyrinxCkEditor" Namespace="Syrinx.Gui.AspNet" TagPrefix="syx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>

    <script type="text/javascript" src="/ckfinder/ckfinder.js"></script>

    <script language="javascript" type="text/javascript">
        function refreshMenu() {
            window.top.leftFrame.window.location.reload(false);
        }
    </script>

</head>
<body bgcolor="#FFFFC8">
    <form id="form1" runat="server">
    <div align="right">
        <asp:Button ID="Save" runat="server" Text="Salva" OnClick="Save_Click" />
    </div>
    <div align="center">
        <table>
            <tr>
                <td>
                    Titolo Pubblico
                </td>
                <td>
                    <asp:TextBox ID="TitoloAlbero" runat="server"></asp:TextBox>(Visibile nel menu)
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:Table ID="Table" runat="server" Height="100%" Width="100%">
        </asp:Table>
    </div>
    </form>
</body>
</html>
