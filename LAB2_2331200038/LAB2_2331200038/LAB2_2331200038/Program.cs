using System;
using System.Collections.Generic;
using LAB2_2331200038;

public class Program
{
    public static void Main(string[] args)
    {
        // EXERCISE 1: Encapsulation - Book Class 
        // Testing property accessors and data validation rules
        Book book1 = new Book { ISBN = "ISBN001", Title = "HEHE", Author = "HUHU", Year = 2005, CopiesAvailable = 1 };
        Book book2 = new Book { ISBN = "ISBN002", Title = "HIHI", Author = "HAHA", Year = 2026, CopiesAvailable = 5 };
        Console.WriteLine("Exercise 1: Encapsulation Testing");
        book1.DisplayInfo();

        // EXERCISE 2: Inheritance - Member and PremiumMember Classes 
        // Demonstrating class hierarchy and extended properties
        Member member1 = new Member { MemberID = "M001", Name = "Hehe", Email = "hehe@gmail.com" };
        PremiumMember member2 = new PremiumMember
        {
            MemberID = "M002",
            Name = "Hehe Premium",
            Email = "hehePremium@gmail.com",
            MembershipExpiry = DateTime.Now.AddYears(15),
            MaxBooksAllowed = 5
        };
        Console.WriteLine("\nExercise 2: Inheritance Testing");
        member1.DisplayInfo();
        member2.DisplayInfo();

        // EXERCISE 6: Constructors - Library Class 
        // Initializing the system container using parameterized constructors
        Library library = new Library("Hehe Library");
        library.AddBook(book1);
        library.AddBook(book2);
        library.AddMember(member1);
        library.AddMember(member2);
        Console.WriteLine("\nExercise 6: Library Initialization Status: " + library.LibraryName);

        // EXERCISE 3 & 4: Abstraction & Polymorphism - Transactions 
        // Executing transactions through a shared interface via TransactionHandler
        Console.WriteLine("\nExercise 3 & 4: Abstraction and Polymorphism Testing");
        TransactionHandler transactionHandler = new TransactionHandler();
        transactionHandler.HandleTransactions(library);

        // EXERCISE 5: Interfaces - IPrintable and IMemberActions 
        // Testing contract-based behaviors implemented in various classes
        Console.WriteLine("\nExercise 5: Interface Implementation Testing");
        book1.PrintDetails();
        member1.PrintDetails();
        member1.BorrowBook(book2);

        // EXERCISE 7: Overloading and Overriding - NotificationService Class 
        // Demonstrating static (Overloading) and dynamic (Overriding) polymorphism
        Console.WriteLine("\nExercise 7: Method Overloading and Overriding");
        NotificationService basicService = new NotificationService();
        AdvancedNotificationService advancedService = new AdvancedNotificationService();

        basicService.SendNotification("Cin Chàooo!");
        basicService.SendNotification("Het han ruii brooo", "Hehe");
        advancedService.SendNotification("Đang cap nhat..."); 

        // EXERCISE 8: Properties with Access Modifiers - LibraryCard Class 
        // Protecting internal state using restricted access modifiers
        Console.WriteLine("\nExercise 8: Access Modifiers Testing");
        LibraryCard card1 = new LibraryCard("C001", member1);
        card1.PrintCardDetails();
        card1.RenewCard();

        // EXERCISE 9: Difference Between Class and Record - BookClass vs BookRecord 
        // Visualizing Reference Equality (Class) vs Value Equality (Record)
        Console.WriteLine("\nExercise 9: Class vs Record Behavior Comparison");
        BookClass bc1 = new BookClass("ISBN123", "Hihi", "Hehe");
        BookClass bc2 = new BookClass("ISBN123", "Hihi", "Hehe"); 
        BookRecord br1 = new BookRecord("ISBN123", "Huhu", "Kkkkk");
        BookRecord br2 = new BookRecord("ISBN123", "Huhu", "Kkkkk"); 

        Console.WriteLine($"Class Equality : {bc1 == bc2}");
        Console.WriteLine($"Record Equality : {br1 == br2}"); 
        var br3 = br1 with { Title = "Hihi" }; 
        Console.WriteLine($"Record Copy using 'with' expression: {br3}");

        // EXERCISE 10: Delegates and Events - Library and NotificationService 
        // Setting up the observer pattern with multi-cast delegates
        Console.WriteLine("\nExercise 10: Delegates and Events Implementation");
        LibraryAndNotificationService notifyHandler = new LibraryAndNotificationService();
        library.OnBookBorrowed += notifyHandler.SendBorrowNotification;
        library.OnBookBorrowed += notifyHandler.LogBorrowActivity;
    }
}