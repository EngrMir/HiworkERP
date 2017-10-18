using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiWork.Utils
{
    public class PageRoleManager
    {
        EstimationPageButtonAttributes estimationAttr;
        public PageRoleManager(string AttributePageType, string AttributeType)
        {
            if (AttributePageType == "estimation")
            {
                estimationAttr = new EstimationPageButtonAttributes(AttributeType);
            }
        }
    }

    public class EstimationPageButtonAttributes
    {
        public string Type { get; set; }
        public List<Tuple<string, bool>> attr = new List<Tuple<string, bool>>();

        public EstimationPageButtonAttributes(string value)
        {
            if (value == "Created")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", true));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", true));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", true));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", true));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", true));
            }
            else if (value == "RequestedForApproval")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", true));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", true));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", true));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", true));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", true));
            }
            else if (value == "Approved")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", true));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", true));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", true));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", true));
                attr.Add(new Tuple<string, bool>("BtnDelete", true));
            }
            else if (value == "Confirmed")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", true));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", true));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", true));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", true));
            }
            else if (value == "Ordered")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", false));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", false));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", false));
            }
            else if (value == "OrderLost")
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", false));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", false));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", false));
            }
            else
            {
                Type = value;
                attr.Add(new Tuple<string, bool>("BtnOrderDetails", false));
                attr.Add(new Tuple<string, bool>("BtnTemporaryRegistration", true));
                attr.Add(new Tuple<string, bool>("BtnProjectInitiation", true));
                attr.Add(new Tuple<string, bool>("BtnApprovalRequest", false));
                attr.Add(new Tuple<string, bool>("BtnApproval", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnQuotationRequest", false));
                attr.Add(new Tuple<string, bool>("BtnConfirmationEmail", false));
                attr.Add(new Tuple<string, bool>("BtnDelete", false));
            }
        }
    }

    public class PageAttributes : PageButtonAttributes
    {
        public PageAttributes(string value)
        {
            if(value == "Created")
            {
                ActionType = value;
                BtnOrderDetails = true;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = true;
                BtnApproval = false;
                BtnQuotationEmail = true;
                BtnQuotationRequest = true;
                BtnConfirmationEmail = false;
                BtnDelete = true;
            }
            else if (value == "RequestedForApproval")
            {
                ActionType = value;
                BtnOrderDetails = true;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = true;
                BtnQuotationEmail = true;
                BtnQuotationRequest = true;
                BtnConfirmationEmail = false;
                BtnDelete = true;
            }
            else if (value == "Approved")
            {
                ActionType = value;
                BtnOrderDetails = true;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = false;
                BtnQuotationEmail = true;
                BtnQuotationRequest = true;
                BtnConfirmationEmail = true;
                BtnDelete = true;
            }
            else if (value == "Confirmed")
            {
                ActionType = value;
                BtnOrderDetails = true;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = false;
                BtnQuotationEmail = true;
                BtnQuotationRequest = true;
                BtnConfirmationEmail = false;
                BtnDelete = true;
            }
            else if (value == "Ordered")
            {
                ActionType = value;
                BtnOrderDetails = false;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = false;
                BtnQuotationEmail = false;
                BtnQuotationRequest = false;
                BtnConfirmationEmail = false;
                BtnDelete = false;
            }
            else if (value == "OrderLost")
            {
                ActionType = value;
                BtnOrderDetails = false;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = false;
                BtnQuotationEmail = false;
                BtnQuotationRequest = false;
                BtnConfirmationEmail = false;
                BtnDelete = false;
            }
            else
            {
                ActionType = value;
                BtnOrderDetails = false;
                BtnTemporaryRegistration = true;
                BtnProjectInitiation = true;
                BtnApprovalRequest = false;
                BtnApproval = false;
                BtnQuotationEmail = false;
                BtnQuotationRequest = false;
                BtnConfirmationEmail = false;
                BtnDelete = false;
            }
        }
    }

    public class PageButtonAttributes
    {
        public string ActionType { get; set; }
        public bool BtnOrderDetails { get; set; }
        public bool BtnTemporaryRegistration { get; set; }
        public bool BtnProjectInitiation { get; set; }
        public bool BtnApprovalRequest { get; set; }
        public bool BtnApproval { get; set; }
        public bool BtnQuotationEmail { get; set; }
        public bool BtnQuotationRequest { get; set; }
        public bool BtnConfirmationEmail { get; set; }
        public bool BtnDelete { get; set; }
    }    
}
