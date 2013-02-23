var ServiceUrl = "/api";
var page = 1;

ViewModel = {
    News: ko.observableArray(),
    Releases: ko.observableArray(),
    Projects: ko.observableArray(),
    releaseAdded: function(el) {
        //$("#releases-div").isotope('appended', $(el));
    },
    projectAdded: function (el) {
        //$("#projects-div").isotope('appended', $(el));
    },
    newsAdded: function (el) {
        //$("#news-div").isotope('appended', $(el));
    },
    LoadNews: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();
        $("#news-container").show();

        if (ViewModel.News().length > 0) { }
            //$("#news-div").isotope('shuffle', null);
        else {
            $.getJSON(ServiceUrl + "/News" + data, null, function (res) {
                $.each(res, function (index, item) {
                    ViewModel.News.push(new ViewModelImageDetail(item));
                });
            });
        }
    },

    LoadReleases: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();
        $("#releases-container").show();

        if (ViewModel.Releases().length > 0) { }
            //$("#releases-div").isotope('shuffle', null);
        else {
            $.getJSON(ServiceUrl + "/Releases" + data, null, function (res) {
                $.each(res, function (index, item) {
                    ViewModel.Releases.push(new ViewModelImageDetail(item));
                });
            });
        }
        
    },

    LoadProjects: function () {
        var data = "?json=true&page=" + page;
        ViewModel.ClearAllArrays();
        $("#projects-container").show();
        
        if (ViewModel.Projects().length > 0) { }
            //$("#projects-div").isotope('shuffle', null);
        else {
            $.getJSON(ServiceUrl + "/Projects" + data, null, function (res) {
                $.each(res, function (index, item) {
                    ViewModel.Projects.push(new ViewModelImageDetail(item));
                });
            });
        }
    },

    ClearAllArrays: function () {
        $(".model-collection-container").hide();
    }
};



ViewModelWithImage = function (item) {
    this.ID = item.ID;
    this.Title = item.Title;
    this.Creation = item.Creation;
    this.ImagePath = item.ImagePath;
}

ViewModelImageDetail = function (item) {
    this.ID = item.ID;
    this.Title = item.Title;
    this.Creation = item.Creation;
    this.ImagePath = item.ImagePath;
    this.Content = item.Content;
}