using System.Collections.Generic;
using SelfCheckout.Model;


namespace SelfCheckout.Repository
{
    public interface IRepository
    {
        void Create<TEntity>(TEntity entity) where TEntity : Entity;

        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : Entity;

        void Update<TEntity>(TEntity toUpdate) where TEntity : Entity;

        bool Delete<TEntity>(TEntity toRemove) where TEntity : Entity;

        void DeleteAll<TEntity>() where TEntity : Entity;
    }
}
