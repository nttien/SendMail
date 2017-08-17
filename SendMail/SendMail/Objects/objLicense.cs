using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMail
{
    public class objLicense
    {
        public string email { set; get; }
        public string userName { set; get; }
        public string password { set; get; }
        public string key { set; get; }
        public string hardwareId { set; get; }
        public DateTime expDate { set; get; }
        public int countDate { set; get; }
        public bool status { set; get; }
    }
}
