﻿using Eitan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Eitan.Admin.Controllers
{
    public class EitanBaseController : Controller
    {
        protected IEitanUow Uow;
    }
}