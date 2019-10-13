using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanteenPOS
{
    public partial class Password : Form
    {
      public static  bool isAdmin = false;
        public Password()
        {
            InitializeComponent();
           
            txtPassword.PasswordChar = '*';
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "admin" && txtPassword.Text == "pakistan123")
            {
                isAdmin = true;
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else if(txtUserName.Text == "user" && txtPassword.Text == "12345678")
            {
                isAdmin = false;
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            else
                MessageBox.Show("wrong username or password");
        }
    }
}
