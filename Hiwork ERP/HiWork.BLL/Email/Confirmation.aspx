<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Confirmation.aspx.cs" Inherits="HiWork.BLL.Email.Confirmation" %>

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
            Hiwork</label>
        <asp:Image ID="logoImg" ImageUrl="../Content/images/logo_gbs.png" runat="server" />
    </div>
    <div id="container">
        <table class="content">
            <tr>
                <td colspan="2">
                    <label class="titleLabel">
                        Password set</label>
                </td>
            </tr>
            <tr style="height: 10px;">
                <td colspan="2" />
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <span>
                            Thank you, your password has been set.<br />
                            Please click <a href="http://hiwork.jp/">here</a> to go the website and login.
                        </span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- Footer !-->
    <div id="footerContainer" class="footerContent">
        <label class="copyright">
            &copy; 2017 Hiwork management System (version 1.0), All rights reserved.</label>
    </div>
    </form>
</body>
</html>

