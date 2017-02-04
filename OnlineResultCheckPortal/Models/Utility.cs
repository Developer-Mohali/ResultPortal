using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class Utility
    {
        public class Message
        {
           
            public const string TokenNotValid = "<font color=Red><b>Not valid Token Number.<b></font>";
            public const string TokenIdProvided= "<font color=Red><b>Token Id already provided.<b></font>";
            public const string TokenPurchase = "<font color=Red><b>Please purchase new token.<b></font>";
            public const string EndofTermExam = "<font color=Red><b>Registration number and school Id not valid.<b></font>";
            public const string MockExam = "<font color=Red><b>Registration number and school Id not valid.<b></font>";
            public const string JsceMessage = "<font color=Red><b>Registration number and school Id not valid.<b></font>";
            public const string Token = "<font color=Red><b>Token name already exists.<b></font>";
            public const string RegistrationNumber = "<font color=green><b>Registration number already exists.<b></font>";
            public const string RegistrationNumbers = "<font color=red><b>Registration number already exists.<b></font>";
            public const string PurchaseToken = "<font color=white><b>Purchase token successfully.<b></font>";
            public const string UnPurchaseToken = "<font color=white><b>Purchase token deactivate successfully.<b></font>";
            public const string AttachmentNotExist = "<font color=red><b>Attachment is not exist!<b></font>";
            public const string PasswordWrong = "<font color=red><b>Wrong password.<b></font>";
            public const string AuthenticationWrong = "<font color=red><b>Wrong Username/Password. Please try again.<b></font>";
            public const string SessionExpired = "<font color=red><b>Your session has expired. Please log-in again.<b></font>";
            public const string Add_Message = "<font color=Green><b> Record Saved Successfully.<b></font>";
            public const string Add_Register= "<font color=Green><b> Record Saved Successfully.<b></font>";
            public const string EmployeeNumberExist = "<font color=red><b>Employee Number already exists!<b></font>";
            public const string UserNameExist = "<font color=red><b>User name already exists!<b></font>";
            public const string EmailExist = "<font color=red><b>Email Address already exists!<b></font>";
            public const string Update_Message = "<font color=Green><b>Record Updated Successfully.<b></font>";
            public const string Delete_Message = "<font color=white><b>Record Deleted Successfully.<b></font>";
            public const string RecordInUse = "<font color=red><b>Record cannot be deleted! Its in use.<b></font>";
            public const string UserEmailNotExist = "<font color=red><b>User email address not exists!<b></font>";
            public const string CreatePurchaseRequest = "<font color=red><b>Required purchase request.<b></font>";
            public const string Approved = "<font color=green><b>Records Approved Successfully.<b></font>";
            public const string RequestedOrder = "<font color=green>Order #{0}  Requested Successfully.</font>";
            public const string SelectComponent = "<font color=red><b>Please select component before save changes.<b></font>";
            public const string RecordUnsaved = "<font color=red><b>Record not saved please try again.<b></font>";
            public const string RecordNotFound = "<font color=red><b>Record not found.<b></font>";
            public const string ErrorRecordSavingFile = "<font color=red><b>Error Record in Saving File<b></font>";
            public const string AcceptFriendRequest = "<font color=green><b>Accepted Your Friend Request.</b></font>";
            public const string UnblockMessage = "<font color=green><b>User unblock successfull.</b></font>";
            public const string UserblockMessage = "<font color=red><b>User block successfull.<b></font>";
            public const string UnFriend = "<font color=green><b>Unfriend Successfull</b></font>"; 
            public const string IsApproved = "<font color=white><b>Your account will be activated after approving by administrator.</b></font>";
            public const string IsUnApproved = "<font color=white><b>Your account will be deactivate by administrator.</b></font>";
            public const string CheckApproved ="<font color=red><b>Your account is not approved by Admin. </b></font>";
            public const string forgotPasswordSuccess = "<font color=Green><b>Your credential send to your email successfully. </b></font>";
            public const string RegisterApproval = "<font color=Green><b>Sign-up successfull, please keep wait for Admin approval. </b></font>";

        }
        public class Number
        {
            public const int Zero = 0;
            public const int One = 1;
            public const int Two = 2;
            public const int Three = 3;
            public const int Four = 4;
            public const int Five = 5;
            public const int Six = 6;
            public const int Seven = 7;
            public const int Eight = 8;
            public const int Nine = 9;
            public const int Ten = 10;
            public const int Eleven = 11;
            public const int Twelve = 12;
            public const int Thirteen = 13;
            public const int Fourteen = 14;
            public const int Fifteen = 15;
            public const int Sixteen = 16;
            public const int Seventeen = 17;
            public const int Eighteen = 18;

        }
        public class Seperator
        {
            public const string SeperatorSpace = " ";
            public const string SeperatorComma = ",";
            public const string LineBreak = "<br><br>";
            public const char Pipe = '|';
        }
        public class Image
        {
            public const string PDF = "../images/pdf_icon.png";
            public const string XLS = "../images/excel_icon.png";
            public const string DOC = "../images/word_icon.png";
            public const string ZIP = "../images/zip_icon.png";
            public const string File = "../images/file_icon.png";
            public const string RAR = "../images/rar_icon.png";
            public const string Normal = "../images/status_normal.png";
            public const string ProfileNoImage = "../profile_icon.png";
            public const string MyImage = "~";
        }
        public class Action
        {
            public const string Approve = "Approve";
            public const string Reject = "Reject";
            public const string Accept = "Accept";
            public const string ApproveAttachment = "ApproveAttachment";
            public const string RejectAttachment = "RejectAttachment";
            public const string CancelEco = "CancelEco";
            public const string Hold = "Hold";
        }

        public class Operation
        {
            public const string Update = "Update";
            public const string Save = "Save";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }


        public class Roles
        {

            public const string ReadOnly = "ReadOnly";
            public const string Admin = "Admin";
            public const string Trainer = "Trainer";

        }


        public class Column
        {

            public const string OrderID = "OrderID";
            public const string Date = "Date";
            public const string SalesPerson = "SalePerson";
            public const string TotalAmount = "TotalAmount";
            public const string PatientID = "PatientID";
            public const string PatientName = "PatientName";
            public const string Name = "Name";
            public const string Email = "Email";
            public const string LicenceNumber = "LicenceNo";
            public const string UntitledAlbum = "Untitled Album";
            public const string UntitledImage = "Untitled Image";
        }

        public class Status
        {
            public const Int16 Pending = 1;
        }

        public class Page
        {
            public const string Index = "Index";
        }

        /// <summary>
        /// Generate auto number to save album image to folder.
        /// </summary>
        /// <returns></returns>
        public static string NewNumber()
        {
            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            string MyNumber = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 12);
            return MyNumber;
        }
        public static string sendMail(string to, string subject, string body)
        {
            string RetValue = "";
            string fromEmailAddress = "mails@rezinfo.co.in";
            try
            {
                var mailMessage = new System.Net.Mail.MailMessage(fromEmailAddress, to);
                //mailMessage.Bcc.Add(new MailAddress(bcc));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                var smtpClient = new SmtpClient();
                smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                // smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {
                //RetValue =  ex.Message;
                RetValue = "<font color='red'>Error while sending Email.</font>";
            }
            return RetValue;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "ResulPortal";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "ResulPortal";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}