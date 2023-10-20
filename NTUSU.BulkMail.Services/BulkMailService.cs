using AutoMapper;
using NTUST.BulkMail.EntityFramework;
using NTUST.BulkMail.EntityFramework.Infrastructure;
using NTUST.BulkMail.EntityFramework.Interface;
using NTUST.BulkMail.Services.Interface;
using NTUST.BulkMail.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NTUSU.BulkMail.Models;
using NTUST.BulkMail.Repository;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Services.Description;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Collections;
using System.Xml.Linq;
using NLog;
using System.IO.Compression;
using System.Net.Mail;
using System.Net;

namespace NTUST.BulkMail.Services
{
    public class BulkMailService : IBulkMailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffmemberRepository _staffmemberRepository;
        private readonly IStaffmemberTempRepository _staffmemberTempRepository;
        private readonly IEduCodeRepository _eduCodeRepository;
        private readonly IEduCodeTempRepository _eduCodeTempRepository;
        private readonly IStuMemberRepository _stuMemberRepository;
        private readonly IStuMemberTempRepository _stuMemberTempRepository;
        private readonly IUnitcodeRepository _unitcodeRepository;
        private readonly IMailGroupRepository _mailGroupRepository;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public BulkMailService(IUnitOfWork unitOfWork,
            IStaffmemberRepository staffmemberRepository,
            IStaffmemberTempRepository staffmemberTempRepository,
            IEduCodeRepository eduCodeRepository,
            IEduCodeTempRepository eduCodeTempRepository,
            IStuMemberRepository stuMemberRepository,
            IStuMemberTempRepository stuMemberTempRepository,
            IUnitcodeRepository unitcodeRepository,
            IMailGroupRepository mailGroupRepository)
        {
            _unitOfWork = unitOfWork;
            _staffmemberRepository = staffmemberRepository;
            _staffmemberTempRepository = staffmemberTempRepository;
            _eduCodeRepository = eduCodeRepository;
            _eduCodeTempRepository = eduCodeTempRepository;
            _stuMemberRepository = stuMemberRepository;
            _stuMemberTempRepository = stuMemberTempRepository;
            _unitcodeRepository = unitcodeRepository;
            _mailGroupRepository = mailGroupRepository;
        }

        public void CreateEduCode()
        {
            educode_temp educode_Temp = new educode_temp();
            NtustStudent.StuData stuData = new NtustStudent.StuData();
            try
            {
                var eduCode = stuData.get_educode().GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                NewDataSet datas = null;
                using (StringReader data = new StringReader(eduCode))
                {
                    datas = (NewDataSet)serializer.Deserialize(data);
                }
                DeleteEduCode();
                _logger.Info("Insert資料表資料-educode_temp");
                foreach (var item in datas.educode)
                {
                    educode_Temp.EduCode = item.EduCode.Trim();
                    educode_Temp.College1 = item.College.Trim();
                    educode_Temp.Period2 = item.Period.Trim();
                    educode_Temp.Department2 = item.Department.Trim();
                    educode_Temp.Grade1 = item.Grade.Trim();
                    educode_Temp.Class1 = item.Class.Trim();
                    educode_Temp.Team2 = item.Team.Trim();
                    _eduCodeTempRepository.Add(educode_Temp);
                    Save();
                }
            }
            catch (Exception ex)
            {

            }
            _logger.Info("Insert資料表資料-educode");
            _eduCodeRepository.ExecuteSqlCommand("insert into educode select a.EduCode, a.College1, a.Period2, b.sPeriod2, a.Department2, a.Grade1, a.Class1, a.Team2 from educode_temp a inner join GensPeriod2 b on a.Period2 = b.Period2");
        }
        public void CreateStaffMember(string id, string semester)
        {
            staffmember_temp staffmember_Temp = new staffmember_temp();
            staffmember staffmember = new staffmember();
            NtustMember.Service1 service = new NtustMember.Service1();
            //NtustStudent.StuData stuData = new NtustStudent.StuData();
            try
            {
                var memberWebserviceSource = service.members(id, semester).GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                NewDataSet datas = null;
                using (StringReader data = new StringReader(memberWebserviceSource))
                {
                    datas = (NewDataSet)serializer.Deserialize(data);
                }
                DeleteStaffMember();
                _logger.Info("Insert資料表資料-staffmember staffmember_temp");
                foreach (var item in datas.member)
                {
                    staffmember_Temp.name = item.name.Trim();
                    staffmember_Temp.idno = item.IDNO.Trim();
                    staffmember_Temp.unitcode = item.unitcode.Trim();
                    staffmember_Temp.unicode = item.unicode.Trim();
                    staffmember_Temp.@class = item.tclass.Trim();
                    staffmember_Temp.email = item.email.Trim();
                    staffmember_Temp.tel = item.tel.Trim();
                    staffmember_Temp.unit = item.unit.Trim();
                    staffmember_Temp.title = item.title.Trim();
                    staffmember_Temp.kind = item.kind.Trim();

                    if (item.title != "副校長")
                    {
                        if (item.name == "談家珍" || item.title == "四等技術師(B)" || item.title == "四等技術師(A)")
                        {
                            item.title = "技術師";
                        }
                        if (item.tclass == "正式" && item.title == "助教")
                        {
                            item.title = "助理教授";
                        }
                        staffmember.name = item.name.Trim();
                        staffmember.idno = item.IDNO.Trim();
                        staffmember.unitcode = item.unitcode.Trim();
                        staffmember.unicode = item.unicode.Trim();
                        staffmember.@class = item.tclass.Trim();
                        staffmember.email = item.email.Trim();
                        staffmember.tel = item.tel.Trim();
                        staffmember.unit = item.unit.Trim();
                        staffmember.title = item.title.Trim();
                        staffmember.kind = item.kind.Trim();
                        _staffmemberRepository.Add(staffmember);
                        //Save();
                    }
                    _staffmemberTempRepository.Add(staffmember_Temp);
                    Save();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
        public void CreateStudentData(string pnowcondition)
        {
            NtustStudent.StuData stuData = new NtustStudent.StuData();
            stumember stumember = new stumember();
            stumember_temp stumember_Temp = new stumember_temp();
            try
            {
                var stuWebserviceSource = stuData.studata_cond(pnowcondition).GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                NewDataSet datas = null;
                using (StringReader data = new StringReader(stuWebserviceSource))
                {
                    datas = (NewDataSet)serializer.Deserialize(data);
                }
                DeleteStudentData();
                _logger.Info("Insert資料表資料-stumember stumember_temp");
                foreach (var item in datas.student)
                {
                    string text = item.educode.Trim();
                    string code = text.Substring(1, 1).ToUpper();
                    int grade = int.Parse(text.Substring(5, 1));
                    string ext = grade.ToString();
                    if (grade <= 1)
                    {
                        ext = "N";
                    }
                    if ((code == "A" || code == "P") && grade >= 2)
                    {
                        ext = "G";
                    }
                    else if (code == "B" && grade >= 4)
                    {
                        ext = "G";
                    }
                    else if (code == "M" && grade >= 2)
                    {
                        ext = "G";
                    }
                    else if (code == "D" && grade >= 3)
                    {
                        ext = "G";
                    }
                    else if (code == "U" && grade >= 2)
                    {
                        ext = "G";
                    }
                    stumember_Temp.idno = item.idno.Trim();
                    stumember_Temp.name = item.name.Trim();
                    stumember_Temp.sno1 = item.stuNo1.Trim();
                    stumember_Temp.sex = item.sex.Trim();
                    stumember_Temp.birthday = item.bdate.Trim();
                    stumember_Temp.grp = item.group.Trim();
                    stumember_Temp.addr = item.addr.Trim();
                    stumember_Temp.tel = item.tel.Trim();
                    stumember_Temp.sno2 = item.stuNo2.Trim();
                    stumember_Temp.educode = item.educode.Trim();
                    stumember_Temp.ext = ext.Trim();
                    stumember_Temp.foreignermark = item.foreignermark.Trim();
                    stumember_Temp.nation = item.nationalityIdentity.Trim();
                    _stuMemberTempRepository.Add(stumember_Temp);

                    stumember.idno = item.idno.Trim();
                    stumember.name = item.name.Trim();
                    stumember.sno1 = item.stuNo1.Trim();
                    stumember.sex = item.sex.Trim();
                    stumember.birthday = item.bdate.Trim();
                    stumember.grp = item.group.Trim();
                    stumember.addr = item.addr.Trim();
                    stumember.tel = item.tel.Trim();
                    stumember.sno2 = item.stuNo2.Trim();
                    stumember.educode = item.educode.Trim();
                    stumember.ext = ext.Trim();
                    stumember.foreignermark = item.foreignermark.Trim();
                    stumember.nation = string.IsNullOrEmpty(item.nationalityIdentity.Trim()) ? "本國" : item.nationalityIdentity.Trim();
                    _stuMemberRepository.Add(stumember);
                    Save();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CreateAlumnusData()
        {
            NtustStudent.StuData stuData = new NtustStudent.StuData();
            try
            {
                var stuWebserviceSource = stuData.studata_Graduate().GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                NewDataSet datas = null;
                using (StringReader data = new StringReader(stuWebserviceSource))
                {
                    datas = (NewDataSet)serializer.Deserialize(data);
                }

                string insert = "insert into alumnusmember_temp(idno,name,sno1,sex,birthday,grp,unit,subunit,gtype,addr,tel,sno2,educode,email,graduateyear,valid,foreignermark,china_mark,AbroadMark,reply) " +
                    "values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19})";
                using (var dbContext = new mailEntities())
                {
                    dbContext.Database.ExecuteSqlCommand("delete alumnusmember_temp", Array.Empty<object>());
                    _logger.Info("刪除資料表資料-alumnusmember_temp");
                    _logger.Info("Insert資料表資料-alumnusmember_temp");
                    foreach (var item in datas.GradStudent)
                    {
                        string[] grpary = item.group.Split('/')[1].Split(' ');
                        string unit;
                        string subunit;
                        string gtype;
                        if (grpary.Length >= 3)
                        {
                            unit = grpary[0];
                            subunit = grpary[1];
                            gtype = grpary[2];
                        }
                        else
                        {
                            unit = "";
                            subunit = "";
                            gtype = "";
                        }
                        dbContext.Database.ExecuteSqlCommand(insert, new object[]
                        {
                            item.idno.Trim(),
                            item.name.Trim(),
                            item.stuNo1.Trim(),
                            item.sex.Trim(),
                            item.bdate.Trim(),
                            item.group.Trim(),
                            unit,
                            subunit,
                            gtype,
                            item.addr.Trim(),
                            item.tel.Trim(),
                            item.stuNo2.Trim(),
                            item.educode.Trim(),
                            item.email1.Trim(),
                            item.graduationYear.Trim(),
                            CheckEmail(item.email1.Trim()),
                            item.foreignermark.Trim(),
                            item.chinaMark.Trim(),
                            item.abroadMark.Trim(),
                            item.replay.Trim(),
                        });
                    }
                    dbContext.Database.ExecuteSqlCommand("delete alumnusmember;" +
                        "insert into alumnusmember select * from alumnusmember_temp where sno1 not in (select sno1 from alumnusmember_temp group by sno1 having count(*)>1)");
                    _logger.Info("Insert資料表資料-alumnusmember");
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CreateSendBigMailBulletinBoardNew(string dtStart, string dtEnd)
        {
            using (var dbContext = new mailEntities())
            {
                var query = "WITH epage(pt_seq,pt_name,pt_all_url,pt_desc,pt_rel_date,pt__user1,pt__user2,pt_userid,pt_lang,pt_added)AS(select pt_seq,pt_name,pt_all_url,pt_desc,pt_rel_date,pt__user1,pt__user2,pt_userid,pt_lang,pt_added \r\nfrom openquery(RPAGEDB,'select * from rpagedb.pt_view where CASE WHEN DATE(pt_added)=DATE(pt_rel_date) THEN pt_added ELSE pt_rel_date END between ''{0}'' and ''{1}'''))\r\ninsert into sendBigMailBulletinBoardNew(pt_name,pt_all_url,pt_desc,pt_rel_date,pt__user1,membermail,membername,pt_added,sendDate,sended)\r\nselect a.pt_name,a.pt_all_url,SUBSTRING(a.pt_desc,1,800) as pt_desc,a.pt_rel_date,a.pt__user1,ISNULL(c.membermail,b.val) as membermail,ISNULL(c.membername,b.val) as membername,pt_added,'{1}' as sendDate,CAST(0 as bit) as sended \r\nfrom epage a cross apply dbo.mailSplit(a.pt__user2) b left outer join groupmailviewnewsDailyMail c on b.val+'@ns.ntust.edu.tw'=c.mail order by membermail;";
                query = string.Format(query, dtStart, dtEnd);
                dbContext.Database.CommandTimeout = new int?(5000000);
                dbContext.Database.ExecuteSqlCommand("truncate table sendBigMailBulletinBoardNew;");
                dbContext.Database.ExecuteSqlCommand(query);
                List<epageMail> result = dbContext.Database.SqlQuery<epageMail>("select distinct pt_name,pt_all_url,pt_desc,pt_rel_date,pt__user1,membermail,membername,pt_added,sendDate,sended from sendBigMailBulletinBoardNew where sended =0 order by membermail", Array.Empty<object>()).ToList<epageMail>();
                epageMail last = result.FirstOrDefault<epageMail>();
                StringBuilder sbMail = new StringBuilder();
                string dataContent = "<tr>\r\n\t\t\t\t                    <td data-title=\"發佈單位\">{1}</td>\r\n                                    <td data-title=\"標題\"><a target=\"_blank\" href=\"{3}\" style=\"text-decoration:none;\">{2}</a></td>\r\n                                   </tr>";
                string mailTail = "</tbody></table></div></body></html>";
                string mailHead = File.ReadAllText("D:\\sendMailTemplate\\indexMy.html");
                MailAddress sender = new MailAddress("bulletin@mail.ntust.edu.tw", "臺科公佈欄");
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "140.118.31.96";
                    smtp.Port = 25;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = false;
                    smtp.Credentials = new NetworkCredential("netadmin", "hvf543$#%vghxVgdDxc");
                    string subject = string.Format("[TaiwanTech] 臺科公佈欄(NTUST Bulletin)[{0}-{1}]", dtStart, dtEnd);
                    foreach (epageMail q in result)
                    {
                        if (last.membermail != q.membermail)
                        {
                            MailMessage msg = new MailMessage();
                            msg.Subject = subject;
                            msg.Body = mailHead + sbMail.ToString() + mailTail;
                            msg.BodyEncoding = Encoding.UTF8;
                            msg.IsBodyHtml = true;
                            msg.Priority = MailPriority.Normal;
                            msg.From = sender;
                            try
                            {
                                msg.To.Add(new MailAddress(last.membermail, last.membername));
                                //msg.To.Add(new MailAddress("shadow@mail.ntust.edu.tw", "shadow"));
                                smtp.Send(msg);
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.ToString());
                            }
                            //Console.WriteLine("寄送給{0}", last.membername);
                            last = q;
                            sbMail.Clear();
                        }
                        string url = q.pt_all_url;
                        Match re = Regex.Match(url, "(https://bulletin.ntust.edu.tw/)([\\w\\-/]+)(\\.php\\?Lang=zh-tw)");
                        if (re.Success)
                        {
                            url = re.Groups[1].Value + re.Groups[2].Value + ",r1391" + re.Groups[3].Value;
                        }
                        sbMail.AppendFormat(dataContent, new object[]
                        {
                        q.pt_added.ToString("yyyy/MM/dd HH:mm"),
                        q.pt__user1,
                        q.pt_name,
                        url
                        });
                    }
                    if (last != null)
                    {
                        MailMessage msg2 = new MailMessage();
                        msg2.Subject = subject;
                        msg2.Body = mailHead + sbMail.ToString() + mailTail;
                        msg2.BodyEncoding = Encoding.UTF8;
                        msg2.IsBodyHtml = true;
                        msg2.Priority = MailPriority.Normal;
                        msg2.From = sender;
                        try
                        {
                            msg2.To.Add(new MailAddress(last.membermail, last.membername));
                            //msg2.To.Add(new MailAddress("shadow@mail.ntust.edu.tw", "shadow"));
                            smtp.Send(msg2);
                        }
                        catch (Exception)
                        {
                        }
                        dbContext.Database.ExecuteSqlCommand("insert into BigMailBulletinBoardSendedLog(mail) values({0})", new object[]
                        {
                        last.membermail
                        });
                    }
                    //Console.WriteLine("完成寄送校園公佈欄{0}-{1} :{2}", dtStart, dtEnd, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                }

            }
        }
        public void GenerateDataFromRawData()
        {
            try
            {
                using (var dbContext = new mailEntities())
                {
                    _logger.Info("Update資料表資料-staffmember set email='jyen@mail.ntust.edu.tw'  where name='顏家鈺'");
                    dbContext.Database.ExecuteSqlCommand("update staffmember set email='jyen@mail.ntust.edu.tw'  where name='顏家鈺'");
                    
                    _logger.Info("Delete資料表資料-mailgroup");
                    _logger.Info("Insert資料表資料-mailgroup");
                    dbContext.Database.ExecuteSqlCommand("delete mailgroup; " +
                        "insert into mailgroup(name,mail,groupmail) select distinct cname as name,SUBSTRING(mail,1,LEN(mail)-15)+'mail.ntust.edu.tw' as mail,mail as groupmail from groupmailviewnews");
                    
                    _logger.Info("Delete資料表資料-groupmailviewnewdata");
                    dbContext.Database.ExecuteSqlCommand("delete groupmailviewnewdata;insert into groupmailviewnewdata select * from groupmailviewnew");
                    
                    _logger.Info("Delete資料表資料-groupmailviewnewdatadistinct");
                    _logger.Info("Insert資料表資料-groupmailviewnewdatadistinct");
                    dbContext.Database.ExecuteSqlCommand("delete groupmailviewnewdatadistinct;insert into groupmailviewnewdatadistinct(cname,mail) select distinct cname,mail from groupmailviewnewdata");
                    
                    _logger.Info("Delete資料表資料-GroupMailViewNewDropdownlistData");
                    _logger.Info("Insert資料表資料-GroupMailViewNewDropdownlistData");
                    dbContext.Database.ExecuteSqlCommand("delete GroupMailViewNewDropdownlistData;insert into GroupMailViewNewDropdownlistData select * from dbo.groupmailviewnewdropdownlist1");
                    
                    _logger.Info("Delete資料表資料-groupmailviewnewsDailyMail");
                    _logger.Info("Insert資料表資料-groupmailviewnewsDailyMail");
                    string sendDailyMail = "truncate table groupmailviewnewsDailyMail;insert into groupmailviewnewsDailyMail select * from groupmailviewnews";
                    dbContext.Database.ExecuteSqlCommand(sendDailyMail);
                }
            }
            catch(Exception ex)
            {

            }
        }
        public void GenerateMailGroupFile()
        {
            try
            {
                _logger.Info("產出大宗郵件收件人");
                string lastfilename = "";
                StreamWriter sw = null;
                if (Directory.Exists("D:\\getMailFile\\data\\"))
                {
                    Directory.Delete("D:\\getMailFile\\data\\", true);
                }
                Directory.CreateDirectory("D:\\getMailFile\\data\\");
                using (var dbContext = new mailEntities())
                {
                    dbContext.Database.CommandTimeout = new int?(1000000);
                    DbRawSqlQuery<GroupFile> dbRawSqlQuery = dbContext.Database.SqlQuery<GroupFile>("select distinct mail,filename,member from groupmailviewnews order by filename");
                    foreach (var row in dbRawSqlQuery)
                    {
                        if (row.filename.ToString() != lastfilename)
                        {
                            if (sw != null)
                            {
                                sw.Close();
                            }
                            sw = new StreamWriter("D:\\getMailFile\\data\\" + row.filename.ToString(), false, Encoding.GetEncoding("big5"));
                            lastfilename = row.filename.ToString();
                            string mail = row.mail.ToString().Split(new char[]
                            {
                        '@'
                            })[0] + "@mail.ntust.edu.tw";
                            sw.Write(mail + "\n");
                            Console.WriteLine(mail);
                        }
                        sw.Write(row.member.ToString().Trim() + "\n");
                        Console.WriteLine(row.member.ToString());
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void GenerateAliasesFile()
        {
            try
            {
                _logger.Info("產出大宗郵件收件人別名");
                string lastfilename = "";
                File.Copy("D:\\getMailFile\\template\\aliasestemplate", "D:\\getMailFile\\template\\aliases", true);
                StreamWriter sw = new StreamWriter("D:\\getMailFile\\template\\aliases", true, Encoding.GetEncoding("big5"));
                using (var dbContext = new mailEntities())
                {
                    dbContext.Database.CommandTimeout = new int?(1000000);
                    DbRawSqlQuery<GroupFile> dbRawSqlQuery = dbContext.Database.SqlQuery<GroupFile>("select distinct mail,filename from groupmailviewnews order by mail");
                    foreach (var row in dbRawSqlQuery)
                    {
                        if (!(row.mail == "stud-b0xx-x@ns.ntust.edu.tw") && !(row.mail == "stud-a0xx-x@ns.ntust.edu.tw") && !(row.mail == "stud-p0xx-x@ns.ntust.edu.tw"))
                        {
                            string data = string.Format("{0}:    :include:/etc/mail-list/{1}\n", row.mail.Split(new char[]
                            {
                        '@'
                            })[0], row.filename);
                            sw.Write(data);
                        }
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
        }
        public void DeleteEduCode()
        {
            _logger.Info("清除資料表資料-educode educode_temp");
            _eduCodeRepository.ExecuteSqlCommand("TRUNCATE TABLE educode");
            _eduCodeTempRepository.ExecuteSqlCommand("TRUNCATE TABLE educode_temp");
        }
        public void DeleteStaffMember()
        {
            _logger.Info("清除資料表資料-staffmember staffmember_temp");
            _staffmemberRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember");
            _staffmemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember_temp");
        }
        public void DeleteStudentData()
        {
            _logger.Info("清除資料表資料-stumember stumember_temp");
            _stuMemberRepository.ExecuteSqlCommand("TRUNCATE TABLE stumember");
            _stuMemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE stumember_temp");
        }
        public IEnumerable<member> GetStaffMemberData()
        {
            var query = _staffmemberRepository.GetAll();
            var result = query.Select(x => new member()
            {
                name = x.name,
                unit = x.unit,
                email = x.email,
                unitcode = x.unitcode,
            });
            return result.OrderBy(x => x.unitcode);
        }
        public IEnumerable<unitcode> GetUnitcodesDataList()
        { 
            var query = _unitcodeRepository.GetAll();
            return query.OrderBy(x => x.unitcode1);
        }
        public unitcode GetUnitcodesData(string tunitcode, string unitcode)
        {
            var query = _unitcodeRepository.Get(x => x.tunitcode == tunitcode && x.unitcode1 == unitcode);
            return query;
        }

        public IEnumerable<MailGroupViewModal> GetMailGroup()
        {
            var query = _mailGroupRepository.GetAll().Select(x => new MailGroupViewModal()
            {
                name = x.name,
                mail = x.mail,
                groupmail = x.groupmail,
            });
            var result = query.Distinct().OrderBy(x => x.name);
            return result;
        }
        
        public List<MailGroupListViewModal> GetMailGroupList(string groupName)
        {
            using (var dbContext = new mailEntities())
            {
                DbRawSqlQuery<GroupMailViewNews> dbRawSqlQuery = dbContext.Database.SqlQuery<GroupMailViewNews>("SELECT * FROM groupmailviewnews WHERE cname = {0} ORDER BY cname", groupName);
                List<MailGroupListViewModal> list = new List<MailGroupListViewModal>();
                foreach (var item in dbRawSqlQuery)
                {
                    MailGroupListViewModal query = new MailGroupListViewModal();
                    query.cname = item.cname;
                    query.name = item.membername;
                    query.mail = item.membermail;
                    query.groupmail = item.mail;
                    list.Add(query);
                }
                return list;
            }
        }

        public List<MailGroupListViewModal> GetMailGroupName(string groupName)
        {
            using (var dbContext = new mailEntities())
            {
                DbRawSqlQuery<GroupMailViewNews> dbRawSqlQuery = dbContext.Database.SqlQuery<GroupMailViewNews>("SELECT * FROM groupmailviewnews WHERE cname = {0} ORDER BY cname", groupName);
                List<MailGroupListViewModal> list = new List<MailGroupListViewModal>();
                foreach (var item in dbRawSqlQuery)
                {
                    MailGroupListViewModal query = new MailGroupListViewModal();
                    query.cname = item.cname;
                    query.name = item.membername;
                    query.mail = item.membermail;
                    query.groupmail = item.mail;
                    list.Add(query);
                }
                return list;
            }
        }

        public void UpdateUnicodeData(UnicodeViewModal unitcode)
        {
            var query = _unitcodeRepository.Get(x => x.tunitcode == unitcode.tunitcode && x.unitcode1 == unitcode.unitcode1);
            query.tunitcode = unitcode.tunitcode;
            query.unit = unitcode.unit;
            query.unitcode1 = unitcode.unitcode1;
            _unitcodeRepository.Update(query);
            Save();
        }
        public void CreateUnicodeData(UnicodeViewModal unitcodeObject)
        {
            unitcode unitcode = new unitcode();
            unitcode.unitcode1 = unitcodeObject.unitcode1;
            unitcode.tunitcode = unitcodeObject.tunitcode;
            unitcode.unit = unitcodeObject.unit;
            _unitcodeRepository.Add(unitcode);
            Save();
        }

        public void DeleteUnicodeData(string tunitcode, string unitcode)
        {
            var query = _unitcodeRepository.Get(x => x.tunitcode == tunitcode && x.unitcode1 == unitcode);
            _unitcodeRepository.Delete(query);
            Save();
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        //反組譯 蟹老闆大宗郵件程式碼-檢查email格式
        private static string CheckEmail(string email)
        {
            if (new Regex("^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z0-9]+[\\w-]+\\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$").IsMatch(email))
            {
                return "1";
            }
            return "0";
        }

        //反組譯 蟹老闆大宗郵件程式碼-取學年度
        private static string GetSemester()
        {
            DateTime dt = DateTime.Now;
            int month = dt.Month;
            string y_seme;
            if (month >= 7 && month <= 8)
            {
                y_seme = (dt.Year - 1912).ToString() + "h";
            }
            else if (month >= 2 && month <= 6)
            {
                y_seme = (dt.Year - 1912).ToString() + "2";
            }
            else if (month == 1)
            {
                y_seme = (dt.Year - 1912).ToString() + "1";
            }
            else
            {
                y_seme = (dt.Year - 1911).ToString() + "1";
            }
            return y_seme;
        }
    }
}
