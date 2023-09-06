using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using NTUST.BulkMail.Models;

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
        public ActionResult Test()
        {
            ResultMessage resultMessage = new ResultMessage();
            ServiceReference1.Service1SoapClient service1Soap = new ServiceReference1.Service1SoapClient();
            var test = service1Soap.members("eel6212", "1121");
            string json = JsonConvert.SerializeObject(test);
            var content = new
            {
                data = json,
            };
            return Json(content, JsonRequestBehavior.AllowGet);
        }
    }
}