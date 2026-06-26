using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public class TransactionHandler
    {
        public void HandleTransactions(Library library)
        {
            List<Transaction> transactions = new List<Transaction>();

            if (library.Books.Count > 0 && library.Members.Count > 0)
            {
                var book = library.Books[0]; 
                var member = library.Members[0]; 

                BorrowTransaction borrow1 = new BorrowTransaction
                {
                    TransactionID = "T001",
                    TransactionDate = DateTime.Now,
                    Member = member,
                    BookBorrowed = book
                };

                ReturnTransaction return1 = new ReturnTransaction
                {
                    TransactionID = "T002",
                    TransactionDate = DateTime.Now,
                    Member = member,
                    BookReturned = book
                };

                transactions.Add(borrow1);
                transactions.Add(return1);

                foreach (var transaction in transactions)
                {
                    transaction.Execute();
                }
            }
            else
            {
                Console.WriteLine("No books or members available for transactions.");
            }
        }
    }
}