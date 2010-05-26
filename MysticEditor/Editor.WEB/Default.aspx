<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Editor.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
            EDITOR</h1>
    </div>
    <div>
        <asp:HyperLink ID="Import" runat="server" NavigateUrl="Import/Default.aspx">Importa da Word/Html</asp:HyperLink>
    </div>
    <br />
    <div>
        <asp:HyperLink ID="Edit" runat="server" NavigateUrl="~/Edit/Index.htm">Edit</asp:HyperLink>
    </div>
    <br />
    <div>
        <asp:HyperLink ID="View" runat="server" 
            NavigateUrl="~/View/ViewContent.aspx">Visualizza</asp:HyperLink>
    </div>
    <br />
    <div>
        <asp:HyperLink ID="Public" runat="server" 
            NavigateUrl="~/Public/PublicContent.aspx">Publica</asp:HyperLink>
    </div>
    <br />
    <div>
        <asp:HyperLink ID="WebOrb" runat="server" 
            NavigateUrl="~/weborbconsole.html">Console di Weborb</asp:HyperLink>
    </div>    
    <br />
    <div>
        <asp:HyperLink ID="HyperLink1" runat="server" 
            NavigateUrl="~/FX/GetXml.ashx">Albero Contenuti</asp:HyperLink>
    </div>
    </form>
</body>
</html>
