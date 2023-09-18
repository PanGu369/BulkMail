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
        private readonly IAlumnusmemberRepository _alumnusmemberRepository;
        private readonly IAlumnusmemberTempRepository _alumnusmemberTempRepository;
        public BulkMailService(IUnitOfWork unitOfWork,
            IStaffmemberRepository staffmemberRepository,
            IStaffmemberTempRepository staffmemberTempRepository,
            IEduCodeRepository eduCodeRepository,
            IEduCodeTempRepository eduCodeTempRepository,
            IStuMemberRepository stuMemberRepository,
            IStuMemberTempRepository stuMemberTempRepository,
            IAlumnusmemberRepository alumnusmemberRepository,
            IAlumnusmemberTempRepository alumnusmemberTempRepository)
        {
            _unitOfWork = unitOfWork;
            _staffmemberRepository = staffmemberRepository;
            _staffmemberTempRepository = staffmemberTempRepository;
            _eduCodeRepository = eduCodeRepository;
            _eduCodeTempRepository = eduCodeTempRepository;
            _stuMemberRepository = stuMemberRepository;
            _stuMemberTempRepository = stuMemberTempRepository;
            _alumnusmemberRepository = alumnusmemberRepository;
            _alumnusmemberTempRepository = alumnusmemberTempRepository;
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
            alumnusmember alumnusmember = new alumnusmember();
            alumnusmember_temp alumnusmember_Temp = new alumnusmember_temp();
            var batchSize = 1000;
            try
            {
                var stuWebserviceSource = stuData.studata_Graduate().GetXml();
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                NewDataSet datas = null;
                using (StringReader data = new StringReader(stuWebserviceSource))
                {
                    datas = (NewDataSet)serializer.Deserialize(data);
                }
                DeleteAlumnusData();
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
                    alumnusmember_Temp.idno = item.idno.Trim();
                    alumnusmember_Temp.name = item.name.Trim();
                    alumnusmember_Temp.sno1 = item.stuNo1.Trim();
                    alumnusmember_Temp.sex = item.sex.Trim();
                    alumnusmember_Temp.birthday = item.bdate.Trim();
                    alumnusmember_Temp.grp = item.group.Trim();
                    alumnusmember_Temp.unit = unit;
                    alumnusmember_Temp.subunit = subunit;
                    alumnusmember_Temp.gtype = gtype;
                    alumnusmember_Temp.addr = item.addr.Trim();
                    alumnusmember_Temp.tel = item.tel.Trim();
                    alumnusmember_Temp.sno2 = item.stuNo2.Trim();
                    alumnusmember_Temp.educode = item.educode.Trim();
                    alumnusmember_Temp.email = item.email1.Trim();
                    alumnusmember_Temp.graduateyear = item.graduationYear.Trim();
                    alumnusmember_Temp.valid = CheckEmail(item.email1.Trim());
                    alumnusmember_Temp.foreignermark = item.foreignermark.Trim();
                    alumnusmember_Temp.china_mark = item.chinaMark.Trim();
                    alumnusmember_Temp.AbroadMark = item.abroadMark.Trim();
                    alumnusmember_Temp.reply = item.replay.Trim();
                    _alumnusmemberTempRepository.Add(alumnusmember_Temp);

                    alumnusmember.idno = item.idno.Trim();
                    alumnusmember.name = item.name.Trim();
                    alumnusmember.sno1 = item.stuNo1.Trim();
                    alumnusmember.sex = item.sex.Trim();
                    alumnusmember.birthday = item.bdate.Trim();
                    alumnusmember.grp = item.group.Trim();
                    alumnusmember.unit = unit;
                    alumnusmember.subunit = subunit;
                    alumnusmember.gtype = gtype;
                    alumnusmember.addr = item.addr.Trim();
                    alumnusmember.tel = item.tel.Trim();
                    alumnusmember.sno2 = item.stuNo2.Trim();
                    alumnusmember.educode = item.educode.Trim();
                    alumnusmember.email = item.email1.Trim();
                    alumnusmember.graduateyear = item.graduationYear.Trim();
                    alumnusmember.valid = CheckEmail(item.email1.Trim());
                    alumnusmember.foreignermark = item.foreignermark.Trim();
                    alumnusmember.china_mark = item.chinaMark.Trim();
                    alumnusmember.AbroadMark = item.abroadMark.Trim();
                    alumnusmember.reply = item.replay.Trim();
                    _alumnusmemberRepository.Add(alumnusmember);
                    Save();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void GenerateDataFromRawData()
        {

        }
        public void GenerateMailGroupFile()
        {
            var data = _staffmemberRepository.GetAll();
            string lastfilename = "";
            if (Directory.Exists("data\\"))
            {
                Directory.Delete("D:\\getMailFile\\data\\", true);
            }
            Directory.CreateDirectory("D:\\getMailFile\\data\\");
            foreach (var item in data)
            {
            }
        }
        public void DeleteEduCode()
        {
            _eduCodeRepository.ExecuteSqlCommand("TRUNCATE TABLE educode");
            _eduCodeTempRepository.ExecuteSqlCommand("TRUNCATE TABLE educode_temp");
        }
        public void DeleteStaffMember()
        {
            _staffmemberRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember");
            _staffmemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember_temp");
        }
        public void DeleteStudentData()
        {
            _stuMemberRepository.ExecuteSqlCommand("TRUNCATE TABLE stumember");
            _stuMemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE stumember_temp");
        }
        public void DeleteAlumnusData()
        {
            _alumnusmemberRepository.ExecuteSqlCommand("TRUNCATE TABLE alumnusmember");
            _alumnusmemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE alumnusmember_temp");
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
