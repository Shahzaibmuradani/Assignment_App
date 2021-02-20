using Project1.Extras;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Addfac : Form
    {
        private Faculty fac;
        private Account acc;
        private Studmng stumng;
        private Facmng facmng;
        private Accmng accmng;
        private Images images;
        Assignment ass;
        Assignment_submit asb;
        int sid;
        string path,previous,naya;

        public Addfac()
        {
            fac = new Faculty();
            acc = new Account();
            ass = new Assignment();
            stumng = new Studmng();
            facmng = new Facmng();
            accmng = new Accmng();
            images = new Images();
            InitializeComponent();

            cmbxgen.DataSource = facmng.gender();
            cmbxqua.DataSource = facmng.qualification();
            if (accmng.readkey()!= null)
            {
                fac = facmng.get(Convert.ToInt32(accmng.readkey()));
                if (fac == null)
                {
                    throw new Exception("Access Denied");
                }
                else
                {
                    facmng.Email(Convert.ToInt32(accmng.readkey()));
                    string[] a = accmng.annoucements().Split('\n');
                    foreach (string s in a)
                    {
                        listBox2.Items.Add(s);
                    }
                    panel6.Visible = false;
                    phome.Visible = true;
                    acc = accmng.getfacpass(Convert.ToInt32(accmng.readkey()));
                    FillForm(fac, acc);
                    comboBox1.DataSource = facmng.studentids(Convert.ToInt32(accmng.readkey()));
                    dataGridView1.DataSource = facmng.getAll(Convert.ToInt32(accmng.readkey()));
                    dtmarks.DataSource = facmng.getmarks(Convert.ToInt32(accmng.readkey()));
                    dtmarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dtmarks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    ass = facmng.getassign(Convert.ToInt32(accmng.readkey()));
                    if (ass != null)
                    {
                        lblnote.Text = "Assignment Uploaded";
                        btnsubmit.Enabled = false;
                        btndelete.Enabled = true;
                        Fillexistassign(ass);
                    }
                    else if (ass == null)
                    {
                        lblnote.Text = "Assignment Not Uploaded";
                        btnsubmit.Enabled = true;
                        btnbrowse.Enabled = true;
                    }
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
           if(btnsave.Text.Equals("Save Changes"))
            {
                update(Convert.ToInt32(accmng.readkey()));
            }
        }
        
        public void update(int id)
        {
            try
            {
                fac = new Faculty();
                acc = new Account();
                fac.ID = id;
                acc.Fac_id = fac.ID;
                fac.Name = txtname.Text;
                acc.Password = txtpass.Text;
                fac.Qualification = cmbxqua.SelectedItem.ToString();
                fac.Email = txtemail.Text;
                fac.Date = dtpage.Value;
                fac.Gender = cmbxgen.SelectedItem.ToString();
                fac.Image = images.ImagetoByteArray(pictureBox1.Image);
                acc.Pro_id = fac.Program;
                facmng.Update(id,fac);
                facmng.Updatepass(id, acc);
                lblchanges.Text = "Save Successfully";

            }
            catch (Exception ex)
            {
                lblchanges.Text = ex.Message;
            }
        }

        public void FillForm(Faculty fac,Account acc)
        {
            lblid.Text = accmng.readkey();
            txtname.Text = fac.Name;
            txtpass.Text = acc.Password;
            txtemail.Text = fac.Email;
            dtpage.Value = fac.Date;
            lblpro.Text = Convert.ToString(fac.Program);
            lblcourse.Text = Convert.ToString(fac.Course);
            lbldes.Text = fac.Designation;
            lblsal.Text = Convert.ToString(fac.Salary);
            cmbxgen.SelectedItem = fac.Gender; 
            pictureBox1.Image =  images.ByteArraytoImage(fac.Image);
        }

        public void Fillassign (Assignment ass)
        {
            dtpdue.Value = ass.Due_date;
            txtfilename.Text = ass.Name;
            txtpath.Text = ass.Question;
        }

        public void Fillexistassign(Assignment ass)
        {
            dtpdue.Value = ass.Due_date;
            txtfilename.Text = ass.Name;
            txtpath.Text = ass.Question;
            naya = ass.Question;
            previous = ass.Previous;
        }

        private void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                addassign();
            }
            catch (Exception ex)
            {
                lblque.Text = ex.Message;
            }
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            Addfac add = new Addfac();
            this.Close();
            lblchanges.Text = "";
            lblmsg.Text = "";
            lblnote.Text = "";
            phome.Visible = false;
            lblque.Text = "";
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("MyRegKey");
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            acc = new Account();
            try
            {
                acc.Fac_id = Convert.ToInt32(txtidmain.Text);
                acc.Password = txtpassmain.Text;
                accmng.verifyfac(acc.Fac_id, acc.Password);
                string[] a = accmng.annoucements().Split('\n');
                foreach (string s in a)
                {
                    listBox2.Items.Add(s);
                }
                panel6.Visible = false;
                phome.Visible = true;
                accmng.createkey(acc.Fac_id);
                facmng.Email(Convert.ToInt32(accmng.readkey()));
                fac = facmng.get(Convert.ToInt32(accmng.readkey()));
                FillForm(fac, acc);
                lblmsg.Text = "";
                txtidmain.Text = "";
                txtpassmain.Text = "";
                comboBox1.DataSource = facmng.studentids(Convert.ToInt32(accmng.readkey()));
                dataGridView1.DataSource = facmng.getAll(Convert.ToInt32(accmng.readkey()));
                dtmarks.DataSource = facmng.getmarks(Convert.ToInt32(accmng.readkey()));
                dtmarks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dtmarks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                ass = facmng.getassign(Convert.ToInt32(accmng.readkey()));
                if (ass != null)
                {
                    lblnote.Text = "Assignment has been Uploaded";
                    btnsubmit.Enabled = false;
                    btnupdate.Enabled = false;
                    btndelete.Enabled = true;
                    Fillexistassign(ass);
                }
                else if (ass == null)
                {
                    lblnote.Text = "Assignment Not Uploaded";
                    btnsubmit.Enabled = true;
                    btnbrowse.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }

        private void btnbrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files|*.*";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                txtfilename.Text = ofd.SafeFileName;
                txtpath.Text =  ofd.FileName;           
            }          
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                facmng.Delete_assign(Convert.ToInt32(accmng.readkey()));
                //Fillexistassign(ass);
                txtfilename.Text = "";
                txtpath.Text = "";
                File.Move(naya, previous);
                lblque.Text = "Delete Successfully";
                btndelete.Enabled = false;
                btnupdate.Enabled = false;
                btnsubmit.Enabled = true;
            }
            catch (Exception ex)
            {
                lblque.Text = ex.Message;
            }
        }


        private void btnedit_Click(object sender, EventArgs e)
        {
            dtmarks.ReadOnly = false;
        }

        private void dtmarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            double marks = Convert.ToDouble(dtmarks.Rows[e.RowIndex].Cells[0].Value.ToString());
            sid = Convert.ToInt32(dtmarks.Rows[e.RowIndex].Cells[3].Value.ToString());

            if (dtmarks.ReadOnly.Equals(false))
            {
                if (marks == 0.1)
                {
                    pmark.Visible = true;                 
                }
                else
                {
                    MessageBox.Show("Already added");
                }
            }
            else
            {
                MessageBox.Show("Click the button first to add marks");
            }
        }

        public Assignment_submit Marks()
        {
            Assignment_submit asb = new Assignment_submit();
            asb.Marks = Convert.ToDouble(txtmark.Text);
            return asb;
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            dtmarks.ReadOnly = false;
        }

        private void addmark_Click(object sender, EventArgs e)
        {
            try
            {
                asb = Marks();
                stumng.updatemarks(sid, asb);
                lbladdm.Text = "Added Successfully";
                pmark.Visible = false;
                dtmarks.DataSource = facmng.getmarks(Convert.ToInt32(accmng.readkey()));
            }
            catch (Exception ex)
            {
                lbladdm.Text = ex.Message;
            }
        }

        private void btnimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg)|*.jpg|Image files (*.png)|*.png|Image files (*.bmp)|*.bmp|Image files (*.jpeg)|*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);
                pictureBox1.Image = img;
            }
        }

        private void btncheck_Click(object sender, EventArgs e)
        {
            try
            {
                Plagiarism obj = new Plagiarism();
                obj.Scan("shahzaibmuradani1@gmail.com", "9058A47B-DC19-4856-9B1D-1390C5CFEF57");

                string line;
                path = @"E:\cscolor\result.txt";
                StreamReader sr = new StreamReader(path);

                line = sr.ReadToEnd();

                string[] a = line.Split('\n');
                foreach (string s in a)
                {
                    listBox1.Items.Add(s);
                }
                sr.Close();
                File.Delete(path);
                lblcheck.Text = "Checked";

                btnrefresh.Enabled = true;
            }
            catch (Exception ex)
            {
                lblcheck.Text = ex.Message;
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            lblcheck.Text = "";
            btnrefresh.Enabled = false;
        }

        private void txtidmain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void txtmark_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)) && (e.KeyChar!='.'))
            {
                e.Handled = true;
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                ass = facmng.getassign(Convert.ToInt32(accmng.readkey()));
                upassign(Convert.ToInt32(accmng.readkey()));
                if (naya != null && previous != null)
                {                   
                    Fillassign(ass);
                }
            }
            catch (Exception ex)
            {
                lblque.Text = ex.Message;
            }
        }

        public Assignment addassign()
        {
            try
            {
                ass = new Assignment();
                ass.ID = fac.ID;
                ass.Course_id = fac.Course;
                ass.Due_date = dtpdue.Value;
                ass.Name = txtfilename.Text;
                path = @"E:\cscolor\Sir " + fac.Name + "\\" + ass.Name;
                File.Move(txtpath.Text, path);
                ass.Question = path;
                ass.Previous = txtpath.Text;
                facmng.Add(ass);

                lblque.Text = "Upload Successfully";
                naya = ass.Question;
                previous = ass.Previous;
                btnupdate.Enabled = true;
                btndelete.Enabled = true;
                btnsubmit.Enabled = false;
                lblnote.Text = "Assignment has been uploaded";
                accmng.email();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ass;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            txtpassmain.UseSystemPasswordChar = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            txtpassmain.UseSystemPasswordChar = true;
        }

        private void btnforget_Click(object sender, EventArgs e)
        {
            try
            {
                var tuple = facmng.forgetpass(Convert.ToInt32(txtidmain.Text));
                accmng.emailpass(tuple.Item1, tuple.Item2);
                lblmsg.Text = "";
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }

        public Assignment upassign(int id)
        {
            try
            {
                ass = new Assignment();
                ass.ID = id;
                ass.Course_id = fac.Course;
                ass.Due_date = dtpdue.Value;
                ass.Name = txtfilename.Text;
                path = @"E:\cscolor\Sir " + fac.Name + "\\" + ass.Name;
                ass.Question = path;
                ass.Previous = txtpath.Text;
                facmng.updateassign(Convert.ToInt32(accmng.readkey()),ass);
                File.Move(naya, previous);
                File.Move(txtpath.Text, path);

                lblque.Text = "Update Successfully";
                naya = ass.Question;
                previous = ass.Previous;
                btnupdate.Enabled = true;
                btndelete.Enabled = true;
                btnsubmit.Enabled = false;
                lblnote.Text = "Assignment has been uploaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ass;
        }

        public string ansreturn()
        {
            string ans = stumng.getresult(Convert.ToInt32(accmng.readkey()),Convert.ToInt32(comboBox1.SelectedItem.ToString()));
            return ans;
        }
    }
}
