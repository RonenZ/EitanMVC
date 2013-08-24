using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eitan.Admin.Models
{
    public class FacebookMessage
    {
        public string Message { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
    }
}