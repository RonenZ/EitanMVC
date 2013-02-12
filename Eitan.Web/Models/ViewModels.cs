using Eitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eitan.Web.Models
{
    public class ViewModelBase
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(int _ID, string _Title, DateTime _Creation, string Type = "", string _SubTitle = "")
        {
            this.ID = _ID;
            this.Title = _Title;
            this.SubTitle = _SubTitle;
            this.CreationDate = _Creation;
            this.Type = Type;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Creation
        {
            get
            {
                return CreationDate.ToString("dd/MM/yyyy");
            }
        }
        public DateTime CreationDate { get; set; }
    }

    public class ViewModelWithImage : ViewModelBase
    {
        public ViewModelWithImage()
        {
        }

        public ViewModelWithImage(int _ID, string _Title, DateTime _Creation, string _ImagePath, string Type = "", int TypeID = 0, string _SubTitle = "")
            : base(_ID, _Title, _Creation, Type,  _SubTitle)
        {
            this.ImagePath = _ImagePath;
            this.TypeID = TypeID;
        }

        public string ImagePath { get; set; }
    }

    public class ViewModelDetailed : ViewModelWithImage
    {
        public ViewModelDetailed()
        {
        }

        public ViewModelDetailed(int _ID, string _Title, string _Content, DateTime _Creation, string _ImagePath, string Type = "", string _SubTitle = "")
            : base(_ID, _Title, _Creation, _ImagePath, Type)
        {
            this.Content = _Content;
            this.SubTitle = _SubTitle;
        }

        public string Content { get; set; }
    }

    public class HomePageViewModel : Dictionary<string, HomeViewModelWithImageWrap> 
    {
        public HomePageViewModel()
        {
            this.Add("Project", new HomeViewModelWithImageWrap() { Controller = "Projects", Type = "Project" });
            this.Add("Discography", new HomeViewModelWithImageWrap() { Controller = "Releases", Type = "Discography" });
            this.Add("News", new HomeViewModelWithImageWrap() { Controller = "News", Type = "News" });
            this.Add("Movie", new HomeViewModelWithImageWrap() { Controller = "Movies", Type = "Movie" });
        }
    }

    public class HomeViewModelWithImageWrap
    {
        public ViewModelWithImage Item { get; set; }
        public string Controller { get; set; }
        public string Type { get; set; }
    }

    public class PagedViewModelsContainer
    {
        public bool isGotMoreItems { get; set; }
        public int ItemsLeft { get; set; }
        public IEnumerable<ViewModelBase> Items { get; set; }
    }
}