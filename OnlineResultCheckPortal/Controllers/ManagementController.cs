using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class ManagementController : Controller
    {
        //
        // GET: /Management/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
                    SchoolName();
                    return View();
        }
        /// <summary>
        /// This method use to Admin Management profile.
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminMangement()
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjAdmin = ObjOCRP.Users.Where(c => (c.ID == createdBy));
            if (ObjAdmin != null)
            {
                var ObjUserProfile = ObjOCRP.AdminMangement().ToList();

                return new JsonResult { Data = ObjUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
        /// <summary>
        /// This method use to get Scholl name.
        /// </summary>
        public void SchoolName()
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.Schools.ToList(), "ID", "SchoolName");

        }
        /// <summary>
        /// This method use to Bindropdownlist by management id.
        /// </summary>
        /// <param name="ManagementId"></param>
        public ActionResult GetSchoolName(Int32 userID = Models.Utility.Number.Zero)
        {

          var SelectedSchoolName =ObjOCRP.GetSchoolNameDetails(userID).ToList();
          return new JsonResult { Data = SelectedSchoolName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to Delete Admin Management profile by User Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult DeleteAdminMangement(Int32 userID = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objUsersProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == userID));

            if (objUsersProfile != null)
            {
                objUsersProfile.IsDeleted = true;
                objUsersProfile.IsDeletedBy = createdBy;
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Delete_Message;

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        /// <summary>
        /// This method use to Get Edit Management profile by User Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult GetEditManage(Int32 userID = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjAdmin = ObjOCRP.Users.Where(c => (c.ID == createdBy));
            if (ObjAdmin != null)
            {
                var ObjUserProfile = ObjOCRP.GetEditMangement(userID).ToList();

                return new JsonResult { Data = ObjUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
        /// <summary>
        /// This method use to Update ManageManagement profile by User Id.
        /// </summary>
        /// <param name="objAdminManage"></param>
        /// <returns></returns>
        public ActionResult UpdateAmindManagement(Models.Student objAdminManage)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            try
            {
                var ObjUserUpdateProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == objAdminManage.ID && c.EmailID == objAdminManage.Email));
                if (ObjUserUpdateProfile != null)
                {
                    //Update User table.
                    ObjUserUpdateProfile.FirstName = objAdminManage.FirstName;
                    ObjUserUpdateProfile.LastName = objAdminManage.Lastname;
                    ObjUserUpdateProfile.RoleId = objAdminManage.RoleId;
                    ObjUserUpdateProfile.Password = objAdminManage.ConfirmPassword;
                    ObjOCRP.SaveChanges();
                 
                        //This use to save School Id in SchoolName table.
                    var objSchool = ObjOCRP.SchoolNames.FirstOrDefault(c => (c.ManagementID == objAdminManage.ID));
                    if (objSchool != null)
                    {
                        //This use to save School Id in SchoolName table.
                       
                        objSchool.SchoolID = objAdminManage.School;
                        objSchool.ManagementID = objAdminManage.ID;
                        objSchool.UpdateBy = createdBy;
                        objSchool.UpdateDate = DateTime.Now; 
                        ObjOCRP.SaveChanges();
                    }
                    else
                    {
                        objSchool = new SchoolName();
                        objSchool.SchoolID = objAdminManage.School;
                        objSchool.ManagementID = objAdminManage.ID;
                        objSchool.CreatedBy = createdBy;
                        objSchool.CreatedDate = DateTime.Now;
                        ObjOCRP.SchoolNames.Add(objSchool);
                        ObjOCRP.SaveChanges();
                    }
                    var objAdminManagement = ObjOCRP.ManageManagements.FirstOrDefault(c => (c.UserId == objAdminManage.ID));
                    if (objAdminManagement != null)
                    {
                        //Update ManageManagement table.
                        objAdminManagement.Address = objAdminManage.Address;
                        objAdminManagement.ContactNo = objAdminManage.Contact;
                        objAdminManagement.CreatedBy = createdBy;
                        objAdminManagement.CreatedDate = DateTime.Now;
                        objAdminManagement.DateofBirth = objAdminManage.DOB;
                        objAdminManagement.FatherName = objAdminManage.FatherName;
                        objAdminManagement.Gender = objAdminManage.Gender;
                        ObjOCRP.SaveChanges();
                    }
                    else
                    {
                        //Insert ManageManagement table.
                        objAdminManagement = new ManageManagement();
                        objAdminManagement.Address = objAdminManage.Address;
                        objAdminManagement.ContactNo = objAdminManage.Contact;
                        objAdminManagement.CreatedBy = createdBy;
                        objAdminManagement.CreatedDate = DateTime.Now;
                        objAdminManagement.DateofBirth = objAdminManage.DOB;
                        objAdminManagement.FatherName = objAdminManage.FatherName;
                        objAdminManagement.Gender = objAdminManage.Gender;
                        objAdminManagement.UserId = objAdminManage.ID;
                        ObjOCRP.ManageManagements.Add(objAdminManagement);
                        ObjOCRP.SaveChanges();
                    }
                  

                    returnResult = Models.Utility.Message.Update_Message;
                }
                else
                {

                    var ObjUserNewProfile = ObjOCRP.Users.FirstOrDefault(c => (c.EmailID == objAdminManage.Email));
                    if (ObjUserNewProfile == null)
                    {
                        // Saving User Detail in User Table.
                        ObjUserNewProfile = new User();
                        ObjUserNewProfile.FirstName = objAdminManage.FirstName;
                        ObjUserNewProfile.LastName = objAdminManage.Lastname;
                        ObjUserNewProfile.EmailID = objAdminManage.Email;
                        ObjUserNewProfile.Password = objAdminManage.ConfirmPassword;
                        ObjUserNewProfile.RoleId = objAdminManage.RoleId;
                        ObjUserNewProfile.IsDeleted = false;
                        ObjUserNewProfile.IsApproved = false;
                        ObjUserNewProfile.CreatedDate = DateTime.Now;
                        ObjOCRP.Users.Add(ObjUserNewProfile);
                        ObjOCRP.SaveChanges();

                        //Saving data in manageManagement table.
                        var objManagement = new ManageManagement();
                        objManagement.Address = objAdminManage.Address;
                        objManagement.ContactNo = objAdminManage.Contact;
                        objManagement.CreatedBy = createdBy;
                        objManagement.CreatedDate = DateTime.Now;
                        objManagement.DateofBirth = objAdminManage.DOB;
                        objManagement.FatherName = objAdminManage.FatherName;
                        objManagement.Gender = objAdminManage.Gender;
                        objManagement.UserId = ObjUserNewProfile.ID;
                        ObjOCRP.ManageManagements.Add(objManagement);
                        ObjOCRP.SaveChanges();

                        ////This use to save School Id in SchoolName table.

                        var objSchool = ObjOCRP.SchoolNames.FirstOrDefault(c => (c.ManagementID == ObjUserNewProfile.ID ));
                        if (objSchool == null)
                        {
                            //This use to save School Id in SchoolName table.
                            objSchool = new SchoolName();
                            objSchool.SchoolID = objAdminManage.School;
                            objSchool.ManagementID = ObjUserNewProfile.ID;
                            objSchool.CreatedBy = createdBy;
                            objSchool.CreatedDate = DateTime.Now;
                            ObjOCRP.SchoolNames.Add(objSchool);
                            ObjOCRP.SaveChanges();
                        }
                        else
                        {
                            //objSchool = new SchoolName();
                            objSchool.SchoolID = objAdminManage.School;
                            objSchool.ManagementID = objAdminManage.ID;
                            objSchool.UpdateBy = createdBy;
                            objSchool.UpdateDate = DateTime.Now;
                            ObjOCRP.SaveChanges();
                        }

                        returnResult = Models.Utility.Message.Add_Message;
                    }
                    else
                    {
                        returnResult = Models.Utility.Message.EmailExist;

                    }
                }
               
            }
            catch (Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to ApprovedAdmin by user Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult ApprovedAdmin(Int32 userID = Models.Utility.Number.Zero)
        {
            string emailId = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjUserProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == userID));
            if (ObjUserProfile != null)
            {
                ObjUserProfile.IsApproved = true;
                ObjUserProfile.IsApprovedBy = createdBy;
                ObjOCRP.SaveChanges();

                //Get firstname and lastname from user.
                var name = ObjUserProfile.FirstName + " " + ObjUserProfile.LastName;
                emailId = ObjUserProfile.EmailID;
                subject = "Result Masta : Account approval";
                body = "Hi " + name + ",<br/><br/><font color=green><b>You account has been approved by Administrator.</font><br/></b><br/>Please click here to <a href='http://resultmasta.com.yew.arvixe.com/Login/Index' >signIn</a><br/><br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it
                Models.Utility.sendMail(emailId, subject, body);

                returnResult = Models.Utility.Message.IsApproved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to UnApprovedAdmin by User Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult UnApprovedAdmin(Int32 userID = Models.Utility.Number.Zero)
        {
            string emailId = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjUserProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == userID));
            if (ObjUserProfile != null)
            {
                ObjUserProfile.IsApproved = false;
                ObjUserProfile.IsApprovedBy = createdBy;
                ObjOCRP.SaveChanges();

                //Get firstname and lastname from user.
                var name = ObjUserProfile.FirstName + " " + ObjUserProfile.LastName;
                emailId = ObjUserProfile.EmailID;

                subject = "Result Masta : Account Unapproval";
                body = "Hi " + name + ",<br/><br/><font color=green><b>You account has been Unapproved by Administrator.</font><br/></b><br/>Please contact admin for more information.<br/><br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it
                Models.Utility.sendMail(emailId, subject, body);


                returnResult = Models.Utility.Message.IsUnApproved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

     
    }
}
