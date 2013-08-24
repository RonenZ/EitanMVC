using Eitan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Http;
using Eitan.WebApi.Controllers;

namespace Eitan.WebApi.Controllers
{
    public class EitanApiBaseController : ApiController
    {
        protected IEitanUow Uow;
    }

}
