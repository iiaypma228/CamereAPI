using System.Collections.Generic;

namespace Camera.UI.Localize;

public class NotificationLocalize
{
    public static Dictionary<Joint.Data.Constants.NotificationType, string> NotificationType
    {
        get
        {
            return new Dictionary<Joint.Data.Constants.NotificationType, string>()
            {
                {Joint.Data.Constants.NotificationType.Email, "email" },
                {Joint.Data.Constants.NotificationType.SMS, "sms"},
                {Joint.Data.Constants.NotificationType.Telegram, "telegram"}
            };
        }
    }
}