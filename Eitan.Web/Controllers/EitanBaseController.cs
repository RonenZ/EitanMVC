using Eitan.Data;
using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Eitan.Web.Models;

namespace Eitan.Web.Controllers
{
    public class EitanBaseController : Controller
    {
        protected IEitanUow Uow;

        protected void UpsertSEO(IWithSEO Entity, int SEOID, SEO POSTSEO, HttpPostedFileBase[] SEOfile, string ModelName)
        {

            if (SEOID > 0)
            {
                var SEOEntity = Uow.SEORepository.GetByID(SEOID);
                UpdateModel(SEOEntity);
            }
            else if (POSTSEO != null)
            {
                Uow.SEORepository.Add(POSTSEO);
                Uow.Commit();
                Entity.SeoId = POSTSEO.SEOID;
            }


            if (SEOfile != null && SEOfile[0] != null)
                Entity.SEO.ogImage += SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), ModelName);
        }
    }
}
