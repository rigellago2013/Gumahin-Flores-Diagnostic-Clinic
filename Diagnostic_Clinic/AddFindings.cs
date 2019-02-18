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
    public partial class AddFindings : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        protected int app_id;

        public AddFindings(int id)
        {
            InitializeComponent();

            this.app_id = id;
        }

        private void AddFindings_Load(object sender, EventArgs e)
        {
            setInfo();
        }

        private void setInfo()
        {
            //lblName.Text = Findings.patient_name;
        
            lblRef.Text = Findings.ref_id;
            lblDate.Text = Findings.app_date;
            lblService.Text = Findings.service;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtFindings.Text == "")
                MessageBox.Show("Must provide findings!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                OleDbDataAdapter updateAdapter = new OleDbDataAdapter();
                string updateSql = "UPDATE appointment_details SET findings = '" + txtFindings.Text + "' WHERE ID = " + app_id;

                var confirmUpdate = MessageBox.Show("Confirm Findings?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmUpdate == DialogResult.Yes)
                {
                    conn.Open();

                    updateAdapter.UpdateCommand = new OleDbCommand(updateSql, conn);
                    updateAdapter.UpdateCommand.ExecuteNonQuery();
                    MessageBox.Show("Findings Saved!");

                    conn.Close();
                    this.Close();

                
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
