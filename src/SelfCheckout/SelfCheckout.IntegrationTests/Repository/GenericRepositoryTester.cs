using System;
using System.IO;
using System.Linq;
using SelfCheckout.Model;
using SelfCheckout.Repository;
using Newtonsoft.Json;
using NUnit.Framework;

namespace SelfCheckout.IntegrationTests.Repository
{
    [TestFixture]
    public class GenericRepositoryTester
    {
        private IRepository _repository;

        private string _repositoryFolderName;

        [SetUp]
        public void SetUp()
        {
            _repositoryFolderName = Path.Combine("C:\\ShoppingList\\",
               "SelfCheckout.IntegrationTests");

            _repository = new GenericRepository(_repositoryFolderName);
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_repositoryFolderName, true);
        }

        [Test]
        public void add_duplicate_entity_throws_InvalidOperationException()
        {
            
            Item item = new Item("Apple", 1.00m);
            _repository.Create(item);

            Assert.Throws<InvalidOperationException>(() => _repository.Create(item));
        }

        [Test]
        public void creating_an_item_and_getting_it_returns_that_item()
        {
            Item newItem = new Item("Apple", 1.50m);
            _repository.Create(newItem);

            Item retrievedItem = _repository.GetAll<Item>().Single(gi => gi.Id == newItem.Id);

            string expected = JsonConvert.SerializeObject(newItem);
            string actual = JsonConvert.SerializeObject(retrievedItem);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void updating_a_nonexistent_entity_itemthrows_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _repository.Update(new Item("foo", decimal.Zero)));
        }

        [Test]
        public void getting_an_updated_item_gives_item_with_updated_values()
        {
            Sale original = new Sale();
            _repository.Create(original);

            original.SaleType = SaleType.AdditionalProduct;
            _repository.Update(original);

            Sale updated = _repository.GetAll<Sale>().Single(p => p.Id == original.Id);

            Assert.AreEqual(original.SaleType, updated.SaleType);
        }

        [Test]
        public void deleted_items_are_removed_from_repository()
        {
            _repository.Create(new Item("Icecream", decimal.MaxValue));

            Item newItem = new Item("Apple", 2.50m);
            _repository.Create(newItem);
            _repository.Delete(newItem);

            Assert.Throws<InvalidOperationException>(() => _repository.GetAll<Item>().Single(gi => gi.Id == newItem.Id));
        }
    }
}
