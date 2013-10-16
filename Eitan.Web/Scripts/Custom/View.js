var ServiceUrl = "/api";
var page = 1;

ViewModel = {
    News: ko.observableArray(),
    Releases: ko.observableArray(),
    Projects: ko.observableArray(),
    releaseAdded: function(el) {
        $("#releases-div").isotope('appended', $(el));
    },
    projectAdded: function (el) {
        $("#projects-div").isotope('appended', $(el));
    },
    newsAdded: function (el) {
        $("#news-div").isotope('appended', $(el));
    },
    LoadNews: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();

        if (ViewModel.News().length > 0)
            $("#news-container").fadeIn('slow');
        else {
            GetFromWebApiDetails(ViewModel.News, "News", data, ".btn-more-News");
        }
        $("#news-container").show();
    },

    SearchReleases: function (Type, Genre, Year, Search) {
        Type = ValidateNumber(Type);
        Genre = ValidateNumber(Genre);
        Year = ValidateNumber(Year);

        var data = "?json=true&Type=" + Type + "&GenreID=" + Genre + "&Year=" + Year + "&Search=" + Search;

        ViewModel.ClearAllArrays();

        $(".releases-div").isotope('remove', $(".isotope-item"), null);
        $("#render-body").hide();
        
        ViewModel.Releases.removeAll();
        
        GetFromWebApi(ViewModel.Releases, "Releases/Searchs", data, ".btn-more-Releases");

        $(".releases-div").isotope('reloadItems');
        $("#knockout-js #releases-container").show();
    },

    LoadReleases: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();

        if(ViewModel.Releases().length > 0)
            $("#releases-container").fadeIn('slow');
        else {
            GetFromWebApi(ViewModel.Releases, "Releases/GetAll", data, ".btn-more-Releases");
            
        }
        $("#releases-container").show();
    },

    SearchProjects: function (Type, Client, Year, Search) {
        Type = ValidateNumber(Type);
        Client = ValidateNumber(Client);
        Year = ValidateNumber(Year);

        var data = "?json=true&Type=" + Type + "&ClientID=" + Client + "&Year=" + Year + "&Search=" + Search;

        ViewModel.ClearAllArrays();

        $("#projects-div").isotope('remove', $(".isotope-item"), null);
        $("#render-body").hide();

        ViewModel.Projects.removeAll();

        GetFromWebApi(ViewModel.Projects, "Projects/Searchs", data, ".btn-more-Projects");

        $("#projects-div").isotope('reloadItems');
        $("#knockout-js #projects-container").show();
    },

    LoadProjects: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();
        
        if (ViewModel.Projects().length > 0) {
            $("#projects-container").fadeIn('slow');
        }
        else {
            GetFromWebApi(ViewModel.Projects, "Projects", data, ".btn-more-Projects");
            
        }
        $("#projects-container").show();
    },

    ClearAllArrays: function () {
        $(".model-collection-container").hide();
    }
};

function ResetIsotope() {
}


function GetFromWebApi(Objects,path, data, btn) {
    $.getJSON(ServiceUrl + "/" + path + data, null, function (res) {
        $.each(res.Items, function (index, item) {
            Objects.push(new ViewModelWithImage(item));
        });
        if (res.isGotMoreItems)
            $(btn).css("display", "block");
        else
            $(btn).hide();
    });
}

function GetFromWebApiDetails(Objects, path, data, btn) {
    $.getJSON(ServiceUrl + "/" + path + data, null, function (res) {
        $.each(res.Items, function (index, item) {
            Objects.push(new ViewModelImageDetail(item));
        });
        if (res.isGotMoreItems)
            $(btn).css("display", "block");
        else
            $(btn).hide();
    });
}

function GetFromData(Objects, Data, btn) {
    $.each(Data, function (index, item) {
        Objects.push(new ViewModelImageDetail(item));
    });
}


ViewModelWithImage = function (item) {
    this.ID = item.ID;
    this.Title = item.Title;
    this.Creation = item.Creation;
    this.ImagePath = item.ImagePath;
    this.Type = item.Type;
    this.TypeID = item.TypeID;
    this.SubTitle = item.SubTitle;
}

ViewModelImageDetail = function (item) {
    this.ID = item.ID;
    this.Title = item.Title;
    this.Creation = item.Creation;
    this.ImagePath = item.ImagePath;
    this.Content = item.Content;
    this.Type = item.Type;
    this.TypeID = item.TypeID;
    this.SubTitle = item.SubTitle;
}

function ValidateNumber(tovalitate) {
    if (tovalitate == undefined || tovalitate == '')
        return 0;

    return tovalitate;
}
