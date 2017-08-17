using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SendMail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFirst.Text == "")
                {
                    clsUtilities.ShowMessageError("First name not empty");
                    txtFirst.Focus();
                }
                else
                    if (txtLast.Text == "")
                {
                    clsUtilities.ShowMessageError("Last name not empty");
                    txtLast.Focus();
                }
                else
                    if (txtEmail.Text == "")
                {
                    clsUtilities.ShowMessageError("Email not empty");
                    txtEmail.Focus();
                }
                else
                    if (txtPassword.Text == "")
                {
                    clsUtilities.ShowMessageError("Password not empty");
                    txtPassword.Focus();
                }
                else
                    if (txtConfirm.Text == "")
                {
                    clsUtilities.ShowMessageError("Confirm password not empty");
                    txtConfirm.Focus();
                }
                else
                    if (!Regex.IsMatch(txtEmail.Text.Trim(), @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                {
                    clsUtilities.ShowMessageError("Email invalid");
                    txtEmail.Focus();
                }
                else
                    if (txtPassword.Text != txtConfirm.Text)
                {
                    clsUtilities.ShowMessageError("Password must be Confirmed");
                    txtConfirm.Focus();
                }
                else
                    if (clsConnect.checkEmail(txtEmail.Text))
                {
                    clsUtilities.ShowMessageError("Email is registered");
                    txtEmail.Focus();
                }
                else
                {
                    objLicense objL = new objLicense();
                    objL.email = txtEmail.Text.Trim();
                    objL.userName = txtFirst.Text.Trim() + " " + txtLast.Text.Trim();
                    objL.password = txtPassword.Text.Trim();
                    objL.key = clsSendMail.GenerateKey().Trim();
                    objL.hardwareId = null;
                    objL.countDate = 0;

                    clsConnect.Insert(objL);
                    clsUtilities.ShowMessageInformation("Thanks for your register, please check the email to revice license key.");
                    //clsSendMail.SendMail(txtEmail.Text, txtFirst.Text + " " + txtLast.Text, objL.key);

                    //txtEmail.Clear();
                    //txtFirst.Clear();
                    //txtLast.Clear();
                    //txtPassword.Clear();
                    //txtConfirm.Clear();
                    //txtFirst.Focus();
                }
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtchkEmail.Text == "")
                {
                    clsUtilities.ShowMessageError("Email not empty");
                    txtchkEmail.Focus();
                }
                else
                   if (txtlicenseKey.Text == "")
                {
                    clsUtilities.ShowMessageError("License not empty");
                    txtlicenseKey.Focus();
                }
                else
                   if (!clsConnect.checkEmail(txtchkEmail.Text))
                {
                    clsUtilities.ShowMessageError("Email does NOT register. Please register for this email.");
                    txtchkEmail.Focus();
                }
                else
                   if (!clsConnect.checkLicense(txtlicenseKey.Text))
                {
                    clsUtilities.ShowMessageError("License invalid");
                    txtlicenseKey.Focus();
                }
                else
                {
                    if (!clsConnect.checkHardwareID(clsSendMail.getUniqueID("C")))
                    {
                        clsUtilities.ShowMessageError("License is registered.");
                        txtlicenseKey.Focus();
                    }
                    else
                    {
                        objLicense objL = new objLicense();
                        objL.email = txtchkEmail.Text;
                        objL.key = txtlicenseKey.Text;
                        objL.hardwareId = clsSendMail.getUniqueID("C");

                        if (clsUtilities.ShowQuestionMessage("Are you sure apply this license?") == DialogResult.Yes)
                        {
                            clsConnect.Update(objL);
                            clsUtilities.ShowMessageInformation("Applied this license successful. You have 30 days to use application. Thanks for using this application.");
                            txtchkEmail.Clear();
                            txtlicenseKey.Clear();
                        }
                    }
                }
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
        }
    }
}
