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

namespace NTUST.BulkMail.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Test(int pageIndex, string id, string semester)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<member> memberList = new List<member>();
            var pageSizeMax = 15;
            ServiceReference1.Service1SoapClient service1Soap = new ServiceReference1.Service1SoapClient();
            try
            {
                //測試
                //var memberWebserviceSource = service1Soap.members("eel6212", "1121").GetXml();

                var memberWebserviceSource = service1Soap.members(id, semester).GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));

                using (StringReader data = new StringReader(memberWebserviceSource))
                {
                    NewDataSet datas = (NewDataSet)serializer.Deserialize(data);

                    // 反序列化的資料
                    foreach (var item in datas.member)
                    {
                        var model = new member()
                        {
                            IDNO = item.IDNO,
                            name = item.name,
                            unit = item.unit,
                            unicode = item.unicode,
                            tclass = item.tclass,
                            title = item.title,
                            tel = item.tel,
                            email = item.email,
                            kind = item.kind,
                            unitcode = item.unitcode,
                        };
                        memberList.Add(model);
                    }
                }

                var result = memberList.ToPagedList(pageIndex, pageSizeMax);
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