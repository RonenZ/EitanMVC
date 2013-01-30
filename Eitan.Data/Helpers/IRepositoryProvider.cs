using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eitan.Models;

namespace Eitan.Data
{
    public interface IRepositoryProvider
    {
        DbContext DbContext { get; set; }

        IRepository<T> GetRepositoryForEntityType<T>(string tableName) where T : BasicModel;

        T GetRepository<T, O>(Func<DbContext, object> factory = null)
            where T : IRepository<O>
            where O : BasicModel;

        void SetRepository<T>(T repository);
    }
}
