<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MysticEditorSample.WebSite.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<frameset cols="25%, *" frameborder="yes" border="2">
    <frame id="myAlternativeContent"  scrolling="no" name="myAlternativeContent" src="Left.aspx?idc=<%=Request.QueryString["idc"] %>&iditem=<%=Request.QueryString["iditem"] %>&type=<%=Request.QueryString["type"] %>"
       />
    <frame id="contentFrame" name="contentFrame" scrolling="auto" src="" />
</frameset>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
