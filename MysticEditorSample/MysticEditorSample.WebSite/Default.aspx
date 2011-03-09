<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MysticEditorSample.WebSite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <br />
        <br />
        <br />
        <div align="center">
            <br />
            <br />
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField HeaderText="ID" DataField="Contentid"></asp:BoundField>
                    <asp:BoundField HeaderText="TITOLO" DataField="Title">
                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Bottom" />
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="ContentId" DataNavigateUrlFormatString="~/Editor/FX/GetXml.ashx?contentID={0}"
                        Text="Edita" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div align="right" style="width: 839px">
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Crea Nuovo" Height="25px" Width="100px"
            PostBackUrl="~/Import.aspx" Style="margin-left: 0px; margin-right: 0px" />
    </div>
    </form>
</body>
</html>
