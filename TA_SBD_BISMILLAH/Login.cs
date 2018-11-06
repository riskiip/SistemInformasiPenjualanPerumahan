using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TA_SBD_BISMILLAH
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            password.PasswordChar = '*';
        }

        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

        public string VerHash(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        private Boolean frmLogin(string sUsername, string sPassword)
        {
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);
                String querySearch = @"select password from user where username='" + sUsername + "';";
                var da = new MySqlDataAdapter();
                var ds = new DataSet();
                conn.Open();
                da.SelectCommand = new MySqlCommand(querySearch);
                da.SelectCommand.Connection = conn;
                da.SelectCommand.CommandType = CommandType.Text;
                da.Fill(ds);
                if (ds.Tables[0].Rows[0][0].ToString() == VerHash(sPassword))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((username.Text == "") || (password.Text == ""))
            {
                MessageBox.Show("Username atau Password tidak boleh kosong!");
            }
            else
            {
                try
                {
                    if (frmLogin(username.Text, password.Text))
                    {
                        this.Hide();
                        new Form1(username.Text).Show();
                    }
                    else
                    {
                        MessageBox.Show("Username atau Password salah!");
                    }
                }
                catch
                {
                    MessageBox.Show("Username atau Password salah!");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }
    }
}

