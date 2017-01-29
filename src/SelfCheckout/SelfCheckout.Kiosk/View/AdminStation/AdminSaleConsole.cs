using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.View.AdminStation
{
    public class AdminSaleConsole
    {
        private readonly IRepository _repository;

        public AdminSaleConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool end = false;

            while (!end)
            {
                Console.WriteLine("Select an action...");
                Console.Write("[A]dd a new Sale | [L]ist Sales | [D]elete a Sale | Go [B]ack:");

                string action = Console.ReadLine()?.ToLowerInvariant();

                switch (action)
                {
                    case "a":
                        AddSaleConsole addSaleConsole = new AddSaleConsole(_repository);
                        addSaleConsole.Run();
                        break;

                    case "d":
                        EndSale();
                        break;

                    case "l":
                        ListSales();
                        break;

                    case "b":
                        end = true;
                        break;
                }
            }
        }

        public void EndSale()
        {
            IEnumerable<Sale> sales = _repository.GetAll<Sale>().ToList();

            if (sales.Any())
            {
                Sale selectedItem = ConsoleHelper.SelectFrom(sales, "Select the Sale to end:");

                Sale toRemove = _repository.GetAll<Sale>().SingleOrDefault(p => p.Id == selectedItem.Id);

                if (toRemove != null)
                {
                    SaleFactory.RemoveSale(toRemove, _repository);
                    Console.WriteLine($"Sale {selectedItem.Id} has been ended.");
                }
            }
            else
            {
                Console.WriteLine("No Sales to Delete");
            }

        }

        public void ListSales()
        {
            IEnumerable<Sale> sales = _repository.GetAll<Sale>().ToList();

            ConsoleHelper.ListSale(sales);
        }
    }
}
