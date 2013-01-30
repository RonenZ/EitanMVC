using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Eitan.Web.Models
{
    public static class StaticCode
    {
        public static Dictionary<int, string> ReleaseTypes = new Dictionary<int, string>()
        {
            {0, "Album"} ,{1, "Various Artists"}, {2, "EP"}
        };
        public static ViewModelBase ToViewModelBase<T>(this IQueryable<T> Entities) where T : class, IBasicModel
        {
            return Entities.Select(s => new ViewModelBase() { ID = s.ID, Title = s.Title, Creation = s.Date_Creation}).FirstOrDefault();
        }

        public static ViewModelWithImage ToViewModelWithImage<T>(this IQueryable<T> Entities) where T : class, IBasicWithImageModel
        {
            return Entities.Select(s => new ViewModelWithImage()
            {

                ID = s.ID,
                Title = s.Title,
                Creation = s.Date_Creation,
                ImagePath = s.MainImage
            }).FirstOrDefault();
        }

        public static ViewModelWithImage ReleaseToViewModelWithImage(this IQueryable<Release> Entities)
        {
            var result = Entities.Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                Creation = s.Date_Creation,
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
                Creation = s.Date_Creation,
                ImagePath = s.MainImage
            });
        }

        public static IEnumerable<ViewModelWithImage> ReleasesToViewModelsWithImage(this IQueryable<Release> Entities)
        {
            return Entities.ToList().Select(s => new ViewModelWithImage()
            {
                ID = s.ID,
                Title = s.Title,
                Creation = s.Date_Creation,
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
                Creation = s.Date_Creation,
                ImagePath = s.MainImage,
                Type = s.Type == null ? string.Empty : s.Type.Title,
                SubTitle = "Client" + s.ClientID.ToString()
            });
        }



        public static IEnumerable<ViewModelDetailed> ToViewModelsImageDetail<T>(this IQueryable<T> Entities) where T : class, IBasicDetailed, IBasicWithImageModel
        {
            var result = Entities.Select(s => new ViewModelDetailed() { ID = s.ID, Title = s.Title, Creation = s.Date_Creation, 
                ImagePath = s.MainImage, Content = s.Content }).ToList();

            foreach (var item in result)
                item.Content = item.Content.The_Excerpt(100);

            return result;
        }

        public static ViewModelDetailed ToViewModelImageDetail<T>(this T Entity) where T : class, IBasicDetailed, IBasicWithImageModel
        {
            var result = new ViewModelDetailed() { ID = Entity.ID, Title = Entity.Title, Creation = Entity.Date_Creation, 
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
    }
}