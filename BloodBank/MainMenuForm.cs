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
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void donateBloodButton_Click(object sender, EventArgs e)
        {
            donateBloodPanel.BringToFront();
        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            addPersonPanel.BringToFront();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            searchPanel.BringToFront();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string name = nameTextBox.Text;
            string username = usernameTextBox.Text;
            int age = Convert.ToInt32(ageNumericUpDown.Value);
            string sex = "";
            if (maleRadioButton.Checked)
            {
                sex = "Male";
            }
            else if (femaleRadioButton.Checked)
            {
                sex = "Female";
            }
            string mobileNo = mobileNoTextBox.Text;
            string city = cityTextBox.Text;
            string address = addressTextBox.Text;
            int height = Convert.ToInt32(heightNumericUpDown.Value);
            string eyeColor = eyeColorComboBox.Text;
            string diseases = diseasesTextBox.Text;
            string bloodGroup = bloodGroupComboBox.Text;
            string donatedBefore = "";
            string lastDonateDate = "";
            bool flag = false;
            if (yesRadioButton.Checked)
            {
                donatedBefore = "Yes";
                lastDonateDate = donationDateTimePicker.Value.ToString();
            }
            else if (noRadioButton.Checked)
            {
                donatedBefore = "No";
                lastDonateDate = null;
            }
            string mn = "", un = "";
            long mobileNoCheck;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(username) || (!maleRadioButton.Checked && !femaleRadioButton.Checked) || string.IsNullOrWhiteSpace(mobileNo) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(eyeColor) || string.IsNullOrWhiteSpace(bloodGroup) || (!yesRadioButton.Checked && !noRadioButton.Checked))
            {
                MessageBox.Show("Mandatory fields cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag = true;
            }
            else if (!flag)
            {
                try
                {
                    OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select username, mobileno from usertable where username = '" + username + "' or mobileno = '" + mobileNo + "'";
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        un = dr.GetString(0);
                        mn = dr.GetString(2);
                    }
                    if (un == username && !flag)
                    {
                        MessageBox.Show("Username not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!long.TryParse(mobileNo, out mobileNoCheck) || mobileNo.Length < 11)
                    {
                        MessageBox.Show("Invalid mobile number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (mobileNo == mn)
                    {
                        MessageBox.Show("The mobile number you entered is already being used.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        cmd.CommandText = "insert into usertable values(:name, :username, :age, :sex, :mobileno, :city, :address, :height, :eye_color, :diseases, :blood_group, :donated_before)";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new OracleParameter(":name", name));
                        cmd.Parameters.Add(new OracleParameter(":username", username));
                        cmd.Parameters.Add(new OracleParameter(":age", age));
                        cmd.Parameters.Add(new OracleParameter(":sex", sex));
                        cmd.Parameters.Add(new OracleParameter(":mobileno", mobileNo));
                        cmd.Parameters.Add(new OracleParameter(":city", city));
                        cmd.Parameters.Add(new OracleParameter(":address", address));
                        cmd.Parameters.Add(new OracleParameter(":height", height));
                        cmd.Parameters.Add(new OracleParameter(":eye_color", eyeColor));
                        cmd.Parameters.Add(new OracleParameter(":diseaes", diseases));
                        cmd.Parameters.Add(new OracleParameter(":blood_group", bloodGroup));
                        cmd.Parameters.Add(new OracleParameter(":donated_before", donatedBefore));
                        int rowsUpdated = cmd.ExecuteNonQuery();
                        if (rowsUpdated > 0)
                        {
                            MessageBox.Show("Successfully added information.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void donateBloodUsernameTextBox_Leave(object sender, EventArgs e)
        {
            string username = donateBloodUsernameTextBox.Text;
            OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select blood_group from usertable where username = '" + username + "'";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                donateBloodBloodGroupCmboBox.Text = dr.GetString(0);
            }
        }

        private void donateBloodSaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string username = donateBloodUsernameTextBox.Text;
                string bloodGroup = donateBloodBloodGroupCmboBox.Text;
                int amount = Convert.ToInt32(amountNumericUpDown.Value);
                string date = DateTime.Now.ToString("dd-MMM-yy");
                OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insert into donor values(:username, :blood_group, :amount, :donate_date)";
                cmd.Parameters.Add(new OracleParameter(":username", username));
                cmd.Parameters.Add(new OracleParameter(":blood_group", bloodGroup));
                cmd.Parameters.Add(new OracleParameter(":amount", amount));
                cmd.Parameters.Add(new OracleParameter(":donate_date", date));
                int rowsUpdated = cmd.ExecuteNonQuery();
                cmd.CommandText = "update inventory set amount = (amount + '" + amount + "') where blood_group = '" + bloodGroup + "'";
                int rowsUpdated1 = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0 && rowsUpdated1 > 0)
                {
                    MessageBox.Show("Success!");

                }
                else
                {
                    MessageBox.Show("Something went wrong!", "Oops!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source);
            }
        }

        private void searchButton_Click_1(object sender, EventArgs e)
        {
            string sql = "";
            if (searchByComboBox.Text == "Person name")
            {
                sql = "Select * from usertable where name = '" + searchTextBox.Text + "'";
            }
            else if (searchByComboBox.Text == "Blood group")
            {
                sql = "Select * from usertable where blood_group = '" + searchTextBox.Text + "'";
            }
            else if (searchByComboBox.Text == "City")
            {
                sql = "Select * from usertable where city = '" + searchTextBox.Text + "'";
            }
            else if (searchByComboBox.Text == "Age")
            {
                sql = "Select * from usertable where age = '" + Convert.ToInt32(searchTextBox.Text) + "'";
            }
            OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            OracleDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
               // dataGridView1.
            }
        }

        private void inventoryButton_Click(object sender, EventArgs e)
        {
            
            inventoryPanel.BringToFront();
            
        }
        int bloodAmount;
        private void inventorySearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select amount from inventory where blood_group = '" + inventoryBloodGroupComboBox.Text + "'";
                OracleDataReader dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.GetInt32(0) == 0)
                {
                    label21.Text = "Not available";
                    amountTextBox.Enabled = false;
                    getBloodButton.Enabled = false;
                }
                else
                {
                    label21.Text = dr.GetInt32(0) + " bottles available.";
                    bloodAmount = dr.GetInt32(0);
                    amountTextBox.Enabled = true;
                    getBloodButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +" Source: "+ex.Source);
            }
        }

        private void getBloodButton_Click(object sender, EventArgs e)
        {
            try
            {
                int amount = Convert.ToInt32(amountTextBox.Text);
                if (amount > bloodAmount)
                {
                    MessageBox.Show("Not enough available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    OracleConnection conn = new OracleConnection("DATA SOURCE=localhost:1521/XE;USER ID=NAHIAN; PASSWORD=smaran89");
                    conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "update inventory set amount = (amount - '" + amount + "') where blood_group = '" + inventoryBloodGroupComboBox.Text + "'";
                    int rowsUpdated = cmd.ExecuteNonQuery();
                    if (rowsUpdated > 0)
                    {
                        MessageBox.Show("Success");
                    }
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void logOutButton_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            new AdminLogIn().Show();
        }
    }
}
