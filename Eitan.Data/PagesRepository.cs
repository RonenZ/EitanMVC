using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Eitan.Data
{
    public class PagesRepository : GenericRepository<Page>
    {
        public PagesRepository(DbContext context) : base(context) { }

        public Page GetByType(int type)
        {
            if (DbSet == null) DbSet = DbContext.Set<Page>();

            return DbSet.Include("Images").FirstOrDefault(f => f.Type == type);
        }
    }
}
