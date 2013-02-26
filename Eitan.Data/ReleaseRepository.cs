using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public class ReleaseRepository : GenericRepository<Release>
    {
        public ReleaseRepository(DbContext context) : base(context, "Releases") { 
        }


        public IQueryable<Release> GetAllDescWithIncludes()
        {
            return DbContext.Set<Release>().Include("Songs").Include("Label").AsQueryable();
        }

        public IQueryable<Label> GetAllLabels()
        {
            return DbContext.Set<Label>().Where(w => w.isDeleted == false);
        }

        public IQueryable<Genre> GetAllGenres()
        {
            return DbContext.Set<Genre>().Where(w => w.isDeleted == false);
        }

        public Dictionary<int, string> GetReleaseYears()
        {
            var results = DbContext.Database.SqlQuery<int>("SELECT DISTINCT(YEAR(Date_Release)) AS Year FROM Releases");

            return results.ToDictionary(d => d, d => d.ToString());
        }

        public IQueryable<Release> SearchQuery(ReleaseSearchModel query)
        {
            var Entities = this.GetAllDesc("Label");

            if (!string.IsNullOrEmpty(query.Search))
                Entities = Entities.Where(w => w.Title.ToLower().Contains(query.Search.ToLower()));

            if(query.GenreID > 0)
                Entities = Entities.Where(w => w.GenreID == query.GenreID);

            if (query.ReleaseTypeID > 0)
                Entities = Entities.Where(w => w.Type == query.ReleaseTypeID);

            if (query.Year > 0)
                Entities = Entities.Where(w => w.Date_Release.Year == query.Year);

            return Entities;
        }
    }
}
