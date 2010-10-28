<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SSO.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style type="text/css">
        #form1
        {
            margin-top: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table align="center">
        <tr>
            <td>
                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" Font-Italic="True" Font-Names="Arial Narrow"
                    Font-Size="Large" ForeColor="#333333" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
