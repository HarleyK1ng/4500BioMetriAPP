using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _4500BioMetricApp.Model
{
    public class sms_employee
    {
        public string id { set; get; }      
        public string emp_name { set; get; }
        public string emp_father { set; get; }
        public string emp_nationality { set; get; }
        public string emp_religion { set; get; }
        public string emp_exp { set; get; }
        public string emp_cnic { set; get; }
        public string emp_qual { set; get; }
        public string emp_sex { set; get; }
        public string emp_marital { set; get; }
        public DateTime emp_dob { set; get; }
        public string emp_email { set; get; }
        public string emp_address { set; get; }
        public string emp_remarks { set; get; }
        public string emp_pay { set; get; }
        public string emp_cell { set; get; }
        public string emp_phone { set; get; }
        public string emp_type_id { set; get; }
        public string emp_type { set; get; }
        public DateTime date_time { set; get; }
        public DateTime joining_date { set; get; }
        public DateTime leaving_date { set; get; }
        public string created_by { set; get; }
        public string is_active { set; get; }
        public List<char> att_lst { get; set; }
        public List<DateTime> att_date_lst { get; set; }
        public char attendence { set; get; }
        public char[] attendence_arr = new char[100];
        public DateTime attendence_date { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public string sms_message { set; get; }
        public string sms_type { set; get; }
        public bool Checked { set; get; }
        public int title_id { get; set; }
        public string title { get; set; }
        public int designation_id { get; set; }
        public string designation { get; set; }
        public byte[] image { get; set; }

        public Geometry icon { get; set; }


        public bool Active;

        private void ReportChange(string propertyName)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        //For Report
        public double deduction_amount { get; set; }
        public int total_absents { get; set; }
        public int total_days { get; set; }
    }
}
