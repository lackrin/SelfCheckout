using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Model;

namespace SelfCheckout.Kiosk.Controller
{
    public static class ConsoleHelper
    {
        public static T SelectFrom<T>(IEnumerable<T> list, string prompt = null)
        {
            T[] items = list.ToArray();

            if (!items.Any())
            {
                Console.WriteLine("Nothing to List");
                return items.FirstOrDefault();
            }
            while (true)
            {
                Console.WriteLine(prompt ?? $"Select a {typeof(T).Name}...");

                for (int i = 1; i <= items.Length; i++)
                {
                    Console.WriteLine($"\t[{i}] {items[i - 1]}");
                }

                Console.Write("Selection:");

                string input = Console.ReadLine();

                int selection;

                if (int.TryParse(input, out selection) && (selection <= items.Length))
                    return items[selection - 1];

                Console.WriteLine($"Selection {input} is not valid.");
            }
        }

        public static Sale SelectFrom(IEnumerable<Sale> list, string prompt = null)
        {
            var items = list.ToArray();

            if (!items.Any())
            {
                Console.WriteLine("Nothing to List");
                return items.FirstOrDefault();
            }
            while (true)
            {
                Console.WriteLine(prompt ?? $"Select a {typeof(Sale).Name}...");

                for (int i = 1; i <= items.Length; i++)
                {
                    Console.WriteLine($"\t[{i}] {items[i - 1].ItemName} \t {items[i - 1].SaleType}\t {items[i - 1].NumRequired}\t {items[i - 1].SalePrice} \t {items[i - 1].ItemDiscount}");
                }

                Console.Write("Selection:");

                string input = Console.ReadLine();

                int selection;

                if (int.TryParse(input, out selection) && (selection <= items.Length))
                    return items[selection - 1];

                Console.WriteLine($"Selection {input} is not valid.");
            }
        }

        public static void ListFrom<T>(IEnumerable<T> list)
        {
            T[] items = list.ToArray();

            if (!items.Any())
                Console.WriteLine("Nothing to list");


            for (int i = 1; i <= items.Length; i++)
                {
                    Console.WriteLine($"\t[{i}] {items[i - 1]}");
                }

        }

        public static void ListItem(IEnumerable<Item> list)
        {
            List<Item> items = list.ToList();
            
            if (!items.Any())
                Console.WriteLine("Nothing to list");


            for (int i = 1; i <= items.Count; i++)
            {
                Console.WriteLine($"\t[{i}] {items[i - 1].Name}\t {items[i - 1].Price}");
            }

        }

        public static void ListSale(IEnumerable<Sale> list)
        {
            List<Sale> sales = list.ToList();

            if (!sales.Any())
                Console.WriteLine("Nothing to list");


            for (int i = 1; i <= sales.Count; i++)
            {
                Console.WriteLine($"\t[{i}] {sales[i - 1].ItemName} \t {sales[i - 1].SaleType}\t {sales[i - 1].NumRequired}\t {sales[i - 1].SalePrice} \t {sales[i - 1].ItemDiscount}");
            }

        }


        public static int GetInt(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                string input = Console.ReadLine();

                int result;

                if (int.TryParse(input, out result))
                    return result;

                Console.WriteLine($"{input} is not valid.");
            }
        }

        public static decimal GetDecimal(string prompt = null)
        {
            Console.Write(prompt);

            while (true)
            {
                string input = Console.ReadLine();

                decimal result;

                if (decimal.TryParse(input, out result))
                    return result;

                Console.WriteLine($"{input} is not valid.");
            }
        }

        public static double GetDouble(string prompt = null)
        {
            Console.Write(prompt);

            while (true)
            {
                string input = Console.ReadLine();

                double result;

                if (double.TryParse(input, out result))
                    return result;

                Console.WriteLine($"{input} is not valid.");
            }
        }
    }
}
