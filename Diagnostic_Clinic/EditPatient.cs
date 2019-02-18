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
    public partial class EditPatient : Form
    {
        DataTable dataTable;
        int currRec = 0;
        int totalRec = 0;
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source= diagnostic.accdb";
        protected int pId;
        protected string fname, lname, mi, gender, bday, address, contact, civilStat;

        OleDbConnection conn = new OleDbConnection(connectionString);
        public EditPatient(string id, string fname, string lname, string mi, string gender, string bday, string address, string contact, string civilStat)
        {
            InitializeComponent();

            this.pId = Convert.ToInt32(id);
            this.fname = fname;
            this.lname = lname;
            this.mi = mi;
            this.gender = gender;
            this.bday = bday;
            this.address = address;
            this.contact = contact;
            this.civilStat = civilStat;

        }

      

        private void EditPatient_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                PatientList patientList = new PatientList();
                patientList.MdiParent = this.ParentForm;
                patientList.StartPosition = FormStartPosition.Manual;
                patientList.Location = new Point(230, 0);
                patientList.Show();
            }
            catch
            {
                PatientList patientList = new PatientList();
                patientList.MdiParent = this.ParentForm;
                patientList.StartPosition = FormStartPosition.Manual;
                patientList.Location = new Point(230, 0);
                patientList.Show();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLname.Text == "" || txtFname.Text == "" || txtMI.Text == "" || txtAddress.Text == ""
                  || txtContact.Text == "" || birth.Text == "" || cmbGender.Text == "" || cmbStatus.Text == "")
                {
                    MessageBox.Show("Please fill up all fields.");
                    return;
                }
                else
                {
                    OleDbDataAdapter updateAdapter = new OleDbDataAdapter();
                    string update = "UPDATE patients set fname = '" + txtFname.Text.ToUpper() + "', lname ='" + txtLname.Text.ToUpper() + "', mid_initial ='" + txtMI.Text.ToUpper() + "', address ='"
                         + txtAddress.Text.ToUpper() + "',contact = '" + txtContact.Text.ToUpper() + "', birthday = '" + birth.Value + "',gender ='" + cmbGender.Text.ToUpper() + "',civil_status ='" + cmbStatus.Text.ToUpper() + "' ,updated_at = NOW() WHERE ID = " + Convert.ToInt32(pId) + " ";



                    conn.Open();
                    updateAdapter.InsertCommand = new OleDbCommand(update, conn);
                    updateAdapter.InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("Updated...");
                    conn.Close();

                    PatientList patienList = new PatientList();
                    patienList.MdiParent = this.ParentForm;
                    patienList.StartPosition = FormStartPosition.Manual;
                    patienList.Location = new Point(230, 0);
                    patienList.Show();

                    this.Close();
                }
            }
            catch
            {

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {

                PatientList patientList = new PatientList();
                patientList.MdiParent = this.ParentForm;
                patientList.StartPosition = FormStartPosition.Manual;
                patientList.Location = new Point(230, 0);
                patientList.Show();
                this.Close();
            }
            catch
            {
                PatientList patientList = new PatientList();
                patientList.MdiParent = this.ParentForm;
                patientList.StartPosition = FormStartPosition.Manual;
                patientList.Location = new Point(230, 0);
                patientList.Show();
                this.Close();
            }
        }

        private void EditPatient_Load(object sender, EventArgs e)
        {
            txtFname.Text = fname;
            txtLname.Text = lname;
            txtMI.Text = mi;
            txtAddress.Text = address;
            birth.Text = Convert.ToString(bday);
            cmbGender.Text = gender;
            cmbStatus.Text = civilStat;
            txtContact.Text = contact;
        }
    }

}
