using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Song : BasicModel
    {
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
        public string FilePath { get; set; }

        public virtual ICollection<Release> News { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
    }

}
