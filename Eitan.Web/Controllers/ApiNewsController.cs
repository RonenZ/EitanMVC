using Eitan.Data;
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
    public class NewsController : EitanApiBaseController
    {
        private static readonly int pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);

        public NewsController(IEitanUow uow)
        {
            Uow = uow;
        }

        // GET api/apinews
        public PagedViewModelsContainer Get(int page = 1, int _pageSize = 0)
        {
            _pageSize = _pageSize == 0 ? pageSize : _pageSize;

            var ViewModel = new PagedViewModelsContainer();
            int itemsleft = Uow.NewsRepository.GetAll().Count() - (page * _pageSize);
            ViewModel.ItemsLeft = itemsleft < 0 ? 0 : itemsleft;
            ViewModel.isGotMoreItems = itemsleft > 0 ? true : false;
            ViewModel.Items = Uow.NewsRepository.GetAllDesc()
                                    .Skip(--page * _pageSize)
                                    .Take(_pageSize)
                                    .ToViewModelsImageDetail();

            return ViewModel;
        }

        // GET api/apinews/5
        public ViewModelDetailed Get(int id)
        {
            return Uow.NewsRepository.GetByID(id).ToViewModelImageDetail();
        }
    }
}
