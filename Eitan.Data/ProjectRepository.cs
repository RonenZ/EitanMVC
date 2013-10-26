using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public class ProjectRepository : GenericRepository<Project>
    {
        public ProjectRepository(DbContext context) : base(context, "Projects") { }

        public IQueryable<ProjectType> GetAllProjectTypes()
        {
            return DbContext.Set<ProjectType>().Where(w => w.isDeleted == false).OrderBy(o => o.Title);
        }

        public IQueryable<Client> GetAllClients()
        {
            return DbContext.Set<Client>().Where(w => w.isDeleted == false).OrderBy(o => o.Title);
        }

        public IQueryable<Project> SearchQuery(ProjectSearchModel query)
        {
            var Entities = this.GetAllDesc("Client");

            if (!string.IsNullOrEmpty(query.Search))
                Entities = Entities.Where(w => w.Title.ToLower().Contains(query.Search.ToLower()));

            if (query.ClientID > 0)
                Entities = Entities.Where(w => w.ClientID == query.ClientID);

            if (query.ProjectTypeID > 0)
                Entities = Entities.Where(w => w.TypeID == query.ProjectTypeID);

            if (query.Year > 0)
                Entities = Entities.Where(w => w.Date_Creation.Year == query.Year);

            return Entities;
        }
    }
}
