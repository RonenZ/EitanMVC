using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eitan.Models
{
    public class Page : BasicModel
    {
        public string Content { get; set; }
        public int Type { get; set; }

        public int SeoId { get; set; }
        public virtual SEO Seo { get; set; }

        public virtual ICollection<Picture> Images { get; set; }
    }
}
