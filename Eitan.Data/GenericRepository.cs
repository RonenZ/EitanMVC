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
    public class GenericRepository<T> : IRepository<T> where T : BasicModel
    {
        #region Members
        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }
        protected string TableName;
        #endregion

        //Ctor
        public GenericRepository(DbContext _DbContext, string _TableName = "")
        {
            this.TableName = _TableName;

            if (_DbContext == null)
                throw new ArgumentNullException("DbContext");

            DbContext = _DbContext;
            DbSet = _DbContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll(string include = "")
        {
            //return DbContext.Database.SqlQuery<T>("EXEC GenericStoredProc_getAll @TableName", new SqlParameter("@TableName", TableName)).AsQueryable();
            if (string.IsNullOrEmpty(include))
                return DbSet;

            return DbSet.Include(include).AsQueryable();
        }

        public virtual T GetByID(int id, params Expression<Func<T, object>>[] includes)
        {
            if (includes != null && includes.Length > 0)
            {
                return includes.Aggregate(this.GetAll(),
                          (current, include) => current.Include(include)).FirstOrDefault(f => f.ID == id);
            }

            return DbSet.Find(id);

            //return DbContext.Database.SqlQuery<T>("EXEC GenericStoredProc_getById  @TableName, @Id", new SqlParameter("@TableName", TableName), new SqlParameter("@Id", id)).FirstOrDefault();
        }

        public virtual IQueryable<T> SearchStringInTitle(string value)
        {
            return GetAllDesc().Where(w => w.Title.ToLower().Contains(value.ToLower()));
        }

        public virtual void Add(T Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<T>();

            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            if (DbEntityEntry.State != EntityState.Detached)
                DbEntityEntry.State = EntityState.Added;
            else
                DbSet.Add(Entity);
        }

        public virtual IQueryable<T> GetAllDesc(string include = "")
        {
            if (string.IsNullOrEmpty(include))
                return DbSet.OrderByDescending(o => o.Date_Creation);

            return DbSet.Include(include).OrderByDescending(o => o.Date_Creation);
            //return DbContext.Database.SqlQuery<T>("EXEC GenericStoredProc_getAllDESC  @TableName", new SqlParameter("@TableName", TableName)).AsQueryable();
        }

        public virtual void Update(T Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<T>();

            DbEntityEntry DbEntityEntry = DbContext.Entry(Entity);
            
            if (DbEntityEntry.State == EntityState.Detached)
                DbSet.Attach(Entity);

            DbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T Entity)
        {
            if (DbSet == null) DbSet = DbContext.Set<T>();

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
            if (DbSet == null) DbSet = DbContext.Set<T>();

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
