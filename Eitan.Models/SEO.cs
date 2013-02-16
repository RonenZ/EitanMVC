using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Eitan.Models
{
    public class SEO
    {
        [Key]
        public int SEO_ID { get; set; }
        public string ogTitle { get; set; }
        public string ogImage { get; set; }
        public string ogDescription { get; set; }
        public string metaTitle { get; set; }
        public string metaImage { get; set; }
        public string metaDescription { get; set; }
        public string metaKeywords { get; set; }
        public int Type { get; set; }
        public bool isDeleted { get; set; }
    }
}
