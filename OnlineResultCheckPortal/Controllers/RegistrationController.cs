using OnlineResultCheckPortal;
using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class RegistrationController : Controller
    {
        //
        // GET: /Registeration/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            return View();

        }
   
        /// <summary>
        /// This method use register user save data in table.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult RegistrationSave(Models.Registration registration)
        {
            string emailId = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            string password = string.Empty;
            string returnResult = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                   
                        // Checking Email ID exist in User Table.
                        var user = ObjOCRP.Users.FirstOrDefault(c => c.EmailID == registration.EmailID);

                        if (user != null)
                        {
                            returnResult=Models.Utility.Message.EmailExist;
                        }
                        else
                        {
                            // Saving User Detail in User Table.
                            var userDetail = new User();
                            userDetail.FirstName = registration.FirstName;
                            userDetail.LastName = registration.Lastname;
                            userDetail.EmailID = registration.EmailID;
                            userDetail.Password = registration.ConfirmPassword;
                            userDetail.RoleId = 2;
                            userDetail.IsDeleted = false;
                            userDetail.IsApproved = false;
                            userDetail.CreatedDate = DateTime.Now;
                            ObjOCRP.Users.Add(userDetail);
                            ObjOCRP.SaveChanges();

                        var name = userDetail.FirstName + " " + userDetail.LastName;
                        
                        emailId = userDetail.EmailID;
                        subject = "Sign-up Successfully";
                        body = "Hi "+name+ ",<br/><br/><font color=green><b>You have sign-up successfully.</font><br/></b><br/>Please keep patience for admin approval.<br/>" + "User Name: " + emailId + "<br/>" + "Your Password : " + userDetail.Password+ " <br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it
                        Models.Utility.sendMail(emailId, subject, body);
                        returnResult = Models.Utility.Message.RegisterApproval;
                        }

                }
                catch(Exception ex)
                {
                    returnResult = Models.Utility.Message.RecordUnsaved;
                }
                }

            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
            
        

    }
}
