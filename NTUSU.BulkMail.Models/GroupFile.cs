using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Models
{
    public class GroupFile
    {
        public string mail { get; set; }
        public string filename { get; set; }
        public string member { get; set; }
    }

    public class MailList
    {
        public string cname { get; set; }
        public string mail { get; set; }
        public string filname { get; set; }
        public string member { get; set; }
        public string membername { get; set; }
    }
}
