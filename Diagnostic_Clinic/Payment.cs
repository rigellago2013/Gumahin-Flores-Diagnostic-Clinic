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
    public partial class Payment : Form
    {
        static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=diagnostic.accdb";
        OleDbConnection conn = new OleDbConnection(connectionString);

        public double totalAmt, finalAmt, discount, receivedAmt, change;
        public string patId;
        public int refId;

        public Payment(string patientId, double totalAmount, int refNo)
        {
            InitializeComponent();
            totalAmt = totalAmount;
            finalAmt = totalAmount;
            patId = patientId;
            refId = refNo;
          
        }

        private void Payment_Load(object sender, EventArgs e)
        {         
            label3.Text = "Cash Recieved:";
            lblAmount.Text = Convert.ToString(totalAmt) + ".00";
            lblTotalCharges.Text = Convert.ToString(totalAmt) + ".00";
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (txtAmount.Text != "")
            {
                if (txtAmount.Text != lblTotalCharges.Text)
                {
               


                    OleDbDataAdapter daAppointPayment = new OleDbDataAdapter();
                    conn.Open();
                    string sql = "UPDATE appointment SET total =" + totalAmt + ", patient_id =" + Convert.ToInt32(patId) + ", received_amount =" + receivedAmt + ", change =" + change + " WHERE refid =" + refId;
                    daAppointPayment.UpdateCommand = new OleDbCommand(sql, conn);
                    daAppointPayment.UpdateCommand.ExecuteNonQuery();
                    conn.Close();
                    this.Close();
                    Receipt rct = new Receipt(this.refId);
                    rct.StartPosition = FormStartPosition.Manual;
                    rct.Location = new Point(240, 25);
                    rct.Show();
                    MessageBox.Show("Transaction successful! Press (ctrl+p) to print Receipt.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                 

                } else {

                    MessageBox.Show("Please fill in the exact amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            
            }
            else {

                MessageBox.Show("Please fill in the exact amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            string discount = "0." + txtDiscount.Text;

            this.discount = totalAmt * Convert.ToDouble(discount);

            this.finalAmt = totalAmt - this.discount;

            lblTotalCharges.Text = Convert.ToString(finalAmt) + ".00";

        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text != String.Empty)
            {
                this.receivedAmt = Convert.ToDouble(txtAmount.Text);

                if (this.receivedAmt >= this.finalAmt)
                {
                    change = this.receivedAmt - this.finalAmt;
                    lblChange.Text = Convert.ToString(change);
                    btnProceed.Enabled = true;
                }
                else
                {
                    lblChange.Text = "Insufficient Amount.";
                    btnProceed.Enabled = false;
                }
            }
        }

     
            
    }
}
