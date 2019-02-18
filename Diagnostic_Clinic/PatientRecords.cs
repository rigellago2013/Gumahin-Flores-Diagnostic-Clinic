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
    public partial class PatientRecords : Form
    {
        DataTable dataTable, dtEmpty;
        int currRec = 0;
        int totalRec = 0;
        int close = 0;
        public string id, lastName, finder;

        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source= diagnostic.accdb";

        OleDbConnection conn = new OleDbConnection(connectionString);
        public PatientRecords()
        {
            InitializeComponent();
        }



        private void FillRecords()
        {

            txtLname.Text = dataTable.Rows[currRec]["lname"].ToString();
            txtFname.Text = dataTable.Rows[currRec]["fname"].ToString();
            txtMI.Text = dataTable.Rows[currRec]["mid_initial"].ToString();
            txtAddress.Text = dataTable.Rows[currRec]["address"].ToString();
            bday.Text = dataTable.Rows[currRec]["birthday"].ToString();
            txtGender.Text = dataTable.Rows[currRec]["gender"].ToString();
            txtStatus.Text = dataTable.Rows[currRec]["civil_status"].ToString();
            txtContact.Text = dataTable.Rows[currRec]["contact"].ToString();
            ID2.Text = dataTable.Rows[currRec]["ID"].ToString();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void allPatient()
        {
            string command = "SELECT lname as Last_Name,fname as First_Name,mid_initial as M_I,address as Address,birthday as Birthday,contact as Contact_Number,gender as Gender,civil_status as Civil_Status FROM patients where fname LIKE '%" + txtSearch.Text + "%' OR lname like '%" + txtSearch.Text + "%' OR mid_initial like '%" + txtSearch.Text + "%' ORDER BY ID ASC";
            OleDbDataAdapter displayData = new OleDbDataAdapter(command, conn);
            DataSet dsq = new DataSet();
            displayData.Fill(dsq, "progq");

            dataTable = dsq.Tables["progq"];
            currRec = 0;
            totalRec = dataTable.Rows.Count;

            if (totalRec != 0)
            {

                pData.DataSource = dataTable;

            }
            else {

                pData.DataSource = dtEmpty;
            }
            
        
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string command = "SELECT lname as Last_Name,fname as First_Name,mid_initial as M_I,address as Address,birthday as Birthday,contact as Contact_Number,gender as Gender,civil_status as Civil_Status FROM patients where fname LIKE '%" + txtSearch.Text + "%' OR lname like '%" + txtSearch.Text + "%' OR mid_initial like '%" + txtSearch.Text + "%' ORDER BY ID ASC";
            OleDbDataAdapter displayData = new OleDbDataAdapter(command, conn);
            DataSet dsq = new DataSet();
            displayData.Fill(dsq, "progq");

            dataTable = dsq.Tables["progq"];
            currRec = 0;
            totalRec = dataTable.Rows.Count;

            pData.DataSource = dataTable;
        }

        private void pData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID.Text = pData.SelectedRows[0].Cells[0].Value.ToString();

            string commands = "SELECT * FROM patients where lname ='" + ID.Text + "'";
            OleDbDataAdapter displayData2 = new OleDbDataAdapter(commands, conn);
            DataSet dsqs = new DataSet();
            displayData2.Fill(dsqs, "progqs");

            dataTable = dsqs.Tables["progqs"];
            currRec = 0;
            totalRec = dataTable.Rows.Count;

            if (totalRec > 0)
                FillRecords();

            string commandString = "SELECT app_date as App_Date,name as Service,findings as Findings,total as Total FROM services INNER JOIN (patients INNER JOIN (appointment INNER JOIN appointment_details ON appointment.ID = appointment_details.app_id) ON patients.ID = appointment.patient_id) ON services.ID = appointment_details.serv_id WHERE patients.ID = " + ID2.Text;



            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandString, conn);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds, "prog");

            DataTable dataTableq = ds.Tables["prog"];
            currRec = 0;
            totalRec = dataTableq.Rows.Count;

            recordData.DataSource = dataTableq;


            string cString = "SELECT app_date as App_Date,total as Total_Charges,remarks as Remarks,discount as Discounted, received_amount, change FROM services INNER JOIN (patients INNER JOIN (appointment INNER JOIN appointment_details ON appointment.ID = appointment_details.app_id) ON patients.ID = appointment.patient_id) ON services.ID = appointment_details.serv_id WHERE patients.ID = " + ID2.Text;


            OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter(cString, conn);
            DataSet dss = new DataSet();
            dataAdapter2.Fill(dss, "progw");

            DataTable dataTableqq = dss.Tables["progw"];
            currRec = 0;
            totalRec = dataTableqq.Rows.Count;

            paymentData.DataSource = dataTableqq;

            lblName.Text = txtLname.Text + ", " + txtFname.Text + ", " + txtMI.Text + ".";
        }

        private void PatientRecords_Load(object sender, EventArgs e)
        {
            allPatient();
        }

  
 

 


    }
}
