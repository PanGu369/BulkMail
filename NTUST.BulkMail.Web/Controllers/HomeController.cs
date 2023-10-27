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
using NTUST.BulkMail.EntityFramework;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mime;
using WebGrease.Activities;
using Octokit;
using System.Web.Services.Description;
using System.IO.Compression;

namespace NTUST.BulkMail.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBulkMailService _bulkMailService;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private MailMessage msg = new MailMessage();
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
                var query = _bulkMailService.GetStaffMemberData();
                var lostUnintCode = _bulkMailService.GetLostUnitCode();
                var lostStaffClassTitleCode = _bulkMailService.GetLostStaffClassTitleCode();
                if (query.Any())
                {
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
                        lostUnintCode = lostUnintCode,
                        lostStaffClassTitleCode = lostStaffClassTitleCode,
                    };
                    return Json(content, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    resultMessage.Status = "EMPTY";
                    var content = new
                    {
                        resultMessage = resultMessage,
                    };
                    return Json(content, JsonRequestBehavior.AllowGet);
                }
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
        public ActionResult UnincodeList(int pageIndex)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<member> memberList = new List<member>();
            try
            {
                var query = _bulkMailService.GetUnitcodesDataList();
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
        public ActionResult GetUnicodeData(string tunitcode, string unitcode)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<member> memberList = new List<member>();
            try
            {
                var query = _bulkMailService.GetUnitcodesData(tunitcode, unitcode);

                resultMessage.Status = "OK";
                var content = new
                {
                    unicodeData = query,
                    resultMessage = resultMessage,
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
        public ActionResult GetMailGroup()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                resultMessage.Body = _bulkMailService.GetMailGroup();
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetMailGroupList(string groupName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                resultMessage.Body = _bulkMailService.GetMailGroupList(groupName);
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetStaffmember(string id, string semester)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.CreateStaffMember(id, semester);
                _logger.Info($"教職員資料更新成功");
                resultMessage.Status = "OK";
            }
            catch(Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message= ex.ToString();
                _logger.Error(ex.ToString());
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetStudentData(string code)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.CreateStudentData(code);
                _logger.Info($"學生資料更新成功");
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
                _logger.Error(ex.ToString());
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAlumnusData()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.CreateAlumnusData();
                _logger.Info($"畢業生資料更新成功");
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
                _logger.Error(ex.ToString());
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GenerateMailGroupFile()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.GenerateDataFromRawData();
                _bulkMailService.GenerateMailGroupFile();
                _logger.Info($"大宗郵件資料建立成功");
                resultMessage.Status = "OK";
            }
            catch(Exception ex) 
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
                _logger.Error(ex.ToString());
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateAliasesFile()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.GenerateAliasesFile();
                _logger.Info($"大宗郵件別名資料更新成功");
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
                _logger.Error(ex.ToString());
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateUnicodeData(UnicodeViewModal unitcode)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.UpdateUnicodeData(unitcode);
                resultMessage.Status = "OK";
            }
            catch(Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUnicodeData(string tunitcode, string unitcode)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.DeleteUnicodeData(tunitcode, unitcode);
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateUnicodeData(UnicodeViewModal unitcode)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                _bulkMailService.CreateUnicodeData(unitcode);
                resultMessage.Status = "OK";
            }
            catch (Exception ex)
            {
                resultMessage.Status = "NG";
                resultMessage.Message = ex.ToString();
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadZip()
        {
            string folderPath1 = @"D:\getMailFile\data";
            string folderPath2 = @"D:\getMailFile\template";

            if (Directory.Exists(folderPath1) && Directory.Exists(folderPath2))
            {
                // 創建一个内存以保存 ZIP 數據
                using (MemoryStream zipStream = new MemoryStream())
                {
                    using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        // 添加文件夾1中的多個文件到 ZIP 存檔
                        AddFolderToZip(archive, folderPath1);

                        // 添加文件夾2中的多個文件到 ZIP 存檔
                        AddFolderToZip(archive, folderPath2);
                    }

                    // 將 ZIP 數據返回给前端
                    byte[] zipData = zipStream.ToArray();
                    return File(zipData, "application/zip", "archive.zip");
                }
            }
            else
            {
                // 處理文件夾不存在的情况
                return Content("一個或兩個文件夾不存在");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SendEmail()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                MailAddress sender = new MailAddress("shadow@mail.ntust.edu.tw", "shadow");
                HttpFileCollectionBase collectionBase = Request.Files;
                //NtustEmail mail = new NtustEmail();
                // 从FormData中获取JSON数据
                string json = Request.Unvalidated.Form["mail"];

                // 将JSON字符串转换回对象
                NtustEmail mail = JsonConvert.DeserializeObject<NtustEmail>(json);
                using (SmtpClient smtp = new SmtpClient())
                {
                    if (Request.Files.AllKeys.Any())
                    {
                        for (int i = 0, j = collectionBase.Count; i < j; i++)
                        {
                            HttpPostedFileBase item = collectionBase[i];

                            Stream stream = item.InputStream;
                            BinaryReader reader = new BinaryReader(stream);
                            byte[] imageBytes = reader.ReadBytes((int)stream.Length);

                            MemoryStream fileStream = new MemoryStream(imageBytes);
                            Attachment attachment = new Attachment(fileStream, item.FileName);
                            msg.Attachments.Add(attachment);
                        }
                    }
                    smtp.Host = "140.118.31.96";
                    smtp.Port = 25;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = false;
                    smtp.Credentials = new NetworkCredential("netadmin", "hvf543$#%vghxVgdDxc");

                    //MailMessage msg = new MailMessage();
                    msg.Subject = mail.subject;
                    msg.Body = mail.email_content;
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.Priority = MailPriority.Normal;
                    msg.From = sender;


                    //string owner = "PanGu369";
                    //string repo = "uploadFile";
                    //string path = Session["filePath"].ToString(); // 文件在仓库中的路径

                    //string token = "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";

                    //string githubRawUrl = $"https://raw.githubusercontent.com/{owner}/{repo}/main/{path}";

                    //HttpClient httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                    //httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PanGu369"); // 替换为您的应用名称

                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);

                    //HttpResponseMessage response = await httpClient.GetAsync(githubRawUrl);

                    //WebClient webClient = new WebClient();
                    //webClient.Headers.Add("User-Agent", "PanGu369"); // 替换为您的应用名称
                    //webClient.Headers.Add("Authorization", $"token {token}");

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    // 使用GitHub Raw URL下载文件内容
                    //    byte[] fileBytes = await webClient.DownloadDataTaskAsync(githubRawUrl);

                    //    // 获取GitHub文件名
                    //    string[] pathParts = path.Split('/');
                    //    string fileName = pathParts[pathParts.Length - 1];


                    //    // 将GitHub文件内容作为附件添加到电子邮件中
                    //    //MemoryStream fileStream = new MemoryStream(fileBytes);
                    //    //Attachment attachment = new Attachment(fileStream, fileName);
                    //    //msg.Attachments.Add(attachment);
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
                    //    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    //}

                    if (mail.receiver.Count > 0)
                    {
                        foreach (var item in mail.receiver)
                        {
                            if (string.IsNullOrEmpty(item.cname))
                            {
                                var mailGroup = _bulkMailService.GetMailGroupName(item.name);
                                //msg.To.Add(new MailAddress("shadow@mail.ntust.edu.tw", "shadow"));
                                foreach (var itemMail in mailGroup)
                                {
                                    msg.To.Add(new MailAddress(itemMail.mail, itemMail.name));
                                    //msg.To.Add(new MailAddress(item.groupmail, item.name));
                                }
                                smtp.Send(msg);
                            }
                            else
                            {
                                msg.To.Add(new MailAddress("shadow@mail.ntust.edu.tw", "shadow"));
                                //msg.To.Add(new MailAddress(item.mail, item.name));
                                smtp.Send(msg);
                            }
                        }
                    }
                    else
                    {
                        resultMessage.Status = "EMPTY";
                        resultMessage.Message = "沒有收件人";
                        return Json(resultMessage, JsonRequestBehavior.AllowGet);
                    }
                }
                resultMessage.Status = "OK";
                return Json(resultMessage, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) 
            {
                resultMessage.Status="NG";
                resultMessage.Message = ex.ToString();
                return Json(resultMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SendBulletinBoard()
        {
            ResultMessage resultMessage = new ResultMessage();
            try 
            {
                DateTime dt = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                string dtStart = dt.AddDays(-1.0).ToString("yyyy/MM/dd 19:00");
                string dtEnd = dt.ToString("yyyy/MM/dd 19:00");
               // _bulkMailService.CreateSendBigMailBulletinBoardNew(dtStart, dtEnd);
                resultMessage.Status = "OK";
                return Json(resultMessage, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                System.IO.File.WriteAllText("D:\\log.txt", ex.ToString());
                return Json(resultMessage, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> FileUpload()
        {
            ResultMessage resultMessage = new ResultMessage();
            HttpFileCollectionBase collectionBase = Request.Files;
            if (Request.Files.AllKeys.Any())
            {
                try
                {
                    for (int i = 0, j = collectionBase.Count; i < j; i++)
                    {
                        HttpPostedFileBase item = collectionBase[i];
                        string username = "shadow"; //sso建立後可置換取得的使用者
                        //github token 到期需更換或建立永久 建立github token 可參考 https://shengyu7697.github.io/github-personal-access-token/
                        string token = "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";

                        string owner = "PanGu369";
                        string repo = "uploadFile";
                        string branch = "main";
                        string path = item.FileName;

                        string apiUrl = $"https://api.github.com/repos/{owner}/{repo}/contents/{username}/{path}";

                        var userGitHub = new GitHubClient(new Octokit.ProductHeaderValue("UserGitHubUploader"));
                        var userTokenAuth = new Credentials(token);
                        userGitHub.Credentials = userTokenAuth;

                        HttpClient httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PanGu369");

                        Stream stream = item.InputStream;
                        BinaryReader reader = new BinaryReader(stream);
                        byte[] imageBytes = reader.ReadBytes((int)stream.Length);

                        var requestBody = new
                        {
                            message = username + "於" + DateTime.Now.ToString() + "上傳" + item.FileName + " " + "狀態:上傳成功!",
                            content = Convert.ToBase64String(imageBytes)
                        };

                        var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);

                        HttpResponseMessage response = await httpClient.PutAsync(apiUrl, requestContent);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine(item.FileName + DateTime.Now.ToString() + "上傳成功!");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to upload image. Status code: {response.StatusCode}");
                            Console.WriteLine(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        // 輔助方法：將本地文件夾中的多個文件添加到 ZIP 存檔
        private void AddFolderToZip(ZipArchive archive, string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);

            foreach (string filePath in files)
            {
                string entryName = Path.Combine(Path.GetFileName(folderPath), Path.GetFileName(filePath));
                AddFileToZip(archive, filePath, entryName);
            }
        }

        // 輔助方法：將本地文件添加到 ZIP 存檔
        private void AddFileToZip(ZipArchive archive, string filePath, string entryName)
        {
            var entry = archive.CreateEntry(entryName);
            using (var entryStream = entry.Open())
            {
                using (var fileStream = new FileStream(filePath, System.IO.FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(entryStream);
                }
            }
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