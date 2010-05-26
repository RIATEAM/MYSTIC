<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Editor.Web._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Importa Da Word/Html</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="fuDoc" runat="server" />
    </div>
    <div>
        Accoda al Content:
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
    </div>
    <div>
        Crea un nuovo Content:
        <asp:CheckBox ID="CheckBox1" runat="server" />
    </div>
    <div>
        <asp:Button ID="btnCompila" runat="server" Text="Compila" OnClick="btnCompila_Click" />
    </div>
    </form>
</body>
</html>
