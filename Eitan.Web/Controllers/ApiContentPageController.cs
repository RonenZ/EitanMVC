using Eitan.Data;
using Eitan.Models;
using Eitan.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eitan.Web.Controllers
{
    public class ContentPageController : EitanApiBaseController
    {
        public ContentPageController(IEitanUow uow)
        {
            Uow = uow;
        }

        // GET api/apicontentpage
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/apicontentpage/5
        public Page Get(int id)
        {
            return Uow.PagesRepository.GetByID(id);
        }

        // POST api/apicontentpage
        public void Post([FromBody]string value)
        {
        }

        // PUT api/apicontentpage/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/apicontentpage/5
        public void Delete(int id)
        {
        }
    }
}
