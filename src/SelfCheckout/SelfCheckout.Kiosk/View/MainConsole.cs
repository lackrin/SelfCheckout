using System;
using System.Configuration;
using SelfCheckout.Kiosk.Controller;
using SelfCheckout.Kiosk.View.AdminStation;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.View
{
    public class MainConsole
    {
    
           [STAThread] // Required to use dialogs.
        public static void Main(string[] args)
        {
            IRepository repository = new  GenericRepository(ConfigurationManager.AppSettings["RepositoryFolder"]);
           
            while (true)
            {
                string imput = "";
                string adminCode = "ududlrlrab";
                Console.WriteLine("Welcome to GroceryCo Valued Customer");
                Console.WriteLine("Press 'S' followed by Enter to Scan your Shopping List and proceed with checkout...");
                
                imput += Console.ReadLine()?.ToLowerInvariant();
                

                if (imput == "s")
                {
                    Console.WriteLine("Scanning Shopping List...");
                    var checkout = new Checkout(repository);
                    checkout.StartCheckout();
                }
                else if (imput == adminCode)
                {
                    Console.WriteLine("Entering admin mode...");
                    var admin = new AdminConsole(repository);
                    admin.Run();
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }

            }
            
        }
    }
}
