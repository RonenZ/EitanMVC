using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitan.Models
{
    public class ReleaseSearchModel
    {
        public ReleaseSearchModel(string _Search, int _Year, int _Type, int _GenreID)
        {
            this.Search = _Search;
            this.Year = _Year;
            this.ReleaseTypeID = _Type;
            this.GenreID = _GenreID;
        }

        public string Search { get; set; }
        public int Year { get; set; }
        public int ReleaseTypeID { get; set; }
        public int GenreID { get; set; }
    }

    public class ProjectSearchModel
    {
        public ProjectSearchModel(string _Search, int _Year, int _Type, int _ClientID)
        {
            this.Search = _Search;
            this.Year = _Year;
            this.ProjectTypeID = _Type;
            this.ClientID = _ClientID;
        }

        public string Search { get; set; }
        public int Year { get; set; }
        public int ProjectTypeID { get; set; }
        public int ClientID { get; set; }
    }

    public class SearchModels
    {
    }
}
