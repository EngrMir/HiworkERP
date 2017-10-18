using System.Collections.Generic;

namespace HiWork.Utils.Infrastructure
{
    public interface ISMSHandler
    {
        void SendRegistratoinNotification(SMSModel sms);
        KeyValuePair<bool, string> SendSMS(SMSModel sms);
        void SendVoice(SMSModel sms);
        //
    }

}
