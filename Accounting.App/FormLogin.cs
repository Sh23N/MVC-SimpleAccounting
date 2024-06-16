using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.Context;
using ValidationComponents;

namespace Accounting.App
{
    public partial class FormLogin : Form
    {
        public bool isEdit = false;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db=new UnitOfWork())
                {
                    if (isEdit)
                    {
                        var login = db.LoginRepository.Get().First();
                        login.UserName = txtUserName.Text;
                        login.Password = txtPassword.Text;
                        db.LoginRepository.Update(login);
                        db.Save();
                        Application.Restart();
                    }
                    else
                    {


                        if (db.LoginRepository
                            .Get(l => l.UserName == txtUserName.Text && l.Password == txtPassword.Text)
                            .Any())
                        {
                            DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("کاربری با این اطلاعات یافت نشد");
                            txtPassword.Text = "";
                            txtUserName.Text = "";
                        }
                    }
                }

            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            if (isEdit)
            {
                this.Text = "تنظیمات ورود به برنامه ";
                btnLogin.Text = "اعمال تغییرات";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var login = db.LoginRepository.Get().First();
                    txtUserName.Text=login.UserName;
                    txtPassword.Text=login.Password;
                }
            }
        }
    }
}
