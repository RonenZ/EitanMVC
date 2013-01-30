﻿using Eitan.Models;
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
            return DbContext.Set<ProjectType>();
        }
    }
}