

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This method use to admin and student login  by password and email id.
        /// </summary>
        /// <param name="objRegistration"></param>
        /// <returns></returns>
        public ActionResult login(Models.Registration objRegistration)
        {
            string returnResult = string.Empty;
            string dcyrptPassword = Models.Utility.Encrypt(objRegistration.Password);
            var objRegistrationLogin = ObjOCRP.Users.FirstOrDefault(c => (c.EmailID == objRegistration.EmailID && c.Password == dcyrptPassword && c.IsDeleted == false));
            if (objRegistrationLogin != null)
            {
                if (objRegistrationLogin.IsApproved == true)
                {
                    Session["UserId"] = objRegistrationLogin.ID;
                    Session["UserEmail"] = objRegistrationLogin.EmailID;
                    Session["RoleId"] = objRegistrationLogin.RoleId;
                    if (objRegistrationLogin.RoleId == 1)
                    {
                        returnResult = "Management";

                    }
                    else if (objRegistrationLogin.RoleId == 3)
                    {
                        returnResult = "Admin";
                        }
                    else
                    {
                        returnResult = "Student";

                    }
                }
                else {
                    returnResult = Models.Utility.Message.CheckApproved;
                }



            }
            else
            {
                returnResult = Models.Utility.Message.AuthenticationWrong;
               
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
