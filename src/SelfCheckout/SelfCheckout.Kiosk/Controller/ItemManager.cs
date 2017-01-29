using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SelfCheckout.Model;
using SelfCheckout.Repository;

namespace SelfCheckout.Kiosk.Controller
{
    public class ItemManager
    {
        private readonly IRepository _repository;

        public ItemManager(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Item> ImportInventroyList()
        {
            _repository.DeleteAll<Item>();
            var invList = GetInventoryList().ToList();
            
            for (int i = 1; i < invList.Count; i= i+2)
            {
                _repository.Create(new Item(invList[i-1].Trim(), Convert.ToDecimal(invList[i])));                
            }
            
            return _repository.GetAll<Item>();
        }

        private IEnumerable<string> GetInventoryList()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog() != DialogResult.OK)
                return Enumerable.Empty<string>();

            string cartContents;

            using (Stream stream = dialog.OpenFile())
            using (StreamReader reader = new StreamReader(stream))
            {
                cartContents = reader.ReadToEnd();
            }

            return cartContents.Split(',');
        }
    }
}
