using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    internal class LibraryAndNotificationService
    {
        public void SendBorrowNotification(Book book, Member member)
        {
            Console.WriteLine($"Notification: {member.Name} has borrowed '{book.Title}'.");
        }

        public void LogBorrowActivity(Book book, Member member)
        {
            Console.WriteLine($"Log: {member.Name} borrowed '{book.Title}' on {DateTime.Now}.");
        }
    }
}
