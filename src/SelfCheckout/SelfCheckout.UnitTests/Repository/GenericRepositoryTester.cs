using System;
using System.IO;
using SelfCheckout.Model;
using SelfCheckout.Repository;
using NUnit.Framework;

namespace SelfCheckout.UnitTests.Repository
{
    [TestFixture]
    public class GenericRepositoryTester
    {
        private IRepository _repository = new GenericRepository();

        [SetUp]
        public void SetUp()
        {
            _repository = new GenericRepository();
        }

        [Test]
        public void create_repository_pass()
        {
            _repository = new GenericRepository("C:/StoreFolder");
            Assert.Pass();
        }

        [Test]
        public void attempting_to_create_repository_in_inaccessible_folder_throws_DirectoryNotFoundException()
        {
            Assert.Throws<DirectoryNotFoundException>(() => _repository = new GenericRepository("N:/StoreFolder"));
        }

        [Test]
        public void attempting_to_create_null_throws_argument_exception()
        {
            _repository = new GenericRepository();
            Assert.Throws<ArgumentException>(() => _repository.Create((Item)null));
        }
    }
}
