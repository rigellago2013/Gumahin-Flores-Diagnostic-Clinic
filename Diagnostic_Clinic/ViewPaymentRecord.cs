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
    public partial class ViewPaymentRecord : Form
    {
        //database connection
        DataTable dataTable;
        int totalRec, app_id;
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);

        public ViewPaymentRecord(int id)
        {
            InitializeComponent();
            this.app_id = id;
        }

        private void ViewPaymentRecord_Load(object sender, EventArgs e)
        {
            lblRef.Text = Convert.ToString(PaymentRecord.ref_id);
            lblPatient.Text = PaymentRecord.patient_name;
            lblDate.Text = PaymentRecord.app_date;
            lblTotal.Text = PaymentRecord.total;

            richTextBoxFindings.Enabled = false;

            loadDataTable();
     
        }

        private void loadDataTable()
        {
            try
            {
                string searchString = "SELECT D.findings, D.serv_id, S.name, D.amount FROM (appointment_details D INNER JOIN services S ON D.serv_id = S.ID) WHERE D.app_id ="+ app_id;

                DataSet ds = new DataSet();
                OleDbDataAdapter searchAdapter = new OleDbDataAdapter(searchString, conn);

                searchAdapter.Fill(ds, "dtResult");
                dataTable = ds.Tables["dtResult"];

                totalRec = dataTable.Rows.Count;
                dataGridViewAppDetails.DataSource = dataTable;
            }
            catch
            {
                MessageBox.Show("No records found", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridViewAppDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            richTextBoxFindings.Text = dataGridViewAppDetails.Rows[e.RowIndex].Cells[0].Value.ToString();
        }
    }
}
