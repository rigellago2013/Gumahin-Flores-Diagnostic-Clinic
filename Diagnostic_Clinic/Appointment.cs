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
    public partial class Appointment : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        public int allPatient, searchPatient, allServices, totalServiceRendered, serveId, appId, alreadyServed, currServRend, refNo;
        public string patientId, servId, servRendId, appDetId;
        public double totalAmt;
        DataTable dtAllPatient, dtGetPatient, dtAllServices, dtServiceRendered, dtEmpty, dtAlreadyServed;

        public Appointment(int refCode)
        {
            InitializeComponent();
            refNo = refCode;
        }

        private void getPatient()
        {
            DataSet dsGetAllPatient = new DataSet();
            string sql = "SELECT ID, fname, lname, mid_initial,gender, contact, birthday FROM patients ORDER BY ID";
            OleDbDataAdapter daGetAllPatient = new OleDbDataAdapter(sql, conn);
            daGetAllPatient.Fill(dsGetAllPatient, "allPatient");
            dtAllPatient = dsGetAllPatient.Tables["allPatient"];
            allPatient = dtAllPatient.Rows.Count;
            dgvPatientList.DataSource = dtAllPatient;
            dgvPatientList.Columns["ID"].Visible = false;
        }

        private void Appointment_Load(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Today;
            getPatient();
            getServices();
            insertRecord(refNo);
            getAppId(refNo);

            if (totalServiceRendered == 0) {

                btnRemove.Enabled = false;

            }

        }

        private void dgvPatientList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            patientId = dgvPatientList.SelectedRows[0].Cells[0].Value.ToString();
            lblfname.Text = dgvPatientList.SelectedRows[0].Cells[1].Value.ToString();
            lbllname.Text = dgvPatientList.SelectedRows[0].Cells[2].Value.ToString();
            lblMidIn.Text = dgvPatientList.SelectedRows[0].Cells[3].Value.ToString();
            lblGender.Text = dgvPatientList.SelectedRows[0].Cells[4].Value.ToString();
            lblContact.Text = dgvPatientList.SelectedRows[0].Cells[5].Value.ToString();
            lblBirthday.Text = dgvPatientList.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void tbSearchPatient_TextChanged(object sender, EventArgs e)
        {
            DataSet dsSearchPatient = new DataSet();
            string sql = "SELECT ID, fname, lname, mid_initial,gender, contact, birthday FROM patients WHERE fname LIKE '%" + tbSearchPatient.Text + "%' OR lname LIKE '%" + tbSearchPatient.Text+"%' ORDER BY ID";
            OleDbDataAdapter daSearchPatient = new OleDbDataAdapter(sql, conn);
            daSearchPatient.Fill(dsSearchPatient, "searchPatient");
            dtGetPatient = dsSearchPatient.Tables["searchPatient"];
            searchPatient = dtGetPatient.Rows.Count;
            dgvPatientList.DataSource = dtGetPatient;
        }


        private void getServices()
        {
            DataSet dsAllSevices = new DataSet();
            string sql = "SELECT ID, name, rate FROM services ORDER BY ID";
            conn.Open();
            OleDbDataAdapter daAllServices = new OleDbDataAdapter(sql, conn);
            daAllServices.Fill(dsAllSevices, "allServices");
            dtAllServices = dsAllSevices.Tables["allServices"];
            allServices = dtAllServices.Rows.Count;
            dgvServiceList.DataSource = dtAllServices;
            conn.Close();
            dgvServiceList.Columns["ID"].Visible = false;
        }


        private void insertRecord(int refId)
        {
            OleDbDataAdapter addAdapter = new OleDbDataAdapter();
            string addSql = "INSERT INTO appointment (refid, app_date) VALUES ('" + Convert.ToInt64(refId) + "', #"+dtpDate.Text+"#)";
            conn.Open();
            addAdapter.InsertCommand = new OleDbCommand(addSql, conn);
            addAdapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
        }


        private void serviceRendered(int refId)
        {
            refId = refNo;
            DataSet dsServiceRendered = new DataSet();
            string cart = "SELECT AD.ID, S.name, S.rate FROM services S INNER JOIN (appointment A INNER JOIN appointment_details AD ON A.refid = AD.refid) ON S.ID = AD.serv_id WHERE AD.refid = " + Convert.ToInt32(refId) + "";
            OleDbDataAdapter daServiceRendered = new OleDbDataAdapter(cart, conn);
            daServiceRendered.Fill(dsServiceRendered, "Contents");
            dtServiceRendered = dsServiceRendered.Tables["Contents"];
            this.currServRend = 0;
            this.totalServiceRendered = dtServiceRendered.Rows.Count;
        
            if (this.totalServiceRendered != 0) {

                dgvServicesRendered.DataSource = dtServiceRendered;
                dgvServicesRendered.Columns["ID"].Visible = false;
                btnRemove.Enabled = true;

            } else {

                btnRemove.Enabled = false;
                dgvServicesRendered.DataSource = dtEmpty;

            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!getAlreadyServRend()) {

                if (lblServNameList.Text != "-") {

                    OleDbDataAdapter addAppDetAdapter = new OleDbDataAdapter();
                    string query = "INSERT INTO appointment_details (app_id,serv_id, amount, created_at, refid) VALUES (" + Convert.ToInt32(appId) + "," + Convert.ToInt32(servId) + ", " + Convert.ToDouble(lblRateList.Text) + ", NOW(), " + Convert.ToInt32(refNo) + ")";
                    conn.Open();
                    addAppDetAdapter.InsertCommand = new OleDbCommand(query, conn);
                    addAppDetAdapter.InsertCommand.ExecuteNonQuery();
                    conn.Close();
                    serviceRendered(refNo);
                    sumTotalAmount();

                } else {

                    MessageBox.Show("Please fill up the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }

            } else {

                MessageBox.Show("Service already added.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void dgvServiceList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            servId = dgvServiceList.SelectedRows[0].Cells[0].Value.ToString();
            lblServNameList.Text = dgvServiceList.SelectedRows[0].Cells[1].Value.ToString();
            lblRateList.Text = dgvServiceList.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void getAppId(int refId)
        {
            OleDbDataReader reader = null;
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT ID FROM appointment WHERE refid =" + Convert.ToInt32(refId) +" ", conn);
            cmd.Parameters.AddWithValue("@ID", appId);
            reader = cmd.ExecuteReader();

            while (reader.Read()) {
                appId = reader.GetInt32(reader.GetOrdinal("ID"));
    
            }
            conn.Close();
        }

        private void dgvServicesRendered_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblServRendName.Text = dgvServicesRendered.SelectedRows[0].Cells[1].Value.ToString();
            lblServRendRate.Text = dgvServicesRendered.SelectedRows[0].Cells[2].Value.ToString();
            appDetId = dgvServicesRendered.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(appDetId != "" && lblServRendName.Text != "-" && lblServRendRate.Text != "-") {

                OleDbDataAdapter dAdapter = new OleDbDataAdapter();
                conn.Open();
                string sql = "DELETE FROM appointment_details WHERE ID =" + Convert.ToInt32(appDetId);
                dAdapter.DeleteCommand = new OleDbCommand(sql, conn);
                dAdapter.DeleteCommand.ExecuteNonQuery();
                MessageBox.Show("Successfully Removed!", "Dialog Box", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                conn.Close();

                serviceRendered(refNo); //get services rendered by patient

                if (totalServiceRendered != 0) {

                    sumTotalAmount(); //sum total amount of services rendered by patient

                } else {

                    lblTotal.Text =  "0.00"; 

                }

                lblServRendName.Text = "-";
                lblServRendRate.Text = "-";
                appDetId = "";

            } else {

                MessageBox.Show("Please choose a service to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }


        private void sumTotalAmount()
        {
            OleDbDataReader reader = null;
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT SUM(amount) AS amt FROM appointment_details WHERE refid =" + Convert.ToInt32(refNo), conn);
            totalAmt = Convert.ToDouble(cmd.ExecuteScalar());
            conn.Close();
            lblTotal.Text = Convert.ToString(totalAmt) + ".00"; 
        }

        public void addPatientId()
        {
             OleDbDataAdapter addPatientIdAdapter = new OleDbDataAdapter();
             conn.Open();
             string sql = "UPDATE appointment SET patient_id = " + Convert.ToInt32(patientId) + " WHERE refid =" + Convert.ToInt32(refNo) + "";
             addPatientIdAdapter.UpdateCommand = new OleDbCommand(sql, conn);
             addPatientIdAdapter.UpdateCommand.ExecuteNonQuery();
             conn.Close();
        }

        private Boolean getAlreadyServRend() //pilion ang mga services nga ara na daan
        {
            DataSet dsAlRe = new DataSet();
            string q = "SELECT * FROM appointment_details WHERE serv_id ="+Convert.ToInt32(servId)+" AND refid="+ Convert.ToInt32(refNo);
            OleDbDataAdapter daAlreadyServed = new OleDbDataAdapter(q, conn);
            daAlreadyServed.Fill(dsAlRe, "alreadyServed");
            dtAlreadyServed = dsAlRe.Tables["alreadyServed"];
            alreadyServed = 0;
            alreadyServed = dtAlreadyServed.Rows.Count;

            if (alreadyServed > 0) {

                return true;

            } else {

                return false;

            }
        
        }


        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (patientId != "" && lblfname.Text != "-" && lblTotal.Text != "0.00") {

                DialogResult discard = MessageBox.Show("Are you sure you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (discard == DialogResult.Yes) {
                    addPatientId();

                    this.Hide();

                    try
                    {
                        Payment paymentForm = new Payment(patientId, totalAmt, refNo);
                        paymentForm.MdiParent = this.ParentForm;
                        paymentForm.StartPosition = FormStartPosition.Manual;
                        paymentForm.Location = new Point(400, 50);
                        paymentForm.Show();
                    }
                    catch
                    {
                        Payment paymentForm = new Payment(patientId, totalAmt, refNo);
                        paymentForm.MdiParent = this.ParentForm;
                        paymentForm.StartPosition = FormStartPosition.Manual;
                        paymentForm.Location = new Point(400, 50);
                        paymentForm.Show();
                    }
                }

            } else {

                MessageBox.Show("Please fill up the required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.totalServiceRendered != 0)
            {

                DialogResult discard = MessageBox.Show("Do you wish to discard all the items?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (discard == DialogResult.Yes)
                {

                    while (currServRend < totalServiceRendered) {

                        conn.Open();
                        string cancelDeleteSql = "DELETE FROM appointment_details WHERE refid=" + Convert.ToInt32(refNo);
                        OleDbDataAdapter cancelAdapter2 = new OleDbDataAdapter();
                        cancelAdapter2.DeleteCommand = new OleDbCommand(cancelDeleteSql, conn);
                        cancelAdapter2.DeleteCommand.ExecuteNonQuery();
                        conn.Close();
                        currServRend++;
                    
                    }

                    clearContents();
                    this.Close();

                }


            } else {

                this.Close();

            }
        }


        private void clearContents() 
        {
            patientId = "";
            refNo = 0;
            appDetId = "";
            servId = "";
            
        }

        


    }
}
