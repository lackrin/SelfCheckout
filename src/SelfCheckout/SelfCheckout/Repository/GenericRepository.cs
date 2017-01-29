using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SelfCheckout.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SelfCheckout.Repository
{
    public class GenericRepository : IRepository
    {
        private readonly string _directoryPath;

        public GenericRepository()
        {
        }

        public GenericRepository(string directoryPath)
        {
            _directoryPath = directoryPath;
            Directory.CreateDirectory(_directoryPath);
        }

        public void Create<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (entity == null)
                throw new ArgumentException("Model cannot be null", nameof(entity));

            if (ModelExists<TEntity>(entity.Id))
                throw new InvalidOperationException(
                    $"Error: Cannot Create Model {typeof(TEntity).Name} with Id {entity.Id}: Model Already Exists.");

            using (var file = File.AppendText(GetFilePathForType<TEntity>()))
            {
                file.WriteLine(JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented));
            }
        }

        public void DeleteAll<TEntity>() where TEntity : Entity
        {
            if (File.Exists(Path.Combine(_directoryPath, typeof(TEntity).Name + ".json")))
                File.Delete(GetFilePathForType<TEntity>());
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Entity
        {
            if (!File.Exists(Path.Combine(_directoryPath, typeof(TEntity).Name + ".json")))
                return Enumerable.Empty<TEntity>();

            using (var file = File.OpenText(GetFilePathForType<TEntity>()))
            using (var reader = new JsonTextReader(file) {SupportMultipleContent = true, FloatParseHandling = FloatParseHandling.Decimal})
            {
                var entities = new List<TEntity>();

                while (reader.Read())
                {
                    var jModel = (JObject)JToken.ReadFrom(reader);
                    var entity = JsonConvert.DeserializeObject<TEntity>(jModel.ToString());

                    entities.Add(entity);
                }

                return entities;
            }
        }

        public void Update<TEntity>(TEntity update) where TEntity : Entity
        {
            if (!ModelExists<TEntity>(update.Id))
                throw new InvalidOperationException(
                    $"ERROR: Cannont preform Update: Object {typeof(TEntity).Name} with Id {update.Id} does not exist.");

            TEntity updated = Copy(update);

            Delete(update);

            Create(updated);
        }

        public bool Delete<TEntity>(TEntity remove) where TEntity : Entity
        {
            if (remove == null)
                throw new ArgumentException("remove cannot be null", nameof(remove));

            ICollection<TEntity> mList = GetAll<TEntity>().ToList();

            mList.Remove(mList.Single(e => e.Id == remove.Id));

            File.WriteAllText(GetFilePathForType<TEntity>(), string.Empty);

            foreach (TEntity entity in mList)
            {
                Create(entity);
            }

            return true;
        }

        private static TEntity Copy<TEntity>(TEntity source) where TEntity : Entity
        {
            string serialized = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<TEntity>(serialized);
        }

        private string GetFilePathForType<TEntity>() where TEntity : Entity
        {
            return Path.Combine(_directoryPath, typeof(TEntity).Name + ".json");
        }

        private bool ModelExists<TEntity>(Guid id) where TEntity : Entity
        {
            IEnumerable<TEntity> entities = GetAll<TEntity>();

            return entities.SingleOrDefault(e => e.Id == id) != null;
        }

    }
}
