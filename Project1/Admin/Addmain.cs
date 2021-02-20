using Project1.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Addmain : Form
    {
        private Adminmng adminmng;
        private Account acc;
        private Admin ad;
        private Facmng facmng;
        private Accmng accmng;
        private Images images;
        string name;
        int fid;
        public Addmain()
        {
            ad = new Admin();
            accmng = new Accmng();
            facmng = new Facmng();
            ad = new Admin();
            acc = new Account();
            images = new Images();
            adminmng = new Adminmng();
            InitializeComponent();
            if (accmng.readkey() != null)
            {
                ad = adminmng.get(Convert.ToInt32(accmng.readkey()));
                if (ad == null)
                {
                    throw new Exception("Access Denied");
                }
                else
                {
                    if (ad.ID.Equals(Convert.ToInt32(accmng.readkey())))
                    {
                        acc = accmng.getadminpass(Convert.ToInt32(accmng.readkey()));

                        string[] a = accmng.annoucements().Split('\n');
                        foreach (string s in a)
                        {
                            listBox1.Items.Add(s);
                        }

                        panel1.Visible = false;

                        phome.Visible = true;
                        dataGridView1.DataSource = adminmng.getprogram();
                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dataGridView2.DataSource = adminmng.getcourse();
                        dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dataGridView3.DataSource = adminmng.getStudent();
                        comboBox1.DataSource = adminmng.teachernames();
                        FillForm(ad, acc);
                    }
                    else
                    {
                        throw new Exception("Access Denied");
                    }
                }
            }
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            try
            { 
            acc = new Account();
            acc.Admin_id = Convert.ToInt32(txtidmain.Text);
            acc.Password = txtpassmain.Text;
            adminmng.verifyadmin(acc.Admin_id, acc.Password);
            accmng.createkey(acc.Admin_id);
            ad = adminmng.get(Convert.ToInt32(accmng.readkey()));

            string[] a = accmng.annoucements().Split('\n');
            foreach (string s in a)
            {
                listBox1.Items.Add(s);
            }

            panel1.Visible = false;

            phome.Visible = true;
            dataGridView1.DataSource = adminmng.getprogram();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.DataSource = adminmng.getcourse();
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView3.DataSource = adminmng.getStudent();
            comboBox1.DataSource = adminmng.teachernames();
            FillForm(ad, acc);
            lblmsg.Text = "";
            txtidmain.Text = "";
            txtpassmain.Text = "";
            }
            catch(Exception ex)
            {
                lblmsg.Text = ex.Message;
            }           
        }

        private void FillForm(Admin ad, Account acc)
        {
            lblid.Text = accmng.readkey();
            txtname.Text = ad.Name;
            txtpass.Text = acc.Password;
            pictureBox1.Image = images.ByteArraytoImage(ad.Image);
        }
       
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                acc = new Account();
                acc.Password = txtpass.Text;
                ad.Name = txtname.Text;
                ad.Image = images.ImagetoByteArray(pictureBox1.Image);
                adminmng.UpdateName(Convert.ToInt32(accmng.readkey()),ad);
                adminmng.UpdatePass(Convert.ToInt32(accmng.readkey()), acc);
                lblchanges.Text = "Save Successfully";
            }
            catch (Exception ex)
            {
                lblchanges.Text = ex.Message;
            }
        }

        private void btnlogout_Click_3(object sender, EventArgs e)
        {
            phome.Visible = false;
            lblmsg.Text = "";
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("MyRegKey");
            lblchanges.Text = "";
            Addmain admin = new Addmain();
            this.Close();
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

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                Faculty fac = new Faculty();
                fac.Designation = txtdes.Text;
                fac.Salary = Convert.ToInt32(txtsalary.Text);
                adminmng.Update(fid, fac);
                lblchange1.Text = "Save Successfully";
            }
            catch (Exception ex)
            {
                lblchange1.Text = ex.Message;
            }
        }

        private void txtidmain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           name =  comboBox1.Text;
           fid =  facmng.getfac(name);
           var tuple = adminmng.getdetails(fid);
           txtdes.Text = tuple.Item1;
           txtsalary.Text = Convert.ToString(tuple.Item2);
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
                var tuple = adminmng.forgetpass(Convert.ToInt32(txtidmain.Text));
                accmng.emailpass(tuple.Item1, tuple.Item2);
                lblmsg.Text = "";
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }   
        }
    }
}
