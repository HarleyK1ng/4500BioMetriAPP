using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4500BioMetricApp.Model
{
    public class sms_emp_thumb_enroll
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public byte[] template { get; set; }
        public string templateStr { get; set; }
        public byte[] image1 { get; set; }
        public byte[] image2 { get; set; }
        public byte[] image3 { get; set; }
        public int quality1 { get; set; }
        public int quality2 { get; set; }
        public int quality3 { get; set; }
        public int finger_type_id { get; set; }
        public string hand { get; set; }
        public DateTime date_time { get; set; }
        public string created_by { get; set; }
        public int emp_login_id { get; set;        }
        public string is_active { get; set; }
     }
}
