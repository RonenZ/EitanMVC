using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitan.Models
{
    public class Page : BasicModel, IWithSEO
    {
        public string Content { get; set; }
        public int Type { get; set; }

        public int SeoId { get; set; }
        public virtual SEO SEO { get; set; }

        public virtual ICollection<Picture> Images { get; set; }
    }
}
