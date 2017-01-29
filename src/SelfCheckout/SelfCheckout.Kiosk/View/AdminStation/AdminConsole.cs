using System;
using System.Collections.Generic;
using System.Linq;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.View.AdminStation
{
    public class AdminConsole
    {
        private readonly IRepository _repository;

        public AdminConsole(IRepository repository)
        {
            _repository = repository;
        }

        public void Run()
        {
            bool end = false;

            while (!end)
            {
                Console.WriteLine("Select an action...");
                Console.Write("[S]ales Options | [I]tem Options | Go [B]ack:");

                string action = Console.ReadLine()?.ToLowerInvariant();

                switch (action)
                {
                    case "s":
                        AdminSaleConsole saleConsole = new AdminSaleConsole(_repository);
                        saleConsole.Run();
                        break;

                    case "i":
                        AdminStoreItemConsole itemConsole = new AdminStoreItemConsole(_repository);
                        itemConsole.Run();
                        break;

                    case "b":
                        end = true;
                        break;
                }
            }
        }
    }
}
