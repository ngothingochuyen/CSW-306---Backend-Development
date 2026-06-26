using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace LAB2_2331200038
{
    public class Member : IPrintable, IMemberActions
    {
        public string MemberID { get; set; }
        public string Name {  get; set; }
        public string Email { get; set; }

        
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Member Id: {MemberID}, Name: {Name}, Email: {Email}");
        }

        public virtual void BorrowBook(Book book)
        {
            if (book.CopiesAvailable > 0)
            {
                book.CopiesAvailable--;
                Console.WriteLine($"{Name} borrowed '{book.Title}'.");
            }
            else
            {
                Console.WriteLine($"No copies available for '{book.Title}'.");
            }
        }

        public virtual void ReturnBook(Book book)
        {
            book.CopiesAvailable++;
            Console.WriteLine($"{Name} returned '{book.Title}'.");
        }

        public virtual void PrintDetails()
        {
            DisplayInfo();
        }

    }
    public class PremiumMember : Member
    {
        public DateTime MembershipExpiry { get; set; }
        public int MaxBooksAllowed { get; set; }

        public override void BorrowBook(Book book)
        {
            if (book.CopiesAvailable > 0 && (MaxBooksAllowed > 0))
            {
                book.CopiesAvailable--;
                MaxBooksAllowed--;
                Console.WriteLine($"{Name} (Premium) borrowed '{book.Title}'.");
            }
            else
            {
                Console.WriteLine($"Cannot borrow '{book.Title}'.");
            }
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Membership Expiry: {MembershipExpiry.ToShortDateString()}, Max Books Allowed: {MaxBooksAllowed}");

        }
        public override void PrintDetails()
        {
            base.PrintDetails();
            Console.WriteLine($"Membership Expiry: {MembershipExpiry.ToShortDateString()}, Max Books Allowed: {MaxBooksAllowed}");
        }
    }
}
