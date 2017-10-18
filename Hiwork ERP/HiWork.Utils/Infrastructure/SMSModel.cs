namespace HiWork.Utils.Infrastructure
{
    public class SMSModel
    {
        public string Message { get; set; }
        public string MobileNumber { get; set; }

        public SMSModel()
        {
        }

        public SMSModel(string mobileNumber, string message)
        {
            Message = message;
            MobileNumber = mobileNumber;
        }
    }
}
