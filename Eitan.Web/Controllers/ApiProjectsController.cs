﻿using Eitan.Data;
using Eitan.Models;
using Eitan.Web.Models;
using Eitan.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Eitan.Web.Api.Controllers
{
    public class ProjectsController : EitanApiBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["ProjectPageSize"]);

        public ProjectsController(IEitanUow uow)
        {
            Uow = uow;
        }

        // GET api/apiProjects
        public PagedViewModelsContainer GetAll(int page = 1, int _pageSize = 0)
        {
            _pageSize = _pageSize == 0 ? pageSize : _pageSize;

            var ViewModel = new PagedViewModelsContainer();
            int itemsleft = Uow.ProjectRepository.GetAll().Count() - (page * _pageSize);
            ViewModel.ItemsLeft = itemsleft < 0 ? 0 : itemsleft;
            ViewModel.isGotMoreItems = itemsleft > 0 ? true : false;
            ViewModel.Items = Uow.ProjectRepository.GetAll("Client").OrderBy(o => o.Priority)
                                    .Skip(--page * _pageSize)
                                    .Take(_pageSize)
                                    .ProjectsToViewModelsWithImage();

            return ViewModel;
        }

        // GET api/// GET api/apiProj/5
        public Project Get(int id)
        {
            return Uow.ProjectRepository.GetByID(id);
        }

        // GET api/apinews/5
        [System.Web.Http.ActionName("Searchs")]
        public PagedViewModelsContainer GetSearchs(int ClientID, int Year, int Type = 0, string Search = "")
        {
            var Query = new ProjectSearchModel(Search, Year, Type, ClientID);
            var ViewModel = new PagedViewModelsContainer();
            ViewModel.Items = Uow.ProjectRepository.SearchQuery(Query)
                                 .ProjectsToViewModelsWithImage();

            return ViewModel;
        }
    }
}
