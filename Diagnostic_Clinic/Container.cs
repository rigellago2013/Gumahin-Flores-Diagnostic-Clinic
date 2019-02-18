using Diagnostic_Clinic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diagnostic_Clinic
{
    public partial class Container : Form
    {
        private int childFormNumber = 0;
        public static int refCode;
        protected string fname, lname, contact;
        protected int id;


        public Container(int id, string fname, string lname, string contact)
        {
            InitializeComponent();
            this.fname = fname;
            this.lname = lname;
            this.contact = contact;
            this.id = id;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                randomNumber();
                ActiveMdiChild.Close();
                Appointment consultationForm = new Appointment(refCode);
                consultationForm.MdiParent = this;
                consultationForm.StartPosition = FormStartPosition.Manual;
                consultationForm.Location = new Point(230, 0);
                consultationForm.Show();
            }
            catch
            {
                randomNumber();
                Appointment consultationForm = new Appointment(refCode);
                consultationForm.MdiParent = this;
                consultationForm.StartPosition = FormStartPosition.Manual;
                consultationForm.Location = new Point(230, 0);
                consultationForm.Show();
             }
        }


        private void Container_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnPatientList_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                PatientList pl = new PatientList();
                pl.MdiParent = this;
                pl.StartPosition = FormStartPosition.Manual;
                pl.Location = new Point(230, 0);
                pl.Show();
            }
            catch
            {
                PatientList pl = new PatientList();
                pl.MdiParent = this;
                pl.StartPosition = FormStartPosition.Manual;
                pl.Location = new Point(230, 0);
                pl.Show();
            }

        }
  
        public void randomNumber()
        {
            Random rnd = new Random();
            refCode = rnd.Next(1, 5000);
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                Services serveList = new Services();
                serveList.MdiParent = this;
                serveList.StartPosition = FormStartPosition.Manual;
                serveList.Location = new Point(230, 0);
                serveList.Show();
            }
            catch
            {
                Services serveList = new Services();
                serveList.MdiParent = this;
                serveList.StartPosition = FormStartPosition.Manual;
                serveList.Location = new Point(230, 0);
                serveList.Show();
            }

        }

        private void btnUserSettings_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                UserSettings us = new UserSettings(this.id);
                us.MdiParent = this;
                us.StartPosition = FormStartPosition.Manual;
                us.Location = new Point(230, 0);
                us.Show();
            }
            catch
            {
                UserSettings us = new UserSettings(this.id);
                us.MdiParent = this;
                us.StartPosition = FormStartPosition.Manual;
                us.Location = new Point(230, 0);
                us.Show();
            }
        }

        private void btnPaymentRecords_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                PaymentRecord paymentRecord = new PaymentRecord();
                paymentRecord.MdiParent = this;
                paymentRecord.StartPosition = FormStartPosition.Manual;
                paymentRecord.Location = new Point(230, 0);
                paymentRecord.Show();
            }
            catch
            {
                PaymentRecord paymentRecord = new PaymentRecord();
                paymentRecord.MdiParent = this;
                paymentRecord.StartPosition = FormStartPosition.Manual;
                paymentRecord.Location = new Point(230, 0);
                paymentRecord.Show();
            }
        }

        private void btnFindings_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                Findings findings = new Findings();
                findings.MdiParent = this;
                findings.StartPosition = FormStartPosition.Manual;
                findings.Location = new Point(230, 0);
                findings.Show();
            }
            catch
            {
                Findings findings = new Findings();
                findings.MdiParent = this;
                findings.StartPosition = FormStartPosition.Manual;
                findings.Location = new Point(230, 0);
                findings.Show();
            }
        }

        private void btnPatientRecords_Click(object sender, EventArgs e)
        {
            try
            {
                ActiveMdiChild.Close();
                PatientRecords pr = new PatientRecords();
                pr.MdiParent = this;
                pr.StartPosition = FormStartPosition.Manual;
                pr.Location = new Point(230, 0);
                pr.Show();
            }
            catch
            {
                PatientRecords pr = new PatientRecords();
                pr.MdiParent = this;
                pr.StartPosition = FormStartPosition.Manual;
                pr.Location = new Point(230, 0);
                pr.Show();
            }
        }

        private void Container_Load(object sender, EventArgs e)
        {
            lblUserName.Text = fname + " " + lname + "!";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult logout = MessageBox.Show("Are you sure you want to log-out?","Log-out confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (logout == DialogResult.Yes) {

                Logout();
            }
        }


        private void Logout() {

            Application.Restart();
            
        }

   
  
    }
}
