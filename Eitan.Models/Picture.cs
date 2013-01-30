using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Picture : BasicModel
    {
        public int PageId { get; set; }
        public string Source { get; set; }
        public int PictureType { get; set; }
    }
}
