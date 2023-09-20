using NTUST.BulkMail.EntityFramework;
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
        void GenerateDataFromRawData();
        void GenerateMailGroupFile();
        void DeleteEduCode();
        void DeleteStaffMember();
        void DeleteStudentData();
        IEnumerable<member> GetStaffMemberData();
        IEnumerable<unitcode> GetUnitcodesData();
        void Save();
    }
}
