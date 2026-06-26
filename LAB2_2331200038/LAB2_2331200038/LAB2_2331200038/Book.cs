using System;
using System.Collections.Generic;
using System.Text;

namespace LAB2_2331200038
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        private int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (value >= 0)
                {
                    year = value;
                }
                else
                {
                    throw new ArgumentException("Year cannot be negative!");
                }
            }
        }
        private int copiesAvailable;
        public int CopiesAvailable
        {
            get { return copiesAvailable; }
            set
            {
                if (value >= 0)
                {
                    copiesAvailable = value;
                }
                else
                {
                    throw new ArgumentException("CopiesAvailable cannot be negative!");
                }
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"ISBN: {ISBN}, Title: {Title}, Author: {Author}, Year: {Year}, Copies Available: {CopiesAvailable}");
        }

        public void PrintDetails()
        {
            DisplayInfo();
        }

    }

}
