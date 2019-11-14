using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4500BioMetricApp.Model
{
    public class sms_emp_attendance
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public string mode { get; set; }
        public DateTime date_time { get; set; }
        public string created_by { get; set; }
        public int emp_login_id { get; set; }

        public byte[] image { get; set; }
        public string emp_name { get; set; }

    }
}
