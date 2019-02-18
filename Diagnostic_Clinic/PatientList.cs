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
    public partial class PatientList : Form
    {
        DataTable dataTable, dtEmpty;
        int currRec = 0;
        int totalRec = 0;
        int close = 0;

        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source= diagnostic.accdb";
        protected string pId, fname, lname, mi, gender, birthday, address, contact, civilStat;

        OleDbConnection conn = new OleDbConnection(connectionString);
        public PatientList()
        {
            InitializeComponent();
        }

        private void Retrieve()
        {
            DataSet ds = new DataSet();

            string commandString = "SELECT ID, fname as First_Name,lname as Last_Name,mid_initial as M_I,gender as Gender,birthday as Birthday,address as Address,contact as Contact_Number,civil_status as Civil_Status from patients ORDER BY ID ASC";

            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandString, conn);

            dataAdapter.Fill(ds, "prog");
            dataTable = ds.Tables["prog"];

            currRec = 0;

            totalRec = dataTable.Rows.Count;

            patientData.DataSource = dataTable;
            patientData.Columns["ID"].Visible = false;

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewPatient newPatient = new NewPatient();
            newPatient.MdiParent = this.ParentForm;
            newPatient.StartPosition = FormStartPosition.Manual;
            newPatient.Location = new Point(400, 50);
            newPatient.Show();
            this.Close();
        }

        private void PatientList_Load(object sender, EventArgs e)
        {
            Retrieve();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      

        private void PatientList_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
            }
            catch
            {

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
         
                string commandString = "select * from patients where fname like '%" + txtSearch.Text + "%' OR lname like '%"
                   + txtSearch.Text + "%' order by ID ASC";


                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandString, conn);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds, "prog");

                DataTable dataTableq = ds.Tables["prog"];
                currRec = 0;
                totalRec = dataTableq.Rows.Count;

                if (totalRec != 0)
                {
                    patientData.DataSource = dataTableq;
                }
                else {
                    patientData.DataSource = dtEmpty;
                }
              

        
        }

        private void patientData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.pId = patientData.SelectedRows[0].Cells[0].Value.ToString();
            this.fname = patientData.SelectedRows[0].Cells[1].Value.ToString();
            this.lname = patientData.SelectedRows[0].Cells[2].Value.ToString();
            this.mi = patientData.SelectedRows[0].Cells[3].Value.ToString();
            this.gender = patientData.SelectedRows[0].Cells[4].Value.ToString();
            this.birthday = patientData.SelectedRows[0].Cells[5].Value.ToString();
            this.address = patientData.SelectedRows[0].Cells[6].Value.ToString();
            this.contact = patientData.SelectedRows[0].Cells[7].Value.ToString();
            this.civilStat = patientData.SelectedRows[0].Cells[8].Value.ToString();

            EditPatient editPatient = new EditPatient(pId, fname, lname, mi, gender, birthday, address, contact, civilStat);
            editPatient.MdiParent = this.ParentForm;
            editPatient.StartPosition = FormStartPosition.Manual;
            editPatient.Location = new Point(400, 50);
            editPatient.Show();
            this.Close();
        }

    
   


    }
  

    
    
}
