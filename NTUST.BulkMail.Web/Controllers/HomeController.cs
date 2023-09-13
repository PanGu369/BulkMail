using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using PagedList;
using NTUST.BulkMail.Models;
using Microsoft.Ajax.Utilities;
using NTUSU.BulkMail.Models;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlTypes;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using System.IO.Pipes;
using System.Runtime.Remoting.Contexts;
using NLog;
using NTUST.BulkMail.Services.Interface;
using NTUST.BulkMail.Services;
using Microsoft.Owin.Logging;
using System.Linq.Dynamic;

namespace NTUST.BulkMail.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBulkMailService _bulkMailService;
        public HomeController(IBulkMailService bulkMailService)
        {
            _bulkMailService = bulkMailService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(int pageIndex, string id, string semester)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<member> memberList = new List<member>();
            try
            {
                _bulkMailService.CreateEduCode();
                _bulkMailService.CreateStaffMemberTemp(id, semester);
                _bulkMailService.CreateStaffMember();
                var query = _bulkMailService.GetStaffMemberData();
                var result = query.ToPagedList(pageIndex, query.Count());
                var pageListViewModel = new PageList
                {
                    FirstItemOnPage = result.FirstItemOnPage,
                    HasNextPage = result.HasNextPage,
                    HasPreviousPage = result.HasPreviousPage,
                    IsFirstPage = result.IsFirstPage,
                    IsLastPage = result.IsLastPage,
                    LastItemOnPage = result.LastItemOnPage,
                    PageCount = result.PageCount,
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalItemCount = result.TotalItemCount,
                    ItemList = result.ToList(),
                };

                resultMessage.Status = "OK";
                var content = new
                {
                    resultMessage = resultMessage,
                    pageListViewModel = pageListViewModel,
                };
                return Json(content, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = "";
                var content = new
                {
                    resultMessage = resultMessage,
                };
                return Json(content, JsonRequestBehavior.AllowGet);
            }
        }
    }
}