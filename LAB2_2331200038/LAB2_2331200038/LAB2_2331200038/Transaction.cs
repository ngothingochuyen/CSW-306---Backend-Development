using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public abstract class Transaction
    {
        public string TransactionID { get; set; }
        public DateTime TransactionDate { get; set; }
        public Member Member { get; set; }

        public abstract void Execute();
    }

    public class BorrowTransaction : Transaction
    {
        public Book BookBorrowed { get; set; }

        public override void Execute()
        {
            if (BookBorrowed.CopiesAvailable > 0)
            {
                BookBorrowed.CopiesAvailable--;
                Console.WriteLine($"{Member.Name} has borrowed '{BookBorrowed.Title}'.");
            }
            else
            {
                Console.WriteLine($"No copies available for '{BookBorrowed.Title}'.");
            }
        }
    }

    public class ReturnTransaction : Transaction
    {
        public Book BookReturned { get; set; }

        public override void Execute()
        {
            BookReturned.CopiesAvailable++;
            Console.WriteLine($"{Member.Name} has returned '{BookReturned.Title}'.");
        }
    }

}
