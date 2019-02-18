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
    public partial class Services : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        DataTable dtAllServices, dtGetService;
        public int allService, searchService;
        public string sId, sName, sRate;

        public Services()
        {
            InitializeComponent();
        }

        private void Services_Load(object sender, EventArgs e)
        {
            allServices();
        }

        private void allServices()
        {
            DataSet dsAllSevices = new DataSet();
            string sql = "SELECT ID, name, rate FROM services ORDER BY ID";
            conn.Open();
            OleDbDataAdapter daAllServices = new OleDbDataAdapter(sql, conn);
            daAllServices.Fill(dsAllSevices, "allServices");
            dtAllServices = dsAllSevices.Tables["allServices"];
            allService = dtAllServices.Rows.Count;
            dgvServiceList.DataSource = dtAllServices;
            conn.Close();
            dgvServiceList.Columns["ID"].Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataSet dsSearchService = new DataSet();
            string sql = "SELECT * FROM services WHERE name LIKE '%" + txtSearch.Text +"%'";
            OleDbDataAdapter daSearchPatient = new OleDbDataAdapter(sql, conn);
            daSearchPatient.Fill(dsSearchService, "searchService");
            dtGetService = dsSearchService.Tables["searchService"];
            searchService = dtGetService.Rows.Count;
            dgvServiceList.DataSource = dtGetService;
        }

        private void newServe_Click(object sender, EventArgs e)
        {
           NewService newServ = new NewService();
             newServ.ShowDialog(); 
             allServices();
        }

        private void dgvServiceList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sId = dgvServiceList.SelectedRows[0].Cells[0].Value.ToString();
            sName = dgvServiceList.SelectedRows[0].Cells[1].Value.ToString();
            sRate = dgvServiceList.SelectedRows[0].Cells[2].Value.ToString();

            EditService newServ = new EditService(sId,sName,sRate);
            newServ.ShowDialog();
            allServices();
        }

  

       


      
    }
}
