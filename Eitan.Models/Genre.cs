using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Genre : BasicModel
    {
        public virtual ICollection<Release> News { get; set; }
    }
}
