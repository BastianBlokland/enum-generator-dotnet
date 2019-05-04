using System;

namespace Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Months:");
            foreach (var month in Enum.GetNames(typeof(Months)))
                Console.WriteLine($"- {month}");

            Console.WriteLine("\nExpensive products:");
            foreach (var productName in Enum.GetNames(typeof(ExpensiveProducts)))
                Console.WriteLine($"- {productName}");
        }
    }
}
