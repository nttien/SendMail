using System;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace SendMail
{
    public class clsSendMail
    {
        public static string getUniqueID(string drive)
        {
            string volumeSerial = "";
            string cpuID = "";
            try
            {
                if (drive == string.Empty)
                {
                    //Find first drive
                    foreach (DriveInfo compDrive in DriveInfo.GetDrives())
                    {
                        if (compDrive.IsReady)
                        {
                            drive = compDrive.RootDirectory.ToString();
                            break;
                        }
                    }
                }

                if (drive.EndsWith(":\\"))
                {
                    //C:\ -> C
                    drive = drive.Substring(0, drive.Length - 2);
                }

                volumeSerial = getVolumeSerial(drive);
                cpuID = getCPUID();
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }

            //Mix them up and remove some useless 0's
            return cpuID.Substring(13) + cpuID.Substring(1, 4) + volumeSerial + cpuID.Substring(4, 4);
        }

        //Get volume serial
        private static string getVolumeSerial(string drive)
        {
            string volumeSerial = "";
            try
            {
                ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                disk.Get();

                volumeSerial = disk["VolumeSerialNumber"].ToString();
                disk.Dispose();
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }

            return volumeSerial;
        }

        //Get CPU id
        private static string getCPUID()
        {
            string cpuInfo = "";
            try
            {
                ManagementClass managClass = new ManagementClass("win32_processor");
                ManagementObjectCollection managCollec = managClass.GetInstances();

                foreach (ManagementObject managObj in managCollec)
                {
                    if (cpuInfo == "")
                    {
                        //Get only the first CPU's ID
                        cpuInfo = managObj.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }

            return cpuInfo;
        }

        //Generate Key
        public static string GenerateKey()
        {
            string key = "";
            try
            {
                Random rn = new Random();
                string charsToUse = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                MatchEvaluator RandomChar = delegate (Match m)
                {
                    return charsToUse[rn.Next(charsToUse.Length)].ToString();
                };

                key = Regex.Replace("XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX", "X", RandomChar);
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
            return key;
        }

        //Send mail
        public static void SendMail(string sendemail, string name, string key)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.sendgrid.net");

                mail.From = new MailAddress("nttien1989@gmail.com");
                mail.To.Add(sendemail);

                mail.Subject = "Facebook Auto Feeder license";
                mail.IsBodyHtml = true;
                mail.Body = "<p align='justify'>Dear <b>" + name + "</b>, <br><br>Thank you for contacting us for nttien1989@gmail license account!<br><br>"
                            + "Here below is your account information:<br><br><ul>Your ID: <b><font size='+1'>nttien1989@gmail.com</font></b><br>"
                            + "Your license key number is: <b><font size='+1'>" + key + "</font></b><br></ul><br>"
                            + "If you want to subsribe our product, please submit your subscription at nttien1989@gmail.com<br><br>"
                            + "Cheer,<br><b>" + name + "</b></p>"; ;

                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("tienn", "9502815tien");
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);
            }
            catch (Exception _err)
            {
                clsUtilities.ShowMessageError("Error: " + _err.Message);
            }
        }
    }
}
