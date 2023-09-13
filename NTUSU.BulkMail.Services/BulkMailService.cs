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

namespace NTUST.BulkMail.Services
{
    public class BulkMailService : IBulkMailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffmemberRepository _staffmemberRepository;
        private readonly IStaffmemberTempRepository _staffmemberTempRepository;

        public BulkMailService(IUnitOfWork unitOfWork,
            IStaffmemberRepository staffmemberRepository,
            IStaffmemberTempRepository staffmemberTempRepository) {
            _unitOfWork = unitOfWork;
            _staffmemberRepository = staffmemberRepository;
            _staffmemberTempRepository = staffmemberTempRepository;
        }

        public void CreateEduCode()
        {

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
            catch(Exception ex)
            {

            }
        }
        public void DeleteEduCode()
        {

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
    }
}
