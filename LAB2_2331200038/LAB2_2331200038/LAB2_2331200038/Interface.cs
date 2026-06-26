using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public interface IPrintable
    {
        void PrintDetails();
    }

    public interface IMemberActions
    {
        void BorrowBook(Book book);
        void ReturnBook(Book book);
    }
}
