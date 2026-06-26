using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public class NotificationService
    {
        public virtual void SendNotification(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }

        public void SendNotification(string message, string recipient)
        {
            Console.WriteLine($"Notification to {recipient}: {message}");
        }

        public void SendNotification(string message, List<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                Console.WriteLine($"Notification to {recipient}: {message}");
            }
        }
    }

    public class AdvancedNotificationService : NotificationService
    {
        public override void SendNotification(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] Advanced Notification: {message}");
        }
    }

}
