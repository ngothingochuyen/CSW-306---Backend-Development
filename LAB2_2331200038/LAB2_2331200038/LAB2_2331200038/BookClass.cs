using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public class BookClass
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public BookClass(string isbn, string title, string author)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
        }
    }

    public record BookRecord(string ISBN, string Title, string Author);
}
