using DotNetOpenAuth.AspNet.Clients;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Eitan.Admin.Models
{
    public class FBModel
    {
        public bool isPostFB { get; set; }
        public string FBMSGTitle { get; set; }
    }
    public static class StaticCode
    {
        public static Dictionary<int, string> SetType = new Dictionary<int, string> { { 0, "Choose SetType" }, { 1, "DJ Set" }, { 2, "Live Set" }, { 3, "DJ & Live Set" } };
        public static Dictionary<int, string> EventType = new Dictionary<int, string> { { 0, "Choose Event Type" }, { 1, "Open Air" }, { 2, "Club" }, { 3, "Festival" } };
        public static Dictionary<int, string> UserRole = new Dictionary<int, string> { { 0, "Choose Role" }, { 1, "Administrator" }, { 2, "Content Manager" } };

        public static string The_Excerpt(this string Value, int charNumber, string link = "")
        {
            if (string.IsNullOrEmpty(Value))
                return string.Empty;

            string result = (Value.Length > charNumber) ? Value.Substring(0, charNumber - 1) : Value;

            return result.RemoveHtml();
        }

        public static string RemoveHtml(this string ToReduce)
        {
            return Regex.Replace(ToReduce, @"<[^>]*>", string.Empty);
        }

        public static string GetYoutubeVideoID(this string Value)
        {
            Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            Match youtubeMatch = YoutubeVideoRegex.Match(Value);

            string videoid = string.Empty;

            if (youtubeMatch.Success)
                videoid = youtubeMatch.Groups[1].Value;

            return videoid;
        }

        public static string Make_Linkable(this string Value)
        {
            return Value.Replace(" ", "_");
        }

        public static string Nullable_ToShortDate(this DateTime? Source)
        {
            if (Source.HasValue)
                return Source.Value.ToString("dd/MM/yyyy");

            return string.Empty;
        }

        public static string GetPasswordHashed(this string value)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(value)));
        }

        public static bool CompareStringAndHash(this string value, string hash)
        {
            string hashOfInput = value.GetPasswordHashed();

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
                return true;

            return false;
        }

        public static void CreateDirIfNotExist(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static string SaveUploadedFile(this HttpPostedFileBase File, string fullpath)
        {
            var filename = File.FileName;
            fullpath.CreateDirIfNotExist();
            File.SaveAs(fullpath + filename);

            return filename;
        }

        public static string SaveImage(this string fullpath, WebImage image, bool isThumb = true, int width = 470, int height = 227)
        {
            var filename = image.FileName;
            fullpath.CreateDirIfNotExist();
            image.Save(fullpath + filename);
            if (isThumb)
            {
                image.Resize(width, height);
                image.Save(fullpath + "thumb_" + filename);
            }
            return filename;
        }

        //public static string SaveLinkImage(this string fullpath, string imgpath, string url)
        //{
        //    var request = (HttpWebRequest)WebRequest.Create(imgpath);
        //    var response = (HttpWebResponse)request.GetResponse();
        //    WebImage image = new WebImage(response.GetResponseStream());
        //    var filename = url.Trim().Replace(".", "").Replace(" ", "_").Replace("&", "").Replace("?", "").Replace("@", "").Replace(":", "").Replace("/", "");

        //    image.Save(fullpath + filename, "png");

        //    return filename + ".png";
        //}



        public static string SaveFile(this string fullpath, HttpPostedFileBase file)
        {
            var filename = file.FileName;
            fullpath.CreateDirIfNotExist();
            file.SaveAs(fullpath + filename);

            return filename;
        }


        public static string FBSaveImages(this HttpPostedFileBase[] files, string path, string property)
        {
            var sb = new StringBuilder();
            files = files.Where(w => w != null).ToArray();
            foreach (var file in files)
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                var image = new WebImage(data);
                image.FileName = file.FileName;
                var filename = property + "_" + image.FileName;
                path.CreateDirIfNotExist();
                image.Resize(159, 159);
                image.Save(path + filename);
                sb.Append(filename + ";");
            }

            return sb.ToString();
        }

        //public static SEO NullToSEO(this object Obj)
        //{
        //    if (Obj == null)
        //        return new SEO();

        //    return (SEO)Obj;
        //}

        //public static int Get_Page_Num<T>(this IQueryable<T> Entities, int page)
        //where T : class ,IPaged
        //{
        //    var count = Entities.Count();
        //    var result = Convert.ToInt32((count / page));

        //    if ((count % page) > 0)
        //        result++;

        //    return result;
        //}

        public static bool UpdateOnFacebookPage(this FacebookMessage FBM)
        {
            //var Request = HttpContext.Current.Request;
            //var FBCookie = Request.Cookies["FBInfo"];
            //if (FBCookie == null || FBCookie["AccessToken"] == null || FBCookie["uid"] == null)
            //    return false;

            //string FBAccessToken = FBCookie["AccessToken"].ToString();
            //string FBUid = FBCookie["uid"].ToString();
            //string FBAppId = ConfigurationManager.AppSettings["FBAppId"];
            //string FBAppSecret = ConfigurationManager.AppSettings["FBAppSecret"];
            //string FBPageId = ConfigurationManager.AppSettings["FBPageId"];
            //string FBCode = ConfigurationManager.AppSettings["FBCode"];
            //string SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];

            //var client = new FacebookClient(FBAppId, FBAppSecret);

            //try
            //{
            //    dynamic me = client.("me/accounts");

            //    foreach (var item in me.data)
            //    {
            //        if (item.id != null && item.id == FBPageId && item.access_token != null)
            //            return SendFBMessage(item.access_token, FBM);
            //    }
            //}
            //catch (FacebookOAuthException ex)
            //{
            //    string tempToken = string.Empty;
            //    tempToken = RenewToken(FBCookie["AccessToken"]);
            //    if (string.IsNullOrEmpty(tempToken))
            //    {
            //        FBCookie["AccessToken"] = tempToken;
            //        UpdateOnFacebookPage(FBM);
            //    }
            //}

            return false;
        }

        public static bool SendFBMessage(string Token, FacebookMessage FBM)
        {
            //string FBAppId = ConfigurationManager.AppSettings["FBAppId"];
            //string FBAppSecret = ConfigurationManager.AppSettings["FBAppSecret"];
            //string FBCode = ConfigurationManager.AppSettings["FBCode"];
            //string SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            //string FBPageId = ConfigurationManager.AppSettings["FBPageId"];

            //var fb = new FacebookClient(Token);
            //fb.AppId = FBAppId;
            //fb.AppSecret = FBAppSecret;
            //var args = new Dictionary<string, object>();
            //args["message"] = FBM.Message;
            //args["description"] = string.IsNullOrEmpty(FBM.Description) ? string.Empty : Regex.Replace(FBM.Description, "<.*?>", string.Empty);
            //args["name"] = FBM.Name;
            //args["picture"] = FBM.Picture;
            //args["link"] = FBM.Link;
            //args["req_perms"] = "manage_pages";
            //args["scope"] = "manage_pages";
            //args["access_token"] = Token;

            //var result = fb.Post("/" + FBPageId + "/feed", args);

            return true;
        }

        /// <summary>
        /// Renews the token.. (offline deprecation)
        /// </summary>
        /// <param name="existingToken">The token to renew</param>
        /// <returns>A new token (or the same as existing)</returns>
        public static string RenewToken(string existingToken)
        {
            //var fb = new FacebookClient();
            //dynamic result = fb.Get("oauth/access_token",
            //                        new
            //                        {
            //                            client_id = ConfigurationManager.AppSettings["FBAppId"],
            //                            client_secret = ConfigurationManager.AppSettings["FBAppSecret"],
            //                            grant_type = "fb_exchange_token",
            //                            fb_exchange_token = existingToken
            //                        });

            //return result.access_token;
            return string.Empty;
        }

        public static string CleanStringForURL(string str)
        {
            var result = str.Trim().Replace(" ", "_");
            result = result.Replace(".", "");
            result = result.Replace(",", "");

            if (result.Length > 30)
                result = result.Substring(0, 30);

            return result;
        }

        public static bool SendMail(string subject, string body)
        {
            try
            {
                MailMessage Mail = new MailMessage(new MailAddress(ConfigurationManager.AppSettings["BookingRecipiend"]),
                                    new MailAddress(ConfigurationManager.AppSettings["BookingRecipiend"]));

                Mail.Subject = subject;
                Mail.Body = body;
                Mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Send(Mail);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }


    //Excel Result

    public class ExcelResult : ActionResult
    {

        private Stream excelStream;
        private string excelName = string.Empty;

        public ExcelResult(byte[] excel, string _fileName)
        {
            excelStream = new MemoryStream(excel);
            excelName = _fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition", "attachment; filename=" + excelName);

            byte[] buffer = new byte[4096];

            while (true)
            {
                int read = this.excelStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    break;
                }

                response.OutputStream.Write(buffer, 0, read);
            }

            response.End();

        }
    }
}