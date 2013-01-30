using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eitan.Models;

namespace Eitan.Data
{
    public class SongRepository : GenericRepository<Song>
    {
        public SongRepository(DbContext context) : base(context, "Songs") { }

        IQueryable<Genre> GetAllGenres()
        {
            return DbContext.Set<Genre>();
        }

        public virtual void AddGenre(Genre Entity)
        {
            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            if (DbEntityEntry.State != EntityState.Detached)
                DbEntityEntry.State = EntityState.Added;
            else
                DbContext.Set<Genre>().Add(Entity);
        }
    }
}
