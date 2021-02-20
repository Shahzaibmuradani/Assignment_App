using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Project1
{
    class Adminmng
    {
        private connect conobj;
        private string strQuery;
        private string names;
        private Account acc;
        private SqlCommand cmd;
        private Admin add;
        string desc;
        int salary;

        public Adminmng()
        {
            conobj = connect.getconnect();
            acc =  new Account();
            add = new Admin();
        }

        public List<string> teachernames()
        {
            List<string> mylist = new List<string>();
            strQuery = "select fac_name from faculty";
            cmd = conobj.execute(strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    names = dr.GetString(0);
                    mylist.Add(names);
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
            return mylist;
        }

        public List<Course> getcourse()
        {
            Course rec;
            List<Course> mylist = new List<Course>();
            strQuery = "select fac_id,fac_name,faculty.cour_id,course.cour_name from faculty,course where faculty.cour_id=course.cour_id;";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;

            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    rec = new Course();
                    rec.Course_ID = dr.GetInt32(0);
                    rec.Course_Name = dr.GetString(1);
                    rec.Teacher_ID = dr.GetInt32(2);
                    rec.Teacher_Name = dr.GetString(3);
                    mylist.Add(rec);
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
            return mylist;
        }

        public List<Pro> getprogram()
        {
            List<Pro> mylist = new List<Pro>();
            Pro obj = null;
            strQuery = "select pro_id,program_name,sem_num,sem_name from program,semester where semester.sem_id=program.sem_id;";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj = new Pro();
                    obj.Program_id = dr.GetInt32(0);
                    obj.Program_Name = dr.GetString(1);
                    obj.Semester_Number = dr.GetInt32(2);
                    obj.Semester_Name = dr.GetString(3);
                    mylist.Add(obj);
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

            return mylist;
        }

        public List<students> getStudent()
        {
            students rec;
            List<students> list = new List<students>();
            strQuery = "select stu_id,stu_name from Student;";
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rec = new students();
                    rec.ID = (int)dr["stu_id"];
                    rec.Name = (string)dr["stu_name"];
                    list.Add(rec);
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
            return list;
        }

        public Tuple<string, string> forgetpass(int id)
        {
            bool flag = false;
            strQuery = "select user_pass from account where admin_id=" + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;

            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    acc = new Account();
                    acc.Password = (string)dr["user_pass"];
                    flag = true;
                }
                dr.Close();

                if (!flag)
                {
                    throw new Exception("Invalid Id");
                }

                flag = false;
                strQuery = "select admin_email from admin where admin_id=" + id;
                cmd = conobj.execute(this.strQuery);
                SqlDataReader dr1;

                try
                {
                    dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        add = new Admin();
                        add.Email = (string)dr1["admin_email"];
                        flag = true;
                    }
                    dr1.Close();
                    if (!flag)
                    {
                        throw new Exception("Invalid Id");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
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

            return new Tuple<string, string>(add.Email,acc.Password);
        }

        public bool verifyadmin(int id,string password)
        {
            bool flag = false;
            strQuery = "select * from account where admin_id="+id+" and user_pass='"+password+"'";
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
                    acc.Admin_id = dr.GetInt32(2);
                    flag = true;
                }
                if (!flag)
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

        public void Update(int id, Faculty obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update faculty set fac_designation = '" + obj.Designation + "', fac_salary = " + obj.Salary+ " where fac_id = " + id + "; ";
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = System.Data.CommandType.Text;
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

        public void Delete(int id)
        {
            try
            {
                conobj.openconnect();
                strQuery = "delete from assignment where fac_id = " + id;
                cmd = conobj.execute(strQuery);
                cmd.CommandType = System.Data.CommandType.Text;
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

            public Admin get(int id)
            {
            Admin obj = null;
            strQuery = "select * from admin where admin_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj = new Admin();
                    obj.ID = (int)dr["admin_id"];
                    obj.Name = (string)dr["admin_name"];
                    obj.Image = (byte[])dr["admin_img"];
            
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

        public void UpdatePass(int id, Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update account set user_pass = '" + obj.Password + "' where admin_id = " + id + "; ";
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = System.Data.CommandType.Text;
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

            public void UpdateName(int id, Admin obj)
            {
                try
                {
                    conobj.openconnect();
                    strQuery = "update admin set admin_name=@name,admin_img=@img where admin_id = " + id;
                    cmd = conobj.execute(this.strQuery);                
                    cmd.Parameters.AddWithValue("@name", obj.Name);
                    cmd.Parameters.AddWithValue("@img", obj.Image);
                    cmd.CommandType = System.Data.CommandType.Text;
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

        public Tuple<string,int > getdetails(int teacherid)
        {
            strQuery = "select fac_designation,fac_salary from faculty where fac_id=" + teacherid;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    desc = dr.GetString(0);
                    salary = dr.GetInt32(1);
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
            return new Tuple<string, int>(desc, salary);
        }

    }
}