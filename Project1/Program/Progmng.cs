using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    class Progmng
    {
        private connect conobj;
        private string strQuery;
        SqlCommand cmd;
        string strTable = "account";
        private Account acc;
        private Faculty pro;

        public Progmng()
        {
            acc = new Account();
            pro = new Faculty();
            conobj = connect.getconnect();
        }

        public Pro get(int id)
        {
            Pro obj = null;
            strQuery = "select pro_id,program_name,sem_num,sem_name from program,semester where program.pro_id="+id+"and semester.sem_id=program.sem_id;";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Pro();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.Program_id = dr.GetInt32(0);
                    obj.Program_Name = dr.GetString(1);
                    obj.Semester_Number = dr.GetInt32(2);
                    obj.Semester_Name = dr.GetString(3);
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

        public List<Course> getcourse(int id)
        {
            Course rec;
            List<Course> mylist = new List<Course>();
            strQuery = "select course.cour_id,cour_name,fac_id,fac_name from faculty,course where faculty.cour_id=course.cour_id and course.cour_id in (select cour_id from teaches where teaches.program_id="+id+");";
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

        public bool verifyprogram(int id,string password)
        {
            bool flag = false;
            strQuery = "select * from " + strTable + " where pro_id = " +id+ " and user_pass = '" + password + "';";
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
                    acc.Pro_id = dr.GetInt32(5);
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

        public Tuple<string, string> forgetpass(int id)
        {
            bool flag = false;
            strQuery = "select user_pass,fac_id from account where pro_id=" + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;

            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    acc = new Account();
                    acc.Password = (string)dr["user_pass"];
                    acc.Fac_id = (int)dr["fac_id"];
                    flag = true;
                }
                dr.Close();

                if (!flag)
                {
                    throw new Exception("Invalid Id");
                }

                flag = false;
                strQuery = "select fac_email from faculty where fac_id=" + acc.Fac_id;
                cmd = conobj.execute(this.strQuery);
                SqlDataReader dr1;

                try
                {
                    dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        pro = new Faculty();
                        pro.Email = (string)dr1["fac_email"];
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

            return new Tuple<string, string>(pro.Email, acc.Password);
        }


        public void Update(int id, Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update " + this.strTable + " set user_pass = '" + obj.Password + "' where pro_id = " + id + "; ";
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

        public List<students> getAll(int id)
        {
            students rec;
            List<students> list = new List<students>();
            strQuery = "select stu_id,stu_name from Student where stu_id in (select stu_id from teaches where program_id="+id+");";
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rec = new students();

                    rec.ID = dr.GetInt32(0);
                    rec.Name = dr.GetString(1);
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
    }
}
