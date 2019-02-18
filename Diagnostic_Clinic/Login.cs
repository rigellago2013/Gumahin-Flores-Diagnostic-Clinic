using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Diagnostic_Clinic
{
    public partial class Login : Form
    {
            public static string username;
            protected string fname, lname, contact;
            protected int userId;

            //database connection
            DataTable dataTable;
            static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);

            public Login()
            {
                InitializeComponent();
            }

            private void Login_Load(object sender, EventArgs e)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
            }

            //confirm login function
            private void loginConfirm()
            {
                DataSet ds = new DataSet();

                string userName = txtUserName.Text;
                string userPassword = txtPassword.Text;

                if (userName == "" || userPassword == "")
                    MessageBox.Show("Must fill in required fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    string searchString = "SELECT * FROM users WHERE username = '" + userName + "' AND userpassword = '" + userPassword + "'";

                    OleDbDataAdapter searchAdapter = new OleDbDataAdapter(searchString, conn);

                    searchAdapter.Fill(ds, "dtResult");
                    dataTable = ds.Tables["dtResult"];

                    int totalRec = dataTable.Rows.Count;

                    if (totalRec > 0)
                    {
                        username = userName;

                        this.Hide();
                        Container container = new Container(userId,fname,lname,contact);
                        container.ShowDialog();
                    }
                    else
                        MessageBox.Show("Either username or password is invalid. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void cmdLogIn_Click_1(object sender, EventArgs e)
            {
                getUserInfo();
                loginConfirm();
            }   

         private void getUserInfo() 
         {
            string userName = txtUserName.Text;
            string userPassword = txtPassword.Text;
            int contact = 0;

            OleDbDataReader reader = null;
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM users WHERE username = '" + userName + "' AND userpassword = '" + userPassword + "'", conn);
            
            cmd.Parameters.AddWithValue("@ID", userId);
            cmd.Parameters.AddWithValue("@fname", fname);
            cmd.Parameters.AddWithValue("@lname", lname);
            reader = cmd.ExecuteReader();

            while(reader.Read()) {
                userId = reader.GetInt32(reader.GetOrdinal("ID"));
                fname = reader.GetString(reader.GetOrdinal("fname"));
                lname = reader.GetString(reader.GetOrdinal("lname"));
            }
            conn.Close();
        
        }

    



  

     
    
    }
}
