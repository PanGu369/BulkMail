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

namespace NTUST.BulkMail.Services
{
    public class BulkMailService : IBulkMailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffmemberRepository _staffmemberRepository;
        private readonly IStaffmemberTempRepository _staffmemberTempRepository;
        private readonly IEduCodeRepository _eduCodeRepository;
        private readonly IEduCodeTempRepository _eduCodeTempRepository;

        public BulkMailService(IUnitOfWork unitOfWork,
            IStaffmemberRepository staffmemberRepository,
            IStaffmemberTempRepository staffmemberTempRepository,
            IEduCodeRepository eduCodeRepository,
            IEduCodeTempRepository eduCodeTempRepository)
        {
            _unitOfWork = unitOfWork;
            _staffmemberRepository = staffmemberRepository;
            _staffmemberTempRepository = staffmemberTempRepository;
            _eduCodeRepository = eduCodeRepository;
            _eduCodeTempRepository = eduCodeTempRepository;
        }

        public void CreateEduCode()
        {
            DeleteEduCode();
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
        public void CreateStaffMemberTemp(string id, string semester)
        {
            DeleteStaffMemberTemp();
            staffmember_temp staffmember_Temp = new staffmember_temp();
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
                    _staffmemberTempRepository.Add(staffmember_Temp);
                    Save();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void CreateStaffMember()
        {
            DeleteStaffTemp();
            staffmember staffmember = new staffmember();
            var query = _staffmemberTempRepository.GetAll();
            try
            {
                foreach (var item in query)
                {
                    if (item.title != "副校長")
                    {
                        if (item.name == "談家珍" || item.title == "四等技術師(B)" || item.title == "四等技術師(A)")
                        {
                            item.title = "技術師";
                        }
                        if (item.@class == "正式" && item.title == "助教")
                        {
                            item.title = "助理教授";
                        }
                        staffmember.name = item.name.Trim();
                        staffmember.idno = item.idno.Trim();
                        staffmember.unitcode = item.unitcode.Trim();
                        staffmember.unicode = item.unicode.Trim();
                        staffmember.@class = item.@class.Trim();
                        staffmember.email = item.email.Trim();
                        staffmember.tel = item.tel.Trim();
                        staffmember.unit = item.unit.Trim();
                        staffmember.title = item.title.Trim();
                        staffmember.kind = item.kind.Trim();
                        _staffmemberRepository.Add(staffmember);
                        Save();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void DeleteEduCode()
        {
            _eduCodeRepository.ExecuteSqlCommand("TRUNCATE TABLE educode");
            _eduCodeTempRepository.ExecuteSqlCommand("TRUNCATE TABLE educode_temp");
        }
        public void DeleteStaffMemberTemp()
        {
            _staffmemberTempRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember_temp");
        }
        public void DeleteStaffTemp()
        {
            _staffmemberRepository.ExecuteSqlCommand("TRUNCATE TABLE staffmember");
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
