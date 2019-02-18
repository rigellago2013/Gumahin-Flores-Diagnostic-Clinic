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
    public partial class PaymentRecord : Form
    {
        //database connection
        DataTable dataTable;
        int totalRec;
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);

        public static int app_id;
        public static string ref_id, patient_name, app_date, total;

        public PaymentRecord()
        {
            InitializeComponent();
        }

        private void PaymentRecord_Load(object sender, EventArgs e)
        {
            dateTimePickerFrom.Value = DateTime.Today;
            dateTimePickerTo.Value = DateTime.Today;

            loadDataTable();

            //grid buttons
            DataGridViewButtonColumn btnView = new DataGridViewButtonColumn();
            dataGridViewPayment.Columns.Add(btnView);
            btnView.HeaderText = "";
            btnView.Name = "btnView";
            btnView.Text = "View";
            btnView.UseColumnTextForButtonValue = true;
        }

        private void loadDataTable()
        {
            string searchString = "SELECT A.ID, P.ID, A.app_date, A.refid, P.fname, P.lname, A.discount, A.total, A.remarks FROM (appointment A INNER JOIN patients P ON A.patient_id = P.ID) WHERE A.app_date BETWEEN #" + dateTimePickerFrom.Value.ToShortDateString() + "# AND #" + dateTimePickerTo.Value.ToShortDateString() + "# ORDER BY A.app_date ASC";

            DataSet ds = new DataSet();
            OleDbDataAdapter searchAdapter = new OleDbDataAdapter(searchString, conn);

            searchAdapter.Fill(ds, "dtResult");
            dataTable = ds.Tables["dtResult"];

            totalRec = dataTable.Rows.Count;
            dataGridViewPayment.DataSource = dataTable;
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            loadDataTable();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            loadDataTable();
        }

        private void dataGridViewPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.ColumnIndex == 0)
            {
                app_id = Convert.ToInt32(dataGridViewPayment.Rows[e.RowIndex].Cells[1].Value.ToString());

                ref_id = dataGridViewPayment.Rows[e.RowIndex].Cells[4].Value.ToString();
                patient_name = dataGridViewPayment.Rows[e.RowIndex].Cells[6].Value.ToString() + ", " + dataGridViewPayment.Rows[e.RowIndex].Cells[5].Value.ToString();
                app_date = Convert.ToDateTime(dataGridViewPayment.Rows[e.RowIndex].Cells[3].Value.ToString()).ToString("MMMM dd, yyyy");
                total = dataGridViewPayment.Rows[e.RowIndex].Cells[8].Value.ToString();


                ViewPaymentRecord view = new ViewPaymentRecord(app_id);
                view.ShowDialog();
         
            }
        }

 



    }
}
