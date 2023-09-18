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
using NLog.LayoutRenderers.Wrappers;
using Google.Apis.Auth;
using System.Threading.Tasks;
using NTUST.BulkMail.Web.ActionFilter;

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
                //_bulkMailService.CreateEduCode();
                //_bulkMailService.CreateStaffMemberTemp(id, semester);
                //_bulkMailService.CreateStaffMember();
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

        [HttpPost]
        public ActionResult GetStaffmember(string id, string semester)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.CreateStaffMember(id, semester);
                resultMessage.Status = "OK";
            }
            catch(Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message= ex.ToString();
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStudentData(string code)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                //_bulkMailService.CreateStudentData(code);
                //_bulkMailService.CreateAlumnusData();
                _bulkMailService.GenerateMailGroupFile();
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public ActionResult ValidGoogleLogin()
        {
            string formCredential = Request.Form["credential"]; //回傳憑證
            string formToken = Request.Form["g_csrf_token"]; //回傳令牌
            string cookiesToken = Request.Cookies["g_csrf_token"].Value; //Cookie 令牌

            // 驗證 Google Token
            GoogleJsonWebSignature.Payload payload = VerifyGoogleToken(formCredential, formToken, cookiesToken).Result;
            if (payload == null)
            {
                // 驗證失敗
                ViewData["Msg"] = "驗證 Google 授權失敗";
            }
            else
            {
                //驗證成功，取使用者資訊內容
                ViewData["Msg"] = "驗證 Google 授權成功" + "<br>";
                ViewData["Msg"] += "Email:" + payload.Email + "<br>";
                ViewData["Msg"] += "Name:" + payload.Name + "<br>";
                ViewData["Msg"] += "Picture:" + payload.Picture;
            }

            return View();
        }

        /// <summary>
        /// 驗證 Google Token
        /// </summary>
        /// <param name="formCredential"></param>
        /// <param name="formToken"></param>
        /// <param name="cookiesToken"></param>
        /// <returns></returns>
        [AllowCrossSiteJson]
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string formCredential, string formToken, string cookiesToken)
        {
            // 檢查空值
            if (formCredential == null || formToken == null && cookiesToken == null)
            {
                return null;
            }

            GoogleJsonWebSignature.Payload payload;
            try
            {
                // 驗證 token
                if (formToken != cookiesToken)
                {
                    return null;
                }

                // 驗證憑證
                //IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
                string GoogleApiClientId = "514530260057-if5ajahcqno2kppdgs54082st9495mdp.apps.googleusercontent.com";
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { GoogleApiClientId }
                };
                payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
                if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                {
                    return null;
                }
                if (payload.ExpirationTimeSeconds == null)
                {
                    return null;
                }
                else
                {
                    DateTime now = DateTime.Now.ToUniversalTime();
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                    if (now > expiration)
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return payload;
        }
    }
}