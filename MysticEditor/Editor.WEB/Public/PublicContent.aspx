﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PublicContent.aspx.cs"
    Inherits="Editor.Web.Public.PublicContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pubblica</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Pubblica il Content:
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
    </div>
    <div>
        <asp:Button ID="Button1" runat="server" Text="Pubblica" 
            onclick="Button1_Click" />
    </div>
    </form>
</body>
</html>
