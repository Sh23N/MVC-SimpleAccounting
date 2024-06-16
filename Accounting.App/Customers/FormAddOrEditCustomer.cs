using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using ValidationComponents;

namespace Accounting.App
{
    public partial class FormAddOrEditCustomer : Form
    {
        public int customerId = 0;
        private UnitOfWork db = new UnitOfWork();
        public FormAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation=openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path+imageName);
                Customers customers = new Customers()
                {
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    Mobile = txtMobile.Text,
                    FullName = txtName.Text,
                    CustomerImage = imageName
                };
                if (customerId==0)
                {
                    db.CustomerRepository.InsertCustomer(customers);
                }
                else
                {
                    customers.CustomersID=customerId;
                    db.CustomerRepository.UpdateCustomer(customers);
                }
                db.Save();
                DialogResult=DialogResult.OK;
            }
            
               
            
        }

        private void FormAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {
                this.Text = "ویرایش شخص";
                btnSave.Text = "ویرایش";
                var customer = db.CustomerRepository.GetCustomersById(customerId);
                txtEmail.Text=customer.Email;
                txtAddress.Text=customer.Address;
                txtName.Text = customer.FullName;
                txtMobile.Text=customer.Mobile;
                pcCustomer.ImageLocation = Application.StartupPath + "/Images" + customer.CustomerImage;
            }
        }
    }
}
