using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.Context;
using ValidationComponents;

namespace Accounting.App
{
    public partial class FormNewTransAction : Form
    {
        private UnitOfWork db;
        public int AccountID = 0;
        public FormNewTransAction()
        {
            InitializeComponent();
        }

        private void FormNewTransAction_Load(object sender, EventArgs e)
        {
            UnitOfWork db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();
            if (AccountID != 0)
            {
                var account = db.AccGenericRepository.GetById(AccountID);
                txtAmount.Text = account.Amount.ToString();
                txtDescription .Text= account.Description.ToString();
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CostomerID);
                if (account.TypeID == 1)
                {
                    rbRecive.Checked = true;
                }
                else
                {
                    rbPay.Checked = true;
                }

                this.Text = "ویرایش";
                btnSave.Text = "ویرایش";
                db.Dispose();
            }
        }

        private void textFilter_TextChanged(object sender, EventArgs e)
        {

            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(textFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbPay.Checked || rbRecive.Checked)
                {
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CostomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        TypeID = (rbRecive.Checked)?1:2,
                        DateTime = DateTime.Now,
                        Description = txtDescription.Text
                    };
                    if (AccountID == 0)
                    {
                        db.AccGenericRepository.Insert(accounting);
                    }
                    else
                    {
                        accounting.ID=AccountID;
                        db.AccGenericRepository.Update(accounting);
                    }
                    db.Save();
                    db.Dispose();
                    DialogResult= DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("لفا نوع تراکنش را انتخاب کنید");
                }

            }
        }
    }
}
