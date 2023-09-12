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

namespace NTUST.BulkMail.Services
{
    public class BulkMailService : IBulkMailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaffmemberTempRepository _staffmemberTempRepository;

        public BulkMailService(IUnitOfWork unitOfWork,
            IStaffmemberTempRepository staffmemberTempRepository) {
            _unitOfWork = unitOfWork;
            _staffmemberTempRepository = staffmemberTempRepository;
        }

        public void CreateStaffMemberTemp(List<member> member)
        {
            staffmember_temp staffmember_Temp = new staffmember_temp();
            foreach(var item in member)
            {
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
        }
        public void DeleteStaffMemberTemp()
        {
            var data = _staffmemberTempRepository.GetAll();
            foreach (var item in data)
            {
                _staffmemberTempRepository.Delete(item);
            }
            Save();
            
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
