using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Addpro : Form
    {
        private Progmng promng;
        private Accmng accmng;
        private Account acc;
        private Pro pro;
        public Addpro()
        {
            accmng = new Accmng();
            pro = new Pro();
            acc = new Account();
            promng = new Progmng();
            InitializeComponent();
            if(accmng.readkey() != null)
            {
                pro = promng.get(Convert.ToInt32(accmng.readkey()));
                if (pro.Program_id.Equals(Convert.ToInt32(accmng.readkey())))
                {
                    string[] a = accmng.annoucements().Split('\n');
                    foreach (string s in a)
                    {
                        listBox1.Items.Add(s);
                    }
                    panel6.Visible = false;
                    phome.Visible = true;
                    pro = promng.get(Convert.ToInt32(accmng.readkey()));
                    acc = accmng.getpropass(Convert.ToInt32(accmng.readkey()));
                    dataGridView1.DataSource = promng.getcourse(Convert.ToInt32(accmng.readkey()));
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dataGridView2.DataSource = promng.getAll(Convert.ToInt32(accmng.readkey()));
                    FillForm(pro, acc);
                }
                else
                {
                    throw new Exception("Access Denied");
                }
            }
        }

        private void FillForm(Pro pro,Account acc)
        {
            lblid.Text = accmng.readkey();
            lblname.Text = pro.Program_Name;
            txtpas.Text =  acc.Password;
            lblsemnum.Text = Convert.ToString(pro.Semester_Number);
            lblsemname.Text = pro.Semester_Name;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                acc = new Account();
                acc.Password = txtpas.Text;
                promng.Update(Convert.ToInt32(accmng.readkey()), acc);
                lblchanges.Text = "Save Successfully";

            }
            catch (Exception ex)
            {
                lblchanges.Text = ex.Message;
            }
        }

        private void btnpass_Click(object sender, EventArgs e)
        {
            panelpass.Visible = true;
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
            phome.Visible = false;
            lblmsg.Text = "";
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("MyRegKey");
            Addpro pro = new Addpro();
            this.Close();
        }

        private void btnlogin_Click_1(object sender, EventArgs e)
        {
            try
            {
                acc.Pro_id = Convert.ToInt32(txtidmain.Text);
                acc.Password = txtpassmain.Text;

                promng.verifyprogram(acc.Pro_id, acc.Password);

                accmng.createkey(acc.Pro_id);
                pro = promng.get(Convert.ToInt32(accmng.readkey()));

                string[] a = accmng.annoucements().Split('\n');
                foreach (string s in a)
                {
                    listBox1.Items.Add(s);
                }

                panel6.Visible = false;
                phome.Visible = true;
                dataGridView1.DataSource = promng.getcourse(Convert.ToInt32(accmng.readkey()));
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView2.DataSource = promng.getAll(Convert.ToInt32(accmng.readkey()));
                FillForm(pro, acc);
                lblmsg.Text = "";
                txtidmain.Text = "";
                txtpassmain.Text = "";
            }
            catch (Exception ex)
            {
                lblmsg.Text = ex.Message;
            }
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

        private void btnforget_Click(object sender, EventArgs e)
        {
            try
            {
                var tuple = promng.forgetpass(Convert.ToInt32(txtidmain.Text));
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
