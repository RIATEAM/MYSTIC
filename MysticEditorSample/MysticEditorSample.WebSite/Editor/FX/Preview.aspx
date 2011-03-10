<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preview.aspx.cs" Inherits="MysticEditorSample.WebSite.Editor_FX.Preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            height: 100%;
            background: #222c33;
            color: #EEEEEE;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        idp:<asp:Label ID="lbPageID" runat="server" Font-Size="14px" ForeColor="#EEEEEE" />
        iditem:<asp:Label ID="lbItemID" runat="server" Font-Size="14px" ForeColor="#EEEEEE" />
        <p>
            <strong>Lorem Ipsum</strong> is simply dummy text of the printing and typesetting
            industry. Lorem Ipsum has been the industry's standard dummy text ever since the
            1500s, when an unknown printer took a galley of type and scrambled it to make a
            type specimen book. It has survived not only five centuries, but also the leap into
            electronic typesetting, remaining essentially unchanged. It was popularised in the
            1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more
            recently with desktop publishing software like Aldus PageMaker including versions
            of Lorem Ipsum.</p>
    </div>
    </form>
</body>
</html>
