using Eitan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Eitan.Web.Models;
using Eitan.Models;
using System.Web;
using System.Web.Helpers;

namespace Eitan.Web.Areas.Admin.Controllers
{
    public class EitanBaseController : Controller
    {
        protected IEitanUow Uow;

        protected bool InsertImage(IBasicWithImageModel Entity, string ImageUploadName, string ModelName)
        {
            try 
	        {	        
		        var image = WebImage.GetImageFromRequest(ImageUploadName);

                if (image != null)
                    Entity.MainImage = Server.MapPath(string.Format("/Images/{0}/", ModelName)).SaveImage(image);
	        }
	        catch (Exception)
	        {
		        return false;
	        }
            return true;
        }

        protected bool UpsertSEO(IWithSEO Entity, int SEOID, SEO SEO, HttpPostedFileBase[] SEOfile, string ModelName)
        {
            try{

            if (SEOID > 0)
            {
                var SEOEntity = Uow.SEORepository.GetByID(SEOID);
                UpdateModel(SEOEntity);
            }
            else if (SEO != null)
            {
                Uow.SEORepository.Add(SEO);
                Uow.Commit();
                Entity.SeoId = SEO.SEOID;
            }


            if (SEOfile != null && SEOfile[0] != null)
                Entity.SEO.ogImage += SEOfile.FBSaveImages(Server.MapPath("/Images/SEO/"), ModelName);
            }
            catch{
                return false;
            }
            return true;
        }
    }
}
