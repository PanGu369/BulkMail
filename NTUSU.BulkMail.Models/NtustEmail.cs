using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Models
{
    public class NtustEmail
    {
        public string sender { get; set; }
        public List<mailReciver> receiver { get; set; }
        public string txtreceiver { get; set; }
        public string subject { get; set; }
        public string email_content { get; set; }
        public System.DateTime dt { get; set; }
    }
    public class mailReciver
    { 
        public string cname { get; set; }
        public string name { get; set; }
        public string mail { get; set; }
        public string groupmail { get; set; }
    }
}
