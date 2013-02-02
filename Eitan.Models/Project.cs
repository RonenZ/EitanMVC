using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Client : BasicModel { }

    public class Project : MainModel, IBasicWithImageModel
    {
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }

        public int TypeID { get; set; }
        public virtual ProjectType Type { get; set; }
        public string Link_External { get; set; }
        public string MainImage { get; set; }
        public string VideoPath { get; set; }

        public int SeoId { get; set; }
        public virtual SEO Seo { get; set; }
    }

    public class ProjectType : BasicModel { }
}
