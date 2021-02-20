using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    class Studmng
    {
        private connect conobj;
        private string strQuery;
        SqlCommand cmd;
        private string strTableName = "Student",names,places,teacher,answer;
        int sid,fid;
        private Account acc;
        private Student stu;

        public Studmng()
        {
            conobj = connect.getconnect();
            acc = new Account();
            stu = new Student();
        }

        public Student get(int id)
        {
            Student obj = null;
            strQuery = "select * from " + strTableName + " where stu_id = " + id;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                obj = new Student();
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    obj.ID = (int)dr["stu_id"];
                    obj.Name = (string)dr["stu_name"];
                    obj.Email = (string)dr["stu_email"];
                    obj.Semester = (int)dr["stu_sem"];
                    obj.Program = (int)dr["program_id"];
                    obj.Age = (DateTime)dr["stu_age"];
                    obj.Gender = (string)dr["stu_gen"];
                    obj.Image = (byte[])dr["stu_img"];               
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

        public void Update(int id, Student obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update " + this.strTableName + " set stu_name=@name, stu_email=@email, stu_age=@age, stu_gen=@gen, stu_img=@img where stu_id=" + id;
                cmd = conobj.execute(this.strQuery);
                cmd.Parameters.AddWithValue("@name", obj.Name);
                cmd.Parameters.AddWithValue("@email", obj.Email);
                cmd.Parameters.AddWithValue("@age", obj.Age);
                cmd.Parameters.AddWithValue("@gen", obj.Gender);
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

        public Tuple<string, string> forgetpass(int id)
        {
            bool flag = false;
            strQuery = "select user_pass from account where stu_id=" + id;
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
                strQuery = "select stu_email from Student where stu_id=" + id;
                cmd = conobj.execute(this.strQuery);
                SqlDataReader dr1;

                try
                {
                    dr1 = cmd.ExecuteReader();
                    while (dr1.Read())
                    {
                        stu = new Student();
                        stu.Email = (string)dr1["stu_email"];
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

            return new Tuple<string, string>(stu.Email, acc.Password);
        }

        public void Updatepass(int id, Account obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "update account set user_pass = '" + obj.Password + "' where stu_id = " + id + "; ";
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

        public Tuple<int,int> getsubmitassign(int id)
        {
            strQuery = "select * from assignment_submit where stu_id=" + id + "and fac_id="+fid;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                   sid = dr.GetInt32(5);
                   fid = dr.GetInt32(4);
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

            return new Tuple<int, int>(sid,fid);
        }

        public List<Assignment_submit> checkmarks(int id)
        {
            Assignment_submit asb;
            List<Assignment_submit> mylist = new List<Assignment_submit>();
            strQuery = "select * from assignment_submit where stu_id=" + id;
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                cmd = conobj.execute(this.strQuery);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    asb = new Assignment_submit();
                    asb.Date = dr.GetDateTime(1);
                    asb.Marks = dr.GetDouble(2);
                    asb.Answer = dr.GetString(3);
                    asb.Fac_id = dr.GetInt32(4);
                    asb.ID = dr.GetInt32(5);
                    mylist.Add(asb);
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


        public void updatemarks(int id,Assignment_submit ass)
        {           
            try
            {
                conobj.openconnect();
                strQuery = "update assignment_submit set marks=" + ass.Marks + " where stu_id=" + id;
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
        
        public List<string> teachernames(int id)
        {
            List<string> mylist = new List<string>();
            strQuery = "select fac_name from faculty where fac_id in (select fac_id from teaches where stu_id=" + id + ");";           
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

        public void Add(Assignment_submit obj)
        {
            try
            {
                conobj.openconnect();
                strQuery = "insert into assignment_submit (sub_date,marks,answer,fac_id,stu_id) values ('"+obj.Date+"',"+0.1+",'"+obj.Answer+"',"+obj.Fac_id+","+obj.ID+");";
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


        public Tuple<string,string> getassignments(int teacherid)
        {
            strQuery = "select assign_name,assign_question,fac_id from assignment where fac_id="+teacherid;
            cmd = conobj.execute(this.strQuery);
            SqlDataReader dr;
            try
            {
                conobj.openconnect();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {                 
                    places = dr.GetString(0);
                    teacher = dr.GetString(1);
                    fid = dr.GetInt32(2);                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return new Tuple<string,string> (teacher,places);
        }

        public List<Course> getcourse(int id)
        {
            Course rec;
            List<Course> mylist = new List<Course>();
            strQuery = "select course.cour_id,course.cour_name,faculty.fac_id,faculty.fac_name from teaches,course,faculty where stu_id="+id+" and teaches.cour_id=course.cour_id and teaches.fac_id=faculty.fac_id;";
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
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return mylist;
        }

        public string getresult(int id, int stuid)
        {
            try{
                conobj.openconnect();
                strQuery = "select answer from assignment_submit where fac_id=" + id + " and stu_id=" + stuid;
                cmd = conobj.execute(strQuery);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    answer = dr.GetString(0);
                }
            }catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                conobj.closeconnect();
            }
            return answer;
       }
    }
}
