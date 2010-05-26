<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewContent.aspx.cs" Inherits="Editor.Web.View.ViewContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="TreeView1" runat="server" Height="25px" 
            ontreenodeexpanded="TreeView1_TreeNodeExpanded">
            <Nodes>
                <asp:TreeNode ImageUrl="~/Img/root.gif" PopulateOnDemand="false" 
                    Text="Contenuti" Value=""></asp:TreeNode>
            </Nodes>
        </asp:TreeView>
    </div>
    </form>
</body>
</html>
