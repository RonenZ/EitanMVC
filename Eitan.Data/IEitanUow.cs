using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Data
{
    public interface IEitanUow
    {
        //Save Data
        void Commit();

        //Repositories
        IRepository<Artist> ArtistRepository { get; }
        IRepository<News> NewsRepository { get; }
        PagesRepository PagesRepository { get; }
        ProjectRepository ProjectRepository { get; }
        ReleaseRepository ReleaseRepository { get; }
        SongRepository SongRepository { get; }

        //Dispose
        void Dispose();
    }
}
