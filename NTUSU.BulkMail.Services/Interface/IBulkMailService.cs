﻿using NTUST.BulkMail.EntityFramework;
using NTUST.BulkMail.Models;
using NTUSU.BulkMail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Services.Interface
{
    public interface IBulkMailService
    {
        void CreateEduCode();
        void CreateStaffMember(string id, string semester);
        void CreateStudentData(string pnowcondition);
        void CreateAlumnusData();
        void CreateSendBigMailBulletinBoardNew(string dtStart, string dtEnd);
        void GenerateDataFromRawData();
        void GenerateMailGroupFile();
        void GenerateAliasesFile();
        void DeleteEduCode();
        void DeleteStaffMember();
        void DeleteStudentData();
        IEnumerable<member> GetStaffMemberData();
        IEnumerable<unitcode> GetUnitcodesDataList();
        unitcode GetUnitcodesData(string tunitcode, string unitcode);
        IEnumerable<MailGroupViewModal> GetMailGroup();
        List<MailGroupListViewModal> GetMailGroupList(string groupName);
        List<MailGroupListViewModal> GetMailGroupName(string groupName);
        void UpdateUnicodeData(UnicodeViewModal unitcode);
        void CreateUnicodeData(UnicodeViewModal unitcodeObject);
        void DeleteUnicodeData(string tunitcode, string unitcode);
        void Save();
        List<LostUnitCode> GetLostUnitCode();
        List<LostStaffClassTitleCode> GetLostStaffClassTitleCode();
    }
}
