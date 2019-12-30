using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4500BioMetricApp.Model;
using System.Configuration;
using ZKFPEngXControl;

namespace _4500BioMetricApp.DAL
{
    public class EmployeeDAL
    {
        ZKFPEngX fp;
        public List<sms_employee> get_all_active_employees()
        {
            List<sms_employee> emp_list = new List<sms_employee>();

            //using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection_String"].ConnectionString))
            using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp as emp Inner Join sms_emp_title as title on emp.emp_title_id=title.id Inner join sms_emp_designation as designation on emp.emp_designation_id= designation.id  where emp.is_active='Y' Order by emp.emp_type_id ASC";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_employee emp = new sms_employee()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_father = Convert.ToString(reader["emp_father"].ToString()),
                                emp_nationality = Convert.ToString(reader["emp_nationality"].ToString()),
                                emp_religion = Convert.ToString(reader["emp_religion"].ToString()),
                                emp_exp = Convert.ToString(reader["emp_exp"].ToString()),
                                emp_cnic = Convert.ToString(reader["emp_cnic"].ToString()),
                                emp_qual = Convert.ToString(reader["emp_qual"].ToString()),
                                emp_sex = Convert.ToString(reader["emp_sex"].ToString()),
                                emp_marital = Convert.ToString(reader["emp_marital"].ToString()),
                                emp_dob = Convert.ToDateTime(reader["emp_dob"]),
                                joining_date = Convert.ToDateTime(reader["joining_date"]),
                                emp_email = Convert.ToString(reader["emp_email"].ToString()),
                                emp_address = Convert.ToString(reader["emp_address"].ToString()),
                                emp_remarks = Convert.ToString(reader["emp_remarks"].ToString()),
                                emp_pay = Convert.ToString(reader["emp_pay"].ToString()),
                                emp_cell = Convert.ToString(reader["emp_cell"].ToString()),
                                emp_phone = Convert.ToString(reader["emp_phone"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                title_id = Convert.ToInt32(reader["emp_title_id"].ToString()),
                                title = Convert.ToString(reader["title"].ToString()),
                                designation_id = Convert.ToInt32(reader["emp_designation_id"]),
                                designation = Convert.ToString(reader["designation"]),
                                image = (byte[])reader["image"],
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                            };

                            emp_list.Add(emp);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return emp_list;
        }
        public List<sms_emp_thumb_enroll> get_all_thumb_enroll()
        {
            fp = new ZKFPEngX();
            List<sms_emp_thumb_enroll> enroll_list = new List<sms_emp_thumb_enroll>();
            using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp_thumb_enroll as thumb Inner Join sms_emp as emp on emp.id=thumb.emp_id where emp.is_active='Y'";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_emp_thumb_enroll emp = new sms_emp_thumb_enroll()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                template = fp.DecodeTemplate1(reader["template"].ToString()),
                                image1 = (byte[])reader["image1"],
                                image2 = (byte[])reader["image2"],
                                image3 = (byte[])reader["image3"],
                                quality1 = Convert.ToInt32(reader["quality1"]),
                                quality2 = Convert.ToInt32(reader["quality2"]),
                                quality3 = Convert.ToInt32(reader["quality3"]),
                                hand = Convert.ToString(reader["hand"]),
                                finger_type_id = Convert.ToInt32(reader["finger_type_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                is_active = Convert.ToString(reader["is_active"]),
                                emp_login_id = Convert.ToInt32(reader["emp_login_id"])
                            };

                            enroll_list.Add(emp);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return enroll_list;
                }
            }
        }
        public sms_emp_thumb_enroll get_all_thumb_enroll_by_emp_id(int emp_id)
        {
            fp = new ZKFPEngX();
            sms_emp_thumb_enroll thumb = new sms_emp_thumb_enroll();
            using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp_thumb_enroll as thumb Inner Join sms_emp as emp on emp.id=thumb.emp_id where emp.is_active='Y' && emp_id=@emp_id";
                    cmd.Connection = con;
                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = emp_id;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            thumb = new sms_emp_thumb_enroll()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                template = fp.DecodeTemplate1(reader["template"].ToString()),
                                image1 = (byte[])reader["image1"],
                                image2 = (byte[])reader["image2"],
                                image3 = (byte[])reader["image3"],
                                quality1 = Convert.ToInt32(reader["quality1"]),
                                quality2 = Convert.ToInt32(reader["quality2"]),
                                quality3 = Convert.ToInt32(reader["quality3"]),
                                hand = Convert.ToString(reader["hand"]),
                                finger_type_id = Convert.ToInt32(reader["finger_type_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                is_active = Convert.ToString(reader["is_active"]),
                                emp_login_id = Convert.ToInt32(reader["emp_login_id"])
                            };                           
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return thumb;
                }
            }
        }
        public int insert_thumb_enroll(sms_emp_thumb_enroll enroll)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Delete from sms_emp_thumb_enroll where emp_id=@emp_id && hand=@hand && finger_type_id=@finger_type_id";
                                cmd.Connection = con;
                                cmd.Transaction = trans;

                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = enroll.emp_id;
                                cmd.Parameters.Add("@hand", MySqlDbType.VarChar).Value = enroll.hand;
                                cmd.Parameters.Add("@finger_type_id", MySqlDbType.Int32).Value = enroll.finger_type_id;

                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            }
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_emp_thumb_enroll(emp_id, template, image1, image2, image3, quality1,quality2, quality3, hand, finger_type_id,created_by, emp_login_id, date_time) Values(@emp_id, @template, @image1, @image2, @image3, @quality1,@quality2, @quality3, @hand, @finger_type_id,@created_by, @emp_login_id, @date_time)";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = enroll.emp_id;
                                cmd.Parameters.Add("@template", MySqlDbType.Text).Value = enroll.templateStr;
                                cmd.Parameters.Add("@image1", MySqlDbType.Blob).Value = enroll.image1;
                                cmd.Parameters.Add("@image2", MySqlDbType.Blob).Value = enroll.image2;
                                cmd.Parameters.Add("@image3", MySqlDbType.Blob).Value = enroll.image3;
                                cmd.Parameters.Add("@quality1", MySqlDbType.Int32).Value = enroll.quality1;
                                cmd.Parameters.Add("@quality2", MySqlDbType.Int32).Value = enroll.quality2;
                                cmd.Parameters.Add("@quality3", MySqlDbType.Int32).Value = enroll.quality3;
                                cmd.Parameters.Add("@hand", MySqlDbType.VarChar).Value = enroll.hand;
                                cmd.Parameters.Add("@finger_type_id", MySqlDbType.Int32).Value = enroll.finger_type_id;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = enroll.created_by;
                                cmd.Parameters.Add("@emp_login_id", MySqlDbType.Int32).Value = enroll.emp_login_id;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = enroll.date_time;


                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                trans.Commit();
                                con.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return i;
        }

        public string get_last_attendance_mode (int emp_id)
        {
            string mode = "";
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT mode FROM sms_emp_attendance where emp_id=@emp_id order by date_time DESC Limit 1";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = emp_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                       
                            con.Open();

                            MySqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                mode = reader["mode"].ToString();
                            }
                                               
                    }
                }
            }catch(Exception ex)
            { throw ex; }
            return mode;
        }

        public int insert_thumb_attendance(sms_emp_attendance attendance)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
                {
                    con.Open();
                    
                                       
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_emp_attendance(emp_id, mode,created_by, emp_login_id, date_time) Values(@emp_id, @mode,@created_by, @emp_login_id, @date_time)";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = attendance.emp_id;
                                cmd.Parameters.Add("@mode", MySqlDbType.VarChar).Value = attendance.mode;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = attendance.created_by;
                                cmd.Parameters.Add("@emp_login_id", MySqlDbType.Int32).Value = attendance.emp_login_id;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = attendance.date_time;


                                i = Convert.ToInt32(cmd.ExecuteNonQuery());                    
                                con.Close();
                            }                    
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return i;
        }

        public List<sms_emp_attendance> get_all_attendance_by_date(DateTime dt)
        {
           List<sms_emp_attendance> att_list = new List<sms_emp_attendance>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(ConnectionString.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT * FROM sms_emp_attendance as att inner join sms_emp as emp on att.emp_id=emp.id where Date(att.date_time)=@date order by att.date_time DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();

                       while(reader.Read())
                       {
                           sms_emp_attendance att = new sms_emp_attendance()
                           {
                               id = Convert.ToInt32(reader["id"]),
                               emp_id = Convert.ToInt32(reader["emp_id"]),
                               mode = reader["mode"].ToString(),
                               date_time = Convert.ToDateTime(reader["date_time"]),
                               emp_login_id = Convert.ToInt32(reader["emp_login_id"]),
                               image = (byte[])reader["image"],
                               emp_name = reader["emp_name"].ToString(),
                               created_by = reader["created_by"].ToString(),
                           };
                           att_list.Add(att);
                       }

                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            return att_list;
        }
        
    }
}