using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eitan.Models;
using System.Linq.Expressions;

namespace Eitan.Data
{
    public interface IRepository<T> where T : BasicModel
    {
        void setTableName(string tablename);
        IQueryable<T> GetAll(string include = "");
        IQueryable<T> GetAllDesc(string include = "");
        T GetByID(int id, params Expression<Func<T, object>>[] includes);
        void Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
        void Delete(int id);
    }
}
