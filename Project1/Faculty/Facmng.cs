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
    class Facmng
    {
        private connect conobj;
        private string strQuery,files;
        SqlCommand cmd;
        private string strTableName = "faculty";
        int fid;
        private Account acc;
        private Faculty fac;

        public Array qualification()
        {
            string[] a = new string[3] { "Bachelors", "Masters", "Phd" };
            return a;
        }

        public Array gender()
        {
            string[] a = new string[2] { "Male", "Female"};
            return a;
        }


        public Facmng()
        {
            conobj = connect.getconnect();
            fac = new Faculty();
            acc = new Account();
        }

        public int getfac(string name)
        {
            strQuery = "select fac_id from " +this.strTableName+ " where fac_name='" + name + "'";
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fid = (int)dr["fac_id"];
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

            return fid;
        }

        public void Add(Assignment obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "insert into assignment (fac_id, cour_id, due_date,assign_name,assign_question,p_path) values (" + obj.ID + "," + obj.Course_id + ", '" + obj.Due_date + "','"+obj.Name+"','"+obj.Question+"','"+obj.Previous+"')";
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
        }

        public void updateassign(int id,Assignment ass)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update assignment set due_date='"+ass.Due_date+"',assign_name='"+ass.Name+"',assign_question='"+ass.Question+"',p_path='"+ass.Previous+ "' where fac_id=" + id;
                cmd = conobj.execute(this.strQuery);
                cmd.CommandType = CommandType.Text;
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

        public Tuple<string, string> forgetpass(int id)
        {
            bool flag = false;
            strQuery = "select user_pass from account where fac_id=" + id;
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
                    flag = true;
                }
                dr.Close();

                if (!flag)
                {
                    throw new Exception("Invalid Id");
                }

                flag = false;
                strQuery = "select fac_email from faculty where fac_id=" + id;
                cmd = conobj.execute(this.strQuery);
                SqlDataReader dr1;

                try
                {
                    dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        fac = new Faculty();
                        fac.Email = (string)dr1["fac_email"];
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

            return new Tuple<string, string>(fac.Email, acc.Password);
        }


        public void Updatepass(int id, Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update account set user_pass = '" + obj.Password + "' where fac_id = " + id + "; ";
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

        public void Update(int id, Faculty obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update " + this.strTableName + " set fac_name=@name, fac_email=@email, fac_qualification=@qua,fac_age=@age,fac_gen=@gen,fac_img=@img where fac_id=" + id;
                cmd = conobj.execute(this.strQuery);
                cmd.Parameters.AddWithValue("@name", obj.Name);
                cmd.Parameters.AddWithValue("@email",obj.Email);
                cmd.Parameters.AddWithValue("@qua",obj.Qualification);
                cmd.Parameters.AddWithValue("@age",obj.Date);
                cmd.Parameters.AddWithValue("@gen",obj.Gender);
                cmd.Parameters.AddWithValue("@img",obj.Image);
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

        public void Delete_assign(int id)
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

        public Assignment getassign(int id)
        {
            Assignment ass = null;
            strQuery = "select * from assignment where fac_id=" + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    ass = new Assignment();
                    ass.ID = dr.GetInt32(1);
                    ass.Course_id = dr.GetInt32(2);
                    ass.Due_date = dr.GetDateTime(3);
                    ass.Name = dr.GetString(4);
                    ass.Question = dr.GetString(5);
                    ass.Previous = dr.GetString(6);
                }
            }
            catch ( Exception ex)
            {
                throw new Exception(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }

            return ass;
        }

        public Faculty get(int id)
        {
            Faculty obj = null;
            strQuery = "select * from " +this.strTableName+" where fac_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {               
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj = new Faculty();
                    obj.ID = (int)dr["fac_id"];
                    obj.Name = (string)dr["fac_name"];
                    obj.Qualification = (string)dr["fac_qualification"];
                    obj.Email = (string)dr["fac_email"];
                    obj.Date = (DateTime)dr["fac_age"];
                    obj.Course = (int)dr["cour_id"];
                    obj.Program = (int)dr["program_id"];
                    obj.Designation = (string)dr["fac_designation"];
                    obj.Gender = (string)dr["fac_gen"];
                    obj.Salary = (int)dr["fac_salary"];
                    obj.Image = (byte[])dr["fac_img"];

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

        public List<Assignment_submit> getmarks(int id)
        {
            Assignment_submit rec;
            List<Assignment_submit> list = new List<Assignment_submit>();
            strQuery = "select * from assignment_submit where fac_id="+id;
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rec = new Assignment_submit();

                    rec.ID = dr.GetInt32(0);
                    rec.Date = dr.GetDateTime(1);
                    rec.Marks = dr.GetDouble(2);
                    rec.Answer = dr.GetString(3);
                    rec.Fac_id = dr.GetInt32(4);
                    rec.ID = dr.GetInt32(5);
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


        public List<string> getresult(int id)
        {
            List<string> list = new List<string>();
            strQuery = "select answer from assignment_submit where fac_id=" + id;
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    files = dr.GetString(0);
                    list.Add(files);
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


        public List<string> Email(int id)
        {
            List<string> emaillist = new List<string>();
            Student rec;
            strQuery = "select stu_email from Student where stu_id in (select stu_id from teaches where fac_id=" + id + ");";
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    rec = new Student();
                    rec.Email = (string)dr["stu_email"];
                    emaillist.Add(rec.Email);
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
            return emaillist;
        }

        public List<students> getAll(int id)
        {
            students rec;
            List<students> list = new List<students>();
            strQuery = "select stu_id,stu_name from Student where stu_id in (select stu_id from teaches where fac_id=" + id + ");";
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


        public List<int> studentids(int id)
        {
            List<int> mylist = new List<int>();
            strQuery = "select stu_id from assignment_submit where fac_id=" + id;
            cmd = conobj.execute(strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    id = dr.GetInt32(0);
                    mylist.Add(id);
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
    }
}
