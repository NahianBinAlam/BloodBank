using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBank
{
    public partial class AdminLogIn : Form
    {
        public AdminLogIn()
        {
            InitializeComponent();
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            try
            {
                OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
                OracleCommand cmd = new OracleCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "select username from admin where username = '" + username + "' and password = '" + password + "'";
                cmd.CommandType = CommandType.Text;
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (!dr.HasRows)
                {
                    MessageBox.Show("Username and password does not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.Visible = false;
                    new MainMenuForm().Show();

                }
                conn.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
