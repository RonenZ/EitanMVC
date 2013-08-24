using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eitan.Models;

namespace Eitan.Data.Helpers
{
    public class RepositoryFactories
    {
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;


        private IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
                {
                   {typeof(ProjectRepository), dbContext => new ProjectRepository(dbContext)},
                   {typeof(ReleaseRepository), dbContext => new ReleaseRepository(dbContext)},
                   {typeof(PagesRepository), dbContext => new PagesRepository(dbContext)},
                   {typeof(SongRepository), dbContext => new SongRepository(dbContext)},
                };
        }

        public RepositoryFactories()
        {
            _repositoryFactories = GetFactories();
        }

        public RepositoryFactories(IDictionary<Type, Func<DbContext, object>> factories)
        {
            _repositoryFactories = factories;
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryByEntityType<T>() where T : BasicModel
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryType<T>();
        }

        public virtual Func<DbContext, object> DefaultEntityRepositoryType<T>() where T : BasicModel
        {
            return dbContext => new GenericRepository<T>(dbContext);
        }
    }
}
