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
    public partial class Findings : Form
    {
        DataTable dataTable, dtEmpty;
        int totalRec;
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);

        public static int appd_id;
        public static string ref_id, patient_name, app_date, service;

        public Findings()
        {
            InitializeComponent();
        }

        public void Findings_Load(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Today;

            fillCombo();
            loadDataTable();
            loadGridButtons();
        }

        private void fillCombo()
        {
            //fill services combo box
            conn.Open();
            DataSet ds = new DataSet();

            string searchString = "SELECT * FROM services";
            OleDbDataAdapter searchAdapter = new OleDbDataAdapter(searchString, conn);

            searchAdapter.Fill(ds);

            cboService.DataSource = ds.Tables[0];
            cboService.DisplayMember = "name";
            cboService.ValueMember = "ID";

            //conn.Close();
        }

        private void cboService_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDataTable();
        }

        public void loadDataTable()
        {
            //check selected combo box value
            int cboVal;
            try
            {
                cboVal = Convert.ToInt16(((DataRowView)cboService.SelectedItem).Row["ID"]);
            }
            catch
            {
                cboVal = 0;
            }
            
            string searchString;

            if (cboVal > 0)
                //select appointment record of specific service
                searchString = "SELECT D.ID, A.refid, A.app_date, A.total, A.remarks, A.discount, P.fname, P.lname, S.name, S.rate FROM (((appointment A INNER JOIN appointment_details D ON D.app_id = A.ID) INNER JOIN patients P ON A.patient_id = P.ID) INNER JOIN services S ON D.serv_id = S.ID) WHERE D.findings IS NULL AND (D.serv_id = " + cboVal + " AND A.app_date = #" + dtpDate.Value.ToShortDateString() + "#)";
            else
                //select general
                searchString = "SELECT D.ID, A.refid, A.app_date, A.total, A.remarks, A.discount, P.fname, P.lname, S.name, S.rate FROM (((appointment A INNER JOIN appointment_details D ON D.app_id = A.ID) INNER JOIN patients P ON A.patient_id = P.ID) INNER JOIN services S ON D.serv_id = S.ID) WHERE D.findings IS NULL AND A.app_date = #" + dtpDate.Value.ToShortDateString() + "#";
            
            DataSet ds = new DataSet();
            OleDbDataAdapter searchAdapter = new OleDbDataAdapter(searchString, conn);

            searchAdapter.Fill(ds, "dtResult");
            dataTable = ds.Tables["dtResult"];

            totalRec = dataTable.Rows.Count;
            if (totalRec != 0)
            {
                dataGridViewFindings.DataSource = dataTable;
            }
            else
            {
                dataGridViewFindings.DataSource = dtEmpty;
            }
            
        }

        private void loadGridButtons()
        {
            //grid buttons
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            dataGridViewFindings.Columns.Add(btnView);
            btnView.HeaderText = "";
            btnView.Name = "btnAdd";
            btnView.Text = "Add";
            btnView.UseColumnTextForButtonValue = true;
        }

        private void dataGridViewFindings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == 0)
            {
                appd_id = Convert.ToInt32(dataGridViewFindings.Rows[e.RowIndex].Cells[1].Value.ToString());

                ref_id = dataGridViewFindings.Rows[e.RowIndex].Cells[2].Value.ToString();
                patient_name = dataGridViewFindings.Rows[e.RowIndex].Cells[8].Value.ToString() + ", " + dataGridViewFindings.Rows[e.RowIndex].Cells[7].Value.ToString();
                app_date = Convert.ToDateTime(dataGridViewFindings.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("MMMM dd, yyyy");
                service = dataGridViewFindings.Rows[e.RowIndex].Cells[9].Value.ToString();

                AddFindings addFindings = new AddFindings(appd_id);
                addFindings.ShowDialog();

                this.Close();
            }
        
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            loadDataTable();
        }
    }
}
