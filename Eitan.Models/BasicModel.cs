using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public interface IBasicModel
    {
        int ID { get; set; }
        DateTime Date_Creation { get; set; }
        string Title { get; set; }
        bool isDeleted { get; set; }
    }

    public interface IBasicWithImageModel : IBasicModel
    {
        string MainImage { get; set; }
    }

    public interface IBasicDetailed : IBasicModel
    {
        string Content { get; set; }
    }

    public class BasicModel : IBasicModel
    {
        public int ID { get; set; }
        public DateTime Date_Creation { get; set; }
        public string Title { get; set; }
        public bool isDeleted { get; set; }
    }

    public class MainModel : BasicModel, IBasicDetailed
    {
        public string Content { get; set; }
        public bool isOnHomePage { get; set; }
    }

    public interface IWithSEO
    {
        int? SeoId { get; set; }
        SEO SEO { get; set; }
    }
}
