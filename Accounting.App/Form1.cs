using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acconting.Utility;
using Acconting.ViewModles.Accounting;
using Accounting.Business;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin frmLogin = new FormLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {   this.Show();
                lblDate.Text = DateConvertor.ToShamsi(DateTime.Now);
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                Report();
            }
            else
            {
                Application.Exit();
            }
        }

        void Report()
        {
            ReportViewModel report = Account.ReportFormMain();
            lblPay.Text = report.Pay.ToString("#,0");
            lblRecive.Text = report.Recive.ToString("#,0");
            lblMaintain.Text = report.AccountBalance.ToString("#,0");
        }
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            FormCustomers frmCustomers = new FormCustomers();
            frmCustomers.ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e)
        {
            FormNewTransAction frmNewTransAction=new FormNewTransAction();
            frmNewTransAction.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            FormReport frmReport=new FormReport();
            frmReport.TypeID = 2;
            frmReport.ShowDialog();
        }

        private void btnReportRecive_Click(object sender, EventArgs e)
        {
            FormReport frmReport = new FormReport();
            frmReport.TypeID = 1;
            frmReport.ShowDialog();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnEditLogin_Click(object sender, EventArgs e)
        {
            FormLogin frmLogin = new FormLogin();
            frmLogin.isEdit=true;
            frmLogin.ShowDialog();
        }

       
    }
}
