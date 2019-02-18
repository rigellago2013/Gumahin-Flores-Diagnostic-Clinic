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
    public partial class UserSettings : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source= diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        DataTable DataTable, dtEmpty;
        protected int id;

        public UserSettings(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            OleDbDataAdapter addAdapt = new OleDbDataAdapter();
            string query = "INSERT INTO users(fname,lname,username,userpassword) values ('"+txtFirstname.Text+"','"+txtLastname.Text+"','"+txtUsername.Text+"','"+txtPasswordConfirm.Text+"')";
            conn.Open();
            addAdapt.InsertCommand = new OleDbCommand(query, conn);
            addAdapt.InsertCommand.ExecuteNonQuery();
            MessageBox.Show("Added Successfully!");
            conn.Close();

        }


        private void UserSettings_Load(object sender, EventArgs e)
        {
            getUsers();
     
        }

        private void getUsers()
        {
            DataSet ds = new DataSet();

            string commandString = " SELECT ID, fname, lname FROM users ORDER BY ID";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandString, conn);

            dataAdapter.Fill(ds, "prog");
            DataTable = ds.Tables["prog"];

            int totalRec = DataTable.Rows.Count;

            if (totalRec != 0)
            {
                dgvUserList.DataSource = DataTable;
                dgvUserList.Columns["ID"].Visible = false;
            }
            else
            {
                dgvUserList.DataSource = dtEmpty;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUserPasswordNew.Text == txtUserPasswordOld.Text)
            {
                OleDbDataAdapter addAdapt = new OleDbDataAdapter();
               string query = "UPDATE users SET userpassword = '" + txtUserPasswordNew.Text + "' WHERE ID ="+id;
                conn.Open();
                addAdapt.InsertCommand = new OleDbCommand(query, conn);
                addAdapt.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Password updated successfully!");
                conn.Close();
                clearFieldsChangePassword();
                Application.Restart();
            }
            else {

                MessageBox.Show("Password doesnt match! Please try again!");
            }
        }

        private void clearFieldsChangePassword(){
            txtUserPasswordNew.Text = "";
            txtUserPasswordOld.Text = "";
        }

    
    }
}
