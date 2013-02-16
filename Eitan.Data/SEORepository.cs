using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public class SEORepository
    {
        #region Members
        protected DbContext DbContext { get; set; }

        protected DbSet<SEO> DbSet { get; set; }
        protected string TableName;
        #endregion

        //Ctor
        public SEORepository(DbContext _DbContext, string _TableName = "")
        {
            this.TableName = _TableName;

            if (_DbContext == null)
                throw new ArgumentNullException("DbContext");

            DbContext = _DbContext;
            DbSet = _DbContext.Set<SEO>();
        }


        public virtual IQueryable<SEO> GetAll(string include = "")
        {
            //return DbContext.Database.SqlQuery<T>("EXEC GenericStoredProc_getAll @TableName", new SqlParameter("@TableName", TableName)).AsQueryable();
            if (string.IsNullOrEmpty(include))
                return DbSet;

            return DbSet.Include(include).AsQueryable();
        }


        public virtual SEO GetByID(int id, params Expression<Func<SEO, object>>[] includes)
        {
            return DbSet.Find(id);
        }


        public virtual void Add(SEO Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<SEO>();

            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            if (DbEntityEntry.State != EntityState.Detached)
                DbEntityEntry.State = EntityState.Added;
            else
                DbSet.Add(Entity);
        }

        public virtual void Update(SEO Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<SEO>();

            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            
            if (DbEntityEntry.State == EntityState.Detached)
                DbSet.Attach(Entity);

            DbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(SEO Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<SEO>();

            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            if (DbEntityEntry.State != EntityState.Deleted)
            {
                DbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(Entity);
                DbSet.Remove(Entity);
            }
        }

        public virtual void Delete(int id)
        {
            if (DbSet == null) DbSet = DbContext.Set<SEO>();

            var entity = GetByID(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        public void setTableName(string tablename)
        {
            this.TableName = tablename;
        }
    }
}
