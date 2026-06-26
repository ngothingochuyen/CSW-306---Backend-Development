using System.Diagnostics;

public class Program
{
    public static void Main()
    {
        //Q2
        Console.WriteLine("Hello, World!");

        //Q3
        int a = int.Parse(Console.ReadLine());
        int b = int.Parse(Console.ReadLine());
        Console.WriteLine(AddTwoNumbers(a,b));

        //Q4
        SwapNumbers(a, b);

        //Q5
        double score = double.Parse(Console.ReadLine()); ;
        Console.WriteLine("Input: {0}\nOutput: {1}", score, ClassifyStudent(score));

        //Q6
        int month = int.Parse(Console.ReadLine());
        Console.WriteLine("Input: " + month);
        PrintMonthInfo(month);

        //Q7
        int n = int.Parse(Console.ReadLine());
        SumToN(n);

    }

    //Q3
    static int AddTwoNumbers( int numberA, int numberB)
    {
        int sum = numberA + numberB;
        return sum;
    }

    //Q4
    static void SwapNumbers(int numberA, int numberB)
    {
        //Q8
        /*Print the value of 2 number before swap
          Swap 2 numebers by save 1 number in the temp
          Print the value after swap */
        Console.WriteLine("Before swap: A = {0}, B = {1}", numberA, numberB);
        int temp = numberA;
        numberA = numberB;
        numberB = temp;
        Console.WriteLine("After swap: A = {0}, B = {1}", numberA, numberB);
    }

    //Q5
    static string ClassifyStudent(double averageScore)
    {
        if (averageScore >= 90 && averageScore <= 100)
        {
            return "Excellent";
        }
        else if (averageScore >= 80)
        {
            return "Good";
        }
        else if (averageScore >= 70)
        {
            return "Fair";
        }
        else
        {
            return "Average";
        }
    }

    //Q6
    static void PrintMonthInfo(int month) {
        if (month < 1 || month > 12)
        {
            Console.WriteLine("The month input is invalid.");
            return;
        }
        switch (month)
        {
            case 1:
                Console.WriteLine("January - Have 31 days.");
                break;
            case 2:
                Console.WriteLine("February - Have 28 days.");
                break;
            case 3:
                Console.WriteLine("March - Have 31 days.");
                break;
            case 4:
                Console.WriteLine("April - Have 30 days.");
                break;
            case 5:
                Console.WriteLine("May - Have 31 days.");
                break;
            case 6:
                Console.WriteLine("June - Have 30 days.");
                break;
            case 7:
                Console.WriteLine("July - Have 31 days.");
                break;
            case 8:
                Console.WriteLine("August - Have 31 days.");
                break;
            case 9:
                Console.WriteLine("September - Have 30 days.");
                break;
            case 10:
                Console.WriteLine("October - Have 31 days.");
                break;
            case 11:
                Console.WriteLine("November - Have 30 days.");
                break;
            case 12:
                Console.WriteLine("December - Have 31 days.");
                break;
        }
    }

    //Q7
    static void SumToN(int n)
    {
        if (n <= 0)
        {
            Console.WriteLine("n must be > 0");
            return;
        }

        int sum = 0;
        for (int i = 1; i <= n; i++)
        {
            sum += i;
        }
        Console.WriteLine("Input: " + n);
        Console.WriteLine("Output: The sum from 1 to {0} is {1}", n, sum);
    }
}