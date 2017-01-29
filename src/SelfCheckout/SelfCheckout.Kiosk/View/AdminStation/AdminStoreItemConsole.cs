using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.View.AdminStation
{
    public class AdminStoreItemConsole
    {
        private readonly IRepository _repository;

        public AdminStoreItemConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool end = false;

            while (!end)
            {
                Console.WriteLine("Select an action...");
                Console.Write("[L]ist Inventory | [I]mport Inventory | Go [B]ack:");

                string action = Console.ReadLine()?.ToLowerInvariant();

                switch (action)
                {
                    case "l":
                        ListItems();
                        break;

                    case "i":
                        ImportItems();
                        break;

                    case "b":
                        end = true;
                        break;
                }
            }
        }

        public void ListItems()
        {
            IEnumerable<Item> items = _repository.GetAll<Item>().ToList();

            ConsoleHelper.ListItem(items);
        }

        public void ImportItems()
        {
            var itemManager = new ItemManager(_repository);
            IEnumerable<Item> items = itemManager.ImportInventroyList();

            ConsoleHelper.ListItem(items);
        }

    }
}
