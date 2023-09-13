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
        void CreateStaffMemberTemp(string id, string semester);
        void CreateStaffMember();
        void DeleteEduCode();
        void DeleteStaffMemberTemp();
        void DeleteStaffTemp();
        IEnumerable<member> GetStaffMemberData();
        void Save();
    }
}
