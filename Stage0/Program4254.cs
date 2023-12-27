// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome4254();
            Welcome8473();
            Console.ReadKey();
        }
        static partial void Welcome8473();
        private static void Welcome4254()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}
/*Enter your name: shira
shira, welcome to my first console application

C:\Users\shenf\source\repos\dotNet5784_4254_8473\Stage0\bin\Debug\net7.0\Stage0.exe (process 30640) exited with code 0.
Press any key to close this window . . .
*/