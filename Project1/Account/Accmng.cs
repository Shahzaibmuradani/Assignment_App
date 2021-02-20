using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    class Accmng
    {
        private connect conobj;
        private string strQuery;
        private string strTable = "account";
        private SqlCommand cmd;
        private Account acc;
        private Facmng facmng;
        private string user;

        public Accmng()
        {
            facmng = new Facmng();
            acc = new Account();
            conobj = connect.getconnect();
        }

        public void createkey(int id)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("MyRegKey");
            key.SetValue(Convert.ToString("currentuser"), id);
            key.Close();
        }

        public string readkey()
        {

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("MyRegKey");
            if (key != null)
            {
                user = key.GetValue("currentuser").ToString();
            }
            return user;
        }

        public void email()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("youremail@gmail.com");
            foreach (var list in facmng.Email(Convert.ToInt32(readkey())))
            {
                mail.To.Add(list);
            }
            mail.Subject = "alert";
            mail.Body = "Assignment has been Uploaded";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("youremail@gmail.com", "yourpassword");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            MessageBox.Show("Mail sent", "Success", MessageBoxButtons.OK);
        }

        public void emailpass(string email, string pass)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("shahzaibsherali007@gmail.com");
            mail.To.Add(email);
            mail.Subject = "Recovery of Password";
            mail.Body = pass;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("shahzaibsherali007@gmail.com", "muradani101");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
            MessageBox.Show("Password will Be Sent via Email", "Success", MessageBoxButtons.OK);
        }

        public void fac_acc(Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "insert into " + this.strTable + " (user_pass,admin_id,fac_id,stu_id,pro_id,status) values (@pass,@admin,@fac,@stu,@pro,@status);";
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pass", obj.Password);
                cmd.Parameters.AddWithValue("@admin", DBNull.Value);
                cmd.Parameters.AddWithValue("@fac", obj.Fac_id);
                cmd.Parameters.AddWithValue("@stu", DBNull.Value);
                cmd.Parameters.AddWithValue("@pro", DBNull.Value);
                cmd.Parameters.AddWithValue("@status", 1);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
        }

        public void fac_pro(Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "insert into " + this.strTable + " (user_pass,admin_id,fac_id,stu_id,pro_id,status) values (@pass,@admin,@fac,@stu,@pro,@status);";
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@pass", obj.Password);
                cmd.Parameters.AddWithValue("@admin", DBNull.Value);
                cmd.Parameters.AddWithValue("@fac", DBNull.Value);
                cmd.Parameters.AddWithValue("@stu", DBNull.Value);
                cmd.Parameters.AddWithValue("@pro", obj.Pro_id);
                cmd.Parameters.AddWithValue("@status", 1);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
        }

        public bool verifyfac(int id,string password)
        {
            bool flag = false;
            strQuery = "select * from account where fac_id = " + id + " and user_pass = '" +password+ "';";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    acc = new Account();
                    acc.Password = dr.GetString(1);
                    acc.Fac_id = dr.GetInt32(3);
                    flag = true;
                }

                if(!flag)
                {
                    throw new Exception("Invalid Password");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return flag;
        }

        public bool verifystu(int id,string password)
        {
            bool flag = false;
            strQuery = "select * from " + strTable + " where stu_id = " + id + " and user_pass = '" + password + "';";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    acc.Password = dr.GetString(1);
                    acc.Stu_id = dr.GetInt32(4);
                    flag = true;
                }

                if(!flag)
                {
                    throw new Exception("Invalid Password");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return flag;
        }

        public Account getfacpass(int id)
        {
            Account obj = null;
            strQuery = "select * from " + strTable + " where fac_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Account();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Password = dr.GetString(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return obj;
        }

        public Account getstupass(int id)
        {
            Account obj = null;
            strQuery = "select * from " + strTable + " where stu_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Account();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Password = dr.GetString(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return obj;
        }
        public Account getadminpass(int id)
        {
            Account obj = null;
            strQuery = "select * from " + strTable + " where admin_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Account();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Password = dr.GetString(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return obj;
        }

        public Account getpropass(int id)
        {
            Account obj = null;
            strQuery = "select * from " + strTable + " where pro_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Account();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Password = dr.GetString(1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return obj;
        }

        public string annoucements()
        {
            string line;
            string path = @"C:\Users\Shahzaib\source\repos\Project1\Important.txt";
            StreamReader sr = new StreamReader(path);

            line = sr.ReadToEnd();
            string[] a = line.Split('\n');
            sr.Close();

            return line;

        }
    }
}
