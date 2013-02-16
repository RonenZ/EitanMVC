using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class Release : MainModel, IBasicWithImageModel, IWithSEO
    {
        public Release()
        {
            Songs = new HashSet<Song>();
        }

        public DateTime Date_Release { get; set; }
        public string MainImage { get; set; }
        public string RectImage { get; set; }
        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }
        public int LabelID { get; set; }
        public virtual Label Label { get; set; }
        public int Type { get; set; }
        public string CategoryNum { get; set; }
        public string Link_External { get; set; }

        public virtual int? SeoId { get; set; }
        public virtual SEO SEO { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
