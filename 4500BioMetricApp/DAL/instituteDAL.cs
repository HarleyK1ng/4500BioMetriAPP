using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4500BioMetricApp.Model;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace _4500BioMetricApp.DAL
{
    class InstituteDAL
    {
        public sms_institute get_sms_institute()
        {
            sms_institute ins = new sms_institute();

            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection_String"].ConnectionString))
            { 
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_institute";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();

                    Byte[] institute_logo;
                    Byte[] male_image;
                    Byte[] female_image;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    institute_logo = (byte[])(reader["institute_logo"]);
                    male_image = (byte[])(reader["male_image"]);
                    female_image = (byte[])(reader["female_image"]);


                    ins = new sms_institute()
                    {
                        institute_name = Convert.ToString(reader["institute_name"].ToString()),
                        institute_logo = institute_logo,
                        male_image = male_image,
                        female_image = female_image,
                        mac = Convert.ToString(reader["mac"].ToString()),
                        isMultiPartSMSAccess = Convert.ToString(reader["isMultiPartSMSAccess"]),
                    };

                }
                catch (Exception ex)
                {
                    throw ex;   
                }
                return ins;
            }
        }
        }
    }
}
