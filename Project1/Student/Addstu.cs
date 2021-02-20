using Project1.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Addstu : Form
    {
        private Studmng stumng;
        private Accmng accmng;
        private Facmng facmng;
        private Student stu;
        private Images images;
        private Account acc;
        static int fid;
        private string name,imageloc;
        public Addstu()
        {
            stu = new Student();
            facmng = new Facmng();
            stumng = new Studmng();
            accmng = new Accmng();
            images = new Images();
            InitializeComponent();

            cmbxgen.DataSource = facmng.gender();
            if (accmng.readkey()!=null)
            {
                stu = stumng.get(Convert.ToInt32(accmng.readkey()));
                if (stu.ID.Equals(Convert.ToInt32(accmng.readkey())))
                {
                    string[] a = accmng.annoucements().Split('\n');
                    foreach (string s in a)
                    {
                        listBox1.Items.Add(s);
                    }

                    panel6.Visible = false;
                    phome.Visible = true;
                    acc = accmng.getstupass(Convert.ToInt32(accmng.readkey()));
                    FillForm(stu, acc);
                    comboBox1.DataSource = stumng.teachernames(Convert.ToInt32(accmng.readkey()));
                    comboBox2.DataSource = stumng.teachernames(Convert.ToInt32(accmng.readkey()));
                    dataGridView1.DataSource = stumng.getcourse(Convert.ToInt32(accmng.readkey()));
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    
                }
                else
                {
                    throw new Exception("Access Denied");
                }
            }
        }

        public void update(int id)
        {
            try
            {
                stu = new Student();
                acc = new Account();
                stu.ID = id;
                acc.Stu_id = stu.ID;
                stu.Name = txtname.Text;
                acc.Password = txtpass.Text;
                stu.Email = txtemail.Text;
                stu.Age = dtpage.Value;
                stu.Gender = cmbxgen.SelectedItem.ToString();
                stu.Image = images.ImagetoByteArray(pictureBox1.Image);
                acc.Pro_id = stu.Program;
                stumng.Update(id, stu);
                stumng.Updatepass(id, acc);
                lblchanges.Text = "Save Successfully";
            }
            catch (Exception ex)
            {
                lblchanges.Text = ex.Message;
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            acc = new Account();
            try
            {
                acc.Stu_id = Convert.ToInt32(txtidmain.Text);
                acc.Password = txtpassmain.Text;
                accmng.verifystu(acc.Stu_id, acc.Password);

                accmng.createkey(acc.Stu_id);
                stu = stumng.get(Convert.ToInt32(accmng.readkey()));
                FillForm(stu, acc);

                string[] a = accmng.annoucements().Split('\n');
                foreach (string s in a)
                {
                    listBox1.Items.Add(s);
                }

                panel6.Visible = false;

                phome.Visible = true;
                comboBox1.DataSource = stumng.teachernames(Convert.ToInt32(accmng.readkey()));
                comboBox2.DataSource = stumng.teachernames(Convert.ToInt32(accmng.readkey()));
                dataGridView1.DataSource = stumng.getcourse(Convert.ToInt32(accmng.readkey()));
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                lblmsg.Text = "";
                txtidmain.Text = "";
                txtpassmain.Text = "";
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }

        private void FillForm(Student stu,Account acc)
        {
            lblid.Text = accmng.readkey();
            txtname.Text = stu.Name;
            txtpass.Text = acc.Password;
            txtemail.Text = stu.Email;
            dtpage.Value = stu.Age;
            cmbxgen.SelectedItem = stu.Gender;
            lblsem.Text = Convert.ToString(stu.Semester);
            lblpro.Text = Convert.ToString(stu.Program);
            pictureBox1.Image = images.ByteArraytoImage(stu.Image);
        }

        private void btnbrowse_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtfilename.Text = ofd.SafeFileName;
                txtpath.Text = ofd.FileName;
            }
        }

        private void btndownload_Click_1(object sender, EventArgs e)
        {
            name = comboBox1.SelectedItem.ToString();
            fid = facmng.getfac(name);
            var tuple = stumng.getassignments(fid);
            if (tuple.Item1 == null)
            {
                lbldownload.Text = "Not uploaded yet";
            }
            else
            {
                string path = @"E:\cscolor\" + Convert.ToInt32(accmng.readkey()) + "\\" + tuple.Item2;
                if (File.Exists(tuple.Item1))
                {
                    lbldownload.Text = "Already Downloaded";
                }
                else
                {
                    File.Copy(tuple.Item1, path);
                    lbldownload.Text = "Downloaded Successfully";
                }
            }
        }

        private void Check_Click_1(object sender, EventArgs e)
        {
            pcheck.Visible = true;
            dataGridView2.DataSource = stumng.checkmarks(Convert.ToInt32(accmng.readkey()));
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void btnsubmit_Click_1(object sender, EventArgs e)
        {
            name = comboBox2.SelectedItem.ToString();
            fid = facmng.getfac(name);
            var tuple = stumng.getsubmitassign(Convert.ToInt32(accmng.readkey()));
            if (tuple.Item1 != 0 && tuple.Item2 != 0)
            {
                lblque.Text = "Already Submitted";
            }

            else
            {                
                try
                {
                    Upload up = new Upload();
                    Assignment_submit asb = new Assignment_submit();
                    asb.Fac_id = fid;
                    asb.Date = asb.Date;
                    up.Name = txtfilename.Text;
                    string path = @"E:\cscolor\Sir " + name + "\\assignments\\" + up.Name;
                    asb.Answer = path;
                    asb.ID = Convert.ToInt32(accmng.readkey());
                    File.Move(txtpath.Text, path);
                    stumng.Add(asb);
                    lblque.Text = "Uploaded Successfully";
                }
                catch (Exception ex)
                {
                    lblque.Text = ex.Message;
                }
                
            }
        }

        private void btnlogout_Click_1(object sender, EventArgs e)
        {
            phome.Visible = false;
            lblmsg.Text = "";
            txtfilename.Text = "Name";
            txtpath.Text = "Path";
            Addstu stu = new Addstu();
            this.Close();
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("MyRegKey");
        }
        
        private void txtidmain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            txtpassmain.UseSystemPasswordChar = false;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            txtpassmain.UseSystemPasswordChar = true;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (btnsave.Text.Equals("Save Changes"))
            {
                update(Convert.ToInt32(accmng.readkey()));
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

        private void btnforget_Click(object sender, EventArgs e)
        {
            try
            {
                var tuple = stumng.forgetpass(Convert.ToInt32(txtidmain.Text));
                accmng.emailpass(tuple.Item1, tuple.Item2);
                lblmsg.Text = "";
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.jpg)|*.jpg|Image files (*.png)|*.png|Image files (*.bmp)|*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imageloc = ofd.FileName.ToString();
                pictureBox1.ImageLocation = imageloc;
            }
        }
    }
}
