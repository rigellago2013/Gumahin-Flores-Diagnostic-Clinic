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
    public partial class Receipt : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);
        DataTable dtAppointment;
        int allApp, refCode, refid;
        string fname, lname, mi;
        object total, ra, change;
        DateTime appdate;
        public Receipt(int refId)
        {
            InitializeComponent();

            this.refCode = refId;
        }

        private void Receipt_Load(object sender, EventArgs e)
        {
            getAppointmentDetails();
        }

        Bitmap bmp;

        private void printReceipt_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void getAppointmentDetails()
        {
            OleDbDataReader reader = null;
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("SELECT P.fname as fname, P.lname as lname, P.mid_initial as mi, A.refid as refid, A.total as totalamt, A.received_amount as ra,A.change as change, A.app_date as appdate FROM services S INNER JOIN (patients P INNER JOIN (appointment A INNER JOIN appointment_details AD ON A.refid = AD.refid) ON P.ID = A.patient_id) ON S.ID = AD.serv_id WHERE A.refid ="+ refCode, conn);

            cmd.Parameters.AddWithValue("@fname", fname);
            cmd.Parameters.AddWithValue("@lname", lname);
            cmd.Parameters.AddWithValue("@mi", mi);
            cmd.Parameters.AddWithValue("@refid", refid);
            cmd.Parameters.AddWithValue("@totalamt", total);
            cmd.Parameters.AddWithValue("@ra", ra);
            cmd.Parameters.AddWithValue("@change", change);
            cmd.Parameters.AddWithValue("@appdate", appdate);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
               fname = reader.GetString(reader.GetOrdinal("fname"));
               lname = reader.GetString(reader.GetOrdinal("lname"));
               mi = reader.GetString(reader.GetOrdinal("mi"));
               refid = reader.GetInt32(reader.GetOrdinal("refid"));
               total = reader.GetValue(reader.GetOrdinal("totalamt"));
               ra = reader.GetValue(reader.GetOrdinal("ra"));
               change = reader.GetValue(reader.GetOrdinal("change"));
               appdate = reader.GetDateTime(reader.GetOrdinal("appdate"));
            }
            conn.Close();

            lblPatientName.Text = fname+" "+mi+" "+lname;
            lblRefCode.Text = Convert.ToString(refCode);
            lblReceivedAmount.Text = Convert.ToString(ra);
            lblChange.Text = Convert.ToString(change);
            lblTotalAmt.Text = Convert.ToString(total);
            lblDate.Text = Convert.ToDateTime(appdate).ToShortDateString();
    
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            if (e.Control && e.KeyCode == Keys.P)
            {
                Graphics g = this.CreateGraphics();
                bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
                Graphics mg = Graphics.FromImage(bmp);
                mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
                printReceiptDialog.ShowDialog();
                printReceipt.Print();
                this.Close();
     
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

 

 
  
  

     
    }
}
