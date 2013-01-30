using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data.Helpers
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private RepositoryFactories _repositoryFactories;
        protected Dictionary<Type, object> Repositories { get; private set; }
        public DbContext DbContext { get; set; }

        public RepositoryProvider(RepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }


        public IRepository<T> GetRepositoryForEntityType<T>(string TableName = "") where T : BasicModel
        {
            var result = GetRepository<IRepository<T>, T>(
                _repositoryFactories.GetRepositoryFactoryByEntityType<T>());

            result.setTableName(TableName);
            return result;
        }

        public T GetRepository<T,O>(Func<DbContext, object> factory = null) where T : IRepository<O> where O: BasicModel
        {
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            
            if(repoObj != null) { return (T)repoObj; } //if repository found

            return MakeRepository<T, O>(factory, DbContext); //else create repository
        }

        protected virtual T MakeRepository<T, O>(Func<DbContext, object> factory, DbContext DbContext) where T : IRepository<O> where O: BasicModel
        {
            var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();

            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }
            var repo = (T)f(DbContext);
            Repositories[typeof(T)] = repo;
            return repo;
        }

        public void SetRepository<T>(T repository)
        {
            Repositories[typeof(T)] = repository;
        }
    }
}
