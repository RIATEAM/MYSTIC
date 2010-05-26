<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditContent.aspx.cs" Inherits="Editor.Web.Edit.EditContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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
    <div>
        <asp:Table ID="cont" runat="server" Width="100%" Height="100%">
            <asp:TableRow ID="contRiga" runat="server">
                <asp:TableCell ID="tdLbTitle" runat="server">
                    <asp:Label ID="lbTitle" runat="server" Text=""></asp:Label>
                </asp:TableCell>
                <asp:TableCell ID="tdTxtTitle" runat="server">
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <table style="width: 100%;">
            <asp:DataList ID="pages" runat="server">
                <HeaderTemplate>
                    <b>Pagine</b> 
                    <tr>
                        <td>
                            Id
                        </td>
                        <td>
                            Posizione
                        </td>
                        <td>
                            Livello
                        </td>
                        <td>
                            Titolo
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="ID" runat="server" Text='<%# DataBinder.Eval(  Container.DataItem,"Pageid") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(  Container.DataItem,"Position") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(  Container.DataItem,"Level") %>' />
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(  Container.DataItem,"Title") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:DataList>
        </table>
    </div>
    </form>
</body>
</html>
