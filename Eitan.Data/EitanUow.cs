using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public class EitanUow : IEitanUow, IDisposable
    {
        protected IRepositoryProvider RepositoryProvider { get; set; }
        private EitanDbContext DbContext { get; set; }

        public EitanUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        private void CreateDbContext()
        {
            DbContext = new EitanDbContext();

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// Saves changes to DB
        /// </summary>
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public PagesRepository PagesRepository { get { return GetRepo<PagesRepository, Page>(); } }
        public IRepository<Artist> ArtistRepository { get { return GetStandardRepo<Artist>("Artist"); } }
        public IRepository<News> NewsRepository { get { return GetStandardRepo<News>("News"); } }
        public ProjectRepository ProjectRepository { get { return GetRepo<ProjectRepository, Project>(); } }
        public ReleaseRepository ReleaseRepository { get { return GetRepo<ReleaseRepository, Release>(); } }
        public SongRepository SongRepository { get { return GetRepo<SongRepository, Song>(); } }

        /// <summary>
        /// returns a standard IRepository of Entity Type
        /// </summary>
        private IRepository<T> GetStandardRepo<T>(string tableName = "") where T : BasicModel
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>(tableName);
        }

        /// <summary>
        /// Returns a unique repository, non standard
        /// </summary>
        private T GetRepo<T, O>() where T : IRepository<O> where O : BasicModel
        {
            return RepositoryProvider.GetRepository<T, O>();
        }

        /// <summary>
        /// Dispose Unit Of Work
        /// </summary>
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion     
    }
}
