using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HiWork.BLL.Email
{
    public partial class SetPassword : System.Web.UI.Page
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(SetPassword));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0 && Request.QueryString["Reset"] != null)
            {
                lblTitle.InnerText = "Set password";
            }
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            if (!validateForm()) return;

            //log.Debug("QueryString count:" + Request.QueryString.Count);
            //log.Debug("User ID:" + Request.Params["UserId"]);

            if (Request.QueryString.Count > 0 && Request.QueryString[0].ToString() != string.Empty)
            {
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                    return;

                var userId = Request.QueryString[0].ToString();

                //UserManagementDAO oDAO = new UserManagementDAO();
                //UserData oUser = oDAO.GetUserById(userId);

                //if (oUser != null)
                //{
                //    oUser.Password = Tools.MD5(txtNewPassword.Text);
                //    oUser.IsActive = true;

                //    oDAO.SetUserPassword(oUser);

                //    Response.Redirect("Confirmation.aspx");
                //}
                Response.Redirect("Confirmation.aspx");
            }
            else
            {
                //log.Debug("Password set failed");
                Response.Redirect("ErrorInPasswordSet.aspx");
            }

        }

        public bool validateForm()
        {
            int errorCount = 0;
            lblNewPasswordMsg.InnerText = string.Empty;
            lblConfirmPasswordMsg.InnerText = string.Empty;

            if (txtNewPassword.Text == string.Empty)
            {
                lblNewPasswordMsg.InnerText = "This field is required!!!";
                errorCount++;
            }
            if (txtConfirmPassword.Text == string.Empty)
            {
                lblConfirmPasswordMsg.InnerText = "This field is required!!!";
                errorCount++;
            }

            if (errorCount > 0)
            {
                return false;
            }

            if (txtNewPassword.Text.Length < 4)
            {
                lblNewPasswordMsg.InnerText = "Password must be greater than 4 character!!!";
                return false;
            }


            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblConfirmPasswordMsg.InnerText = "Passwords are not equal!!!";
                return false;
            }

            return true;
        }
    }
}