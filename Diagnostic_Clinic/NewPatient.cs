using Diagnostic_Clinic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagnostic_Clinic
{
    public partial class NewPatient : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source= diagnostic.accdb";

        OleDbConnection conn = new OleDbConnection(connectionString);
        public NewPatient()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLname.Text == "" || txtFname.Text == "" || txtMI.Text == "" || txtAddress.Text == ""
                  || txtContact.Text == "" || bday.Text == "" || cmbGender.Text == "" || cmbStatus.Text == "")
                {
                    MessageBox.Show("Please fill up all fields.");
                    return;
                }
                else
                {



                    OleDbDataAdapter addAdapter = new OleDbDataAdapter();
                    string addSql = "INSERT into patients(fname, lname, mid_initial, address, contact, birthday, gender, civil_status, created_at) values('" + txtFname.Text.ToUpper() + "','" + txtLname.Text.ToUpper() + "','" + txtMI.Text.ToUpper() + "','"
                        + txtAddress.Text.ToUpper() + "','" + txtContact.Text.ToUpper() + "','" + bday.Value + "','" + cmbGender.Text.ToUpper() + "','" + cmbStatus.Text.ToUpper() + "', NOW())";


                    conn.Open();
                    addAdapter.InsertCommand = new OleDbCommand(addSql, conn);
                    addAdapter.InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("Added...");
                    conn.Close();

                    PatientList patienList = new PatientList();
                    patienList.MdiParent = this.ParentForm;
                    patienList.StartPosition = FormStartPosition.Manual;
                    patienList.Location = new Point(230, 0);
                    patienList.Show();

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            
          }

        private void NewPatient_FormClosed(object sender, FormClosedEventArgs e)
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

        }
    }

