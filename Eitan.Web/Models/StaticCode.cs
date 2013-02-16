using Eitan.Data;
using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Eitan.Web.Models
{
    public static class StaticCode
    {
        public static Dictionary<int, string> ReleaseTypes = new Dictionary<int, string>()
        {
            {0, "Artist album"}, {1, "Artist album"}, {2, "Artist Ep"}, {3, "My Album"}, {4, "My EP"}
        };

        public static ViewModelBase ToViewModelBase<T>(this IQueryable<T> Entities) where T : class, IBasicModel
        {
            return Entities.Select(s => new ViewModelBase() { ID = s.ID, Title = s.Title, CreationDate = s.Date_Creation}).FirstOrDefault();
        }

        public static ViewModelWithImage ToViewModelWithImage<T>(this IQueryable<T> Entities) where T : class, IBasicWithImageModel
        {
            return Entities.Select(s => new ViewModelWithImage()
            {

                ID = s.ID,
                Title = s.Title,
                CreationDate = s.Date_Creation,
                ImagePath = s.MainImage
            }).FirstOrDefault();
        }

        public static ViewModelWithImage ReleaseToViewModelWithImage(this IQueryable<Release> Entities)
        {
            var result = Entities.Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                CreationDate = s.Date_Creation,
                ImagePath = s.RectImage,
                TypeID = s.Type,
                SubTitle = s.Label.Title
            }).FirstOrDefault();

            result.Type = ReleaseTypes[result.TypeID];

            return result;
        }

        public static IEnumerable<ViewModelWithImage> ToViewModelsWithImage<T>(this IQueryable<T> Entities) where T : class, IBasicWithImageModel
        {
            return Entities.Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                CreationDate = s.Date_Creation,
                ImagePath = s.MainImage
            });
        }

        public static SEO NullToSEO(this object Obj)
        {
            if (Obj == null)
                return new SEO();

            return (SEO)Obj;
        }

        public static IEnumerable<ViewModelWithImage> ReleasesToViewModelsWithImage(this IQueryable<Release> Entities)
        {
            return Entities.ToList().Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                CreationDate = s.Date_Creation,
                ImagePath = s.MainImage,
                Type = ReleaseTypes[s.Type],
                TypeID = s.Type,
                SubTitle = s.Label == null ? string.Empty : s.Label.Title
            });
        }


        public static IEnumerable<ViewModelWithImage> ProjectsToViewModelsWithImage(this IQueryable<Project> Entities)
        {
            return Entities.ToList().Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                CreationDate = s.Date_Creation,
                ImagePath = s.MainImage,
                Type = s.Type == null ? string.Empty : s.Type.Title,
                SubTitle = "Client" + s.ClientID.ToString()
            });
        }



        public static IEnumerable<ViewModelDetailed> ToViewModelsImageDetail<T>(this IQueryable<T> Entities) where T : class, IBasicDetailed, IBasicWithImageModel
        {
            var result = Entities.Select(s => new ViewModelDetailed() { ID = s.ID, Title = s.Title, CreationDate = s.Date_Creation, 
                ImagePath = s.MainImage, Content = s.Content }).ToList();

            foreach (var item in result)
                item.Content = item.Content.The_Excerpt(100);

            return result;
        }

        public static ViewModelDetailed ToViewModelImageDetail<T>(this T Entity) where T : class, IBasicDetailed, IBasicWithImageModel
        {
            var result = new ViewModelDetailed() { ID = Entity.ID, Title = Entity.Title, CreationDate = Entity.Date_Creation, 
                ImagePath = Entity.MainImage, Content = Entity.Content.The_Excerpt(100) };
            return result;
        }

        public static string The_Excerpt(this string Value, int charNumber, string link = "")
        {
            if (string.IsNullOrEmpty(Value))
                return string.Empty;

            string result = (Value.Length > charNumber) ? Value.Substring(0, charNumber - 1)  : Value;

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

        public static bool SendContact(this ContactModel Entity)
        {
            var subject = string.Format("New Contact Us By {0}", Entity.Name);
            var body = string.Format("<table><tr><td colspan='2'>Contact:</td></tr><tr><td>Name: </td><td>{0}</td></tr><tr><tr><td>Email: </td><td><a href='mailto://{1}'>{1}</a></td></tr><tr><td>Content: </td><td>{2}</td></tr></table>", Entity.Name, Entity.Email, Entity.Message);

            string sender = ConfigurationManager.AppSettings["ContactUsSender"];
            string[] recipients = ConfigurationManager.AppSettings["ContactUsRecipients"].Split(',');

            return SendMail(sender, subject, body, recipients);
        }

        public static bool SendMail(string sender, string subject, string body, params string[] mails)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(sender);

                foreach (var mail in mails)
                {
                    message.To.Add(new MailAddress(mail));
                }

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
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
    }
}