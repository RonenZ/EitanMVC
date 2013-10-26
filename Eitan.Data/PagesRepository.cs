using Eitan.Models;
using Eitan.Web.Models;
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

        public Picture GetPictureByID(int ID)
        {
            var pics = DbContext.Set<Picture>();

            return pics.Find(ID);
        }

        public void DeletePicture(int id)
        {
            var pics = DbContext.Set<Picture>();

            var pic = pics.Find(id);

            pics.Remove(pic);
            
        }

        public Page GetByType(int type)
        {
            if (DbSet == null) DbSet = DbContext.Set<Page>();

            return DbSet.Include("Images").Include("SEO").FirstOrDefault(f => f.Type == type);
        }
    }
}
