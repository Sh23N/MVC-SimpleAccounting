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
using Acconting.ViewModles.Customers;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;

namespace Accounting.App
{
    public partial class FormReport : Form
    {
        public int TypeID = 0;
        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModle> list = new List<ListCustomerViewModle>();
                list.Add(new ListCustomerViewModle()
                {
                    CustomerID = 0,
                    FullName = "انتخاب کنید"
                });
                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }

            if (TypeID == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
        }



        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;
                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccGenericRepository.Get(a => a.TypeID == TypeID && a.CostomerID == customerId));
                }
                else
                {
                    result.AddRange(db.AccGenericRepository.Get(a => a.TypeID == TypeID));

                    if (txtFromDate.Text != "    /  /")
                    {
                        startDate = Convert.ToDateTime(txtFromDate);
                        startDate = DateConvertor.ToMiladi(startDate.Value);
                        result = result.Where(r => r.DateTime >= startDate.Value).ToList();
                    }

                    if (txtToDate.Text != "    /  /")
                    {
                        endDate = Convert.ToDateTime(txtFromDate);
                        endDate = DateConvertor.ToMiladi(endDate.Value);
                        result = result.Where(r => r.DateTime <=endDate.Value).ToList();
                    }
                }
                dgReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CostomerID);
                    dgReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTime, accounting.Description);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (MessageBox.Show("آیا از حذف مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccGenericRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                FormNewTransAction frmNew = new FormNewTransAction();
                frmNew.AccountID = id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtPrint.Rows.Add(
                   // item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString(),
                    item.Cells[4].Value.ToString()
                );
            }
            stiPrint.Load(Application.StartupPath+"/Report.mrt");
            stiPrint.RegData("DT",dtPrint);
            stiPrint.Show();

        }
    }
}
