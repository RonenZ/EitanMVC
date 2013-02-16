using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class News : MainModel, IBasicWithImageModel, IWithSEO
    {
        public string MainImage { get; set; }
        public string Link_External { get; set; }

        public int? SeoId { get; set; }
        public virtual SEO SEO { get; set; }
    }
}
