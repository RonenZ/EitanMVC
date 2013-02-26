using Eitan.Data;
using Eitan.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eitan.Web.App_Start
{
    public static class ResourceLoader
    {
        public static void RegisterData()
        {
            using (EitanDbContext _db = new EitanDbContext())
            {
                var relRepo = new ReleaseRepository(_db);
                //Static Genres Resource - lives in memory not in db...
                StaticCode.StaticGenres = _db.Genres.Where(w => w.isDeleted == false).ToDictionary(d => d.ID, d => d.Title);
                StaticCode.StaticYears = relRepo.GetReleaseYears();
            }
        }
    }
}