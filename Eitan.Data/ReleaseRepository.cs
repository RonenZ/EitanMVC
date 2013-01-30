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
        public ReleaseRepository(DbContext context) : base(context, "Releases") { }

        public IQueryable<Release> GetAllDescWithIncludes()
        {
            return DbContext.Set<Release>().Include("Songs").Include("Label").AsQueryable();
        }

        public IQueryable<Label> GetAllLabels()
        {
            return DbContext.Set<Label>();
        }
    }
}
