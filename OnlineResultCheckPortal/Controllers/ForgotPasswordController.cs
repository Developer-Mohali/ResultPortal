using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class ForgotPasswordController : Controller
    {
        //
        // GET: /ForgotPassword/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForgotPassword(Models.Registration objRegisterForgotPassword) {

            string returnResult = string.Empty;
            string password = string.Empty;
            string emailId = string.Empty;
            string subject = string.Empty;
            string body = string.Empty; 
            try
            {
                var objForgotPassword = ObjOCRP.Users.FirstOrDefault(c => (c.EmailID == objRegisterForgotPassword.EmailID));
                if (objForgotPassword != null)
                {
                    password = objForgotPassword.Password;
                    var name = objForgotPassword.FirstName + " " + objForgotPassword.LastName;

                    emailId = objForgotPassword.EmailID;
                    subject = "Forget Password";
                    body = "Hi " + name + ",<br/><br/><font color=green><b>Your credential for login.</font></b><br/>" + "User Name: " + emailId + "<br/>" + "Your Password : " + password + " <br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it

                    Models.Utility.sendMail(emailId, subject, body);
                    returnResult= Models.Utility.Message.forgotPasswordSuccess;
                }
                else
                {
                    returnResult = Models.Utility.Message.UserEmailNotExist;
                }
            }
            catch(Exception ex)
            {
           
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
