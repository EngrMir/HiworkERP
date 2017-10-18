<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetPassword.aspx.cs" Inherits="HiWork.BLL.Email.SetPassword" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GBS Extranet</title>
    <link href="../Content/css/PasswordSetReset.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server" style="height: 100%; width: 100%">
    <div id="headerContainer" class="headerContent">
        <label class="lblProjectName">
            GBS Extranet</label>
        <asp:Image ID="logoImg" ImageUrl="../Content/images/logo_gbs.png" runat="server"/>
    </div>
    <div id="container">
        <table class="content">
            <tr>
                <td colspan="2">
                    <label id="lblTitle" runat="server" class="titleLabel">
                        Set password and activate user account</label>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td colspan="2" />
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        To make your password more secure, use a combination of upper and lower case letters,
                        numbers and special characters such as @, % and !.
                    </div>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td colspan="2" />
            </tr>
            <tr>
                <td class="leftColumn">
                    <label>
                        New password</label>
                </td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                    <label id="lblNewPasswordMsg" runat="server" class="errorMsg" />
                </td>
            </tr>
            <tr style="height: 8px;">
                <td colspan="2" />
            </tr>
            <tr>
                <td class="leftColumn">
                    <label>
                        Confirm password</label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                    <label id="lblConfirmPasswordMsg" runat="server" class="errorMsg" />
                </td>
            </tr>
            <tr style="height: 8px;">
                <td colspan="2" />
            </tr>
            <tr>
                <td class="leftColumn">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSetPassword" runat="server" Text="Set password" CssClass="btnStyle" 
                        OnClick="btnSetPassword_Click" />
                </td>
            </tr>
        </table>
    </div>
    <!-- Footer !-->
    <div id="footerContainer" class="footerContent">
        <label class="copyright">
            &copy; Hiwork centrak system</label>
    </div>
    </form>
</body>
</html>