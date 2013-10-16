using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eitan.Web.Areas.Admin.Models
{
    public class ProjectPriority : Controller
    {
        public int ID { get; set; }
        public int Priority { get; set; }
    }
}
