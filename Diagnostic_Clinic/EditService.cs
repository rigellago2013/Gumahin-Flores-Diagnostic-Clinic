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
    public partial class EditService : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);

        public string sId, sName, sRate;

        public EditService(string id, string name, string rate)
        {
            InitializeComponent();

            sId = id;
            sName = name;
            sRate = rate;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OleDbDataAdapter addAdapter = new OleDbDataAdapter();
            string addSql = "UPDATE services SET rate =" +Convert.ToDouble(txtRate.Text)+ " WHERE ID=" + Convert.ToInt32(sId);
            conn.Open();
            addAdapter.InsertCommand = new OleDbCommand(addSql, conn);
            addAdapter.InsertCommand.ExecuteNonQuery();
            conn.Close();
            this.Close();
        }

        private void EditService_Load(object sender, EventArgs e)
        {
            lblName.Text = sName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtRate.Text = "";
            lblName.Text = "-";
            sId = "";
            this.Close();
        }
    }
}
