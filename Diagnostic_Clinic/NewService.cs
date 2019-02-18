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
    public partial class NewService : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        DataTable dtAldServ;

        public NewService()
        {
            InitializeComponent();
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            } //anti-letter shit
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!getExistingService())
            {
                OleDbDataAdapter addAdapter = new OleDbDataAdapter();
                string addSql = "INSERT INTO services (name, rate) VALUES ('" + txtName.Text + "', " + Convert.ToDouble(txtRate.Text) + ")";
                conn.Open();
                addAdapter.InsertCommand = new OleDbCommand(addSql, conn);
                addAdapter.InsertCommand.ExecuteNonQuery();
                conn.Close();
                this.Close();
            } else {
                MessageBox.Show("Service already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtRate.Text = "";
            this.Close();
        }

        private Boolean getExistingService()
        {
            DataSet dsAlRe = new DataSet();
            string q = "SELECT name FROM services WHERE name LIKE '%" +txtName.Text+"%' OR name='" + txtName.Text + "'";
            OleDbDataAdapter daAlreadyServed = new OleDbDataAdapter(q, conn);
            daAlreadyServed.Fill(dsAlRe, "alreadyServed");
            dtAldServ = dsAlRe.Tables["alreadyServed"];
            int alreadyServed = 0;
            alreadyServed = dtAldServ.Rows.Count;

            if (alreadyServed > 0) {

                return true;

            } else {

                return false;

            }
            
        }
        



    }
}
