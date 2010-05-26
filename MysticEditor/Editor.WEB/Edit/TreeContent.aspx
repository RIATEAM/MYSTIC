<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TreeContent.aspx.cs" Inherits="Editor.Web.Edit.TreeContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body bgcolor="#E6E6E6">
    <form id="form1" runat="server">
    <asp:TreeView ID="TreeView1" runat="server" Height="25px" OnTreeNodeExpanded="TreeView1_TreeNodeExpanded">
        <Nodes>
            <asp:TreeNode ImageUrl="~/Img/root.gif" PopulateOnDemand="false" Text="Contenuti"
                Value=""></asp:TreeNode>
        </Nodes>
    </asp:TreeView>
    </form>
</body>
</html>
