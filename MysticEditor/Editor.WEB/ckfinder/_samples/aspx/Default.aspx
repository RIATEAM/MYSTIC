<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Editor._Default"
    ValidateRequest="false" %>

<%@ Register Assembly="SyrinxCkEditor" Namespace="Syrinx.Gui.AspNet" TagPrefix="syx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/ckeditor/ckeditor.js"></script>
	<script type="text/javascript" src="/ckfinder/ckfinder.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <syx:CkEditor runat="server" ID="ed1" />

    </div>
    </form>
</body>
</html>
