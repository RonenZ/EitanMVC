using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Artist : BasicModel
    {
        public virtual ICollection<Song> Songs { get; set; }
    }
}
