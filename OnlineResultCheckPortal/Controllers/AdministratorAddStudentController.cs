using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class AdministratorAddStudentController : Controller
    {
        //
        // GET: /AdministratoAddStudent/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();

        public ActionResult Index()
        {
            AcademicYear();
            GetSchoolAdministrator();
            return View();
        }
        /// <summary>
        /// This method use to Get school administrator By id.
        /// </summary>
        public void GetSchoolAdministrator()
        {
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            ViewBag.School = new SelectList(ObjOCRP.AdministratorSchool(createdBy).ToList(),"ID", "SchoolName");

        }
        /// <summary>
        /// This method use to get AcademicYear.
        /// </summary>
        public void AcademicYear()
        {

            ViewBag.AcademicYear = new SelectList(ObjOCRP.Academic_Sessions.ToList(), "ID", "AcademicYear");

        }
        /// <summary>
        /// This method use to get user profile  by created By ID for Admin.
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfile()
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

                try
                {
                    // get Start (paging start index) and length (page size for paging)
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    //Get Sort columns value
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int totalRecords = 0;

                    using (OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal())
                    {
                        var ObjUserProfile = ObjOCRP.GetStudentProfileByAdministrator(createdBy).ToList();
                        //Sorting
                        totalRecords = ObjUserProfile.Count();
                        var data = ObjUserProfile.Skip(skip).Take(pageSize).ToList();
                        return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception ex)
                {

                }
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult UpdateUserProfile(Models.Student objRegistration)
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
                var ObjUserNewProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == objRegistration.ID));
                if (ObjUserNewProfile != null)
                {
                    ObjUserNewProfile.FirstName = objRegistration.FirstName;
                    ObjUserNewProfile.LastName = objRegistration.Lastname;
                    ObjUserNewProfile.StudentID = objRegistration.StudentId;
                    ObjOCRP.SaveChanges();

                    //Updat data student table.
                    var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == objRegistration.ID));
                    if (ObjStudnentProfile != null)
                    {

                        ObjStudnentProfile.Dob = objRegistration.DOB;
                        ObjStudnentProfile.AcademicYear = objRegistration.AcademicYear;
                        ObjStudnentProfile.School = objRegistration.School;
                        ObjStudnentProfile.Address = objRegistration.Address;
                        ObjStudnentProfile.Gender = objRegistration.Gender;
                        ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                        ObjStudnentProfile.State = objRegistration.State;
                        ObjOCRP.SaveChanges();
                    }
                    returnResult = Models.Utility.Message.Update_Message;
                }
                else
                {
                    ObjUserNewProfile = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == objRegistration.StudentId));
                    if (ObjUserNewProfile == null)
                    {
                        // Saving User Detail in User Table.
                        ObjUserNewProfile = new User();
                        ObjUserNewProfile.FirstName = objRegistration.FirstName;
                        ObjUserNewProfile.LastName = objRegistration.Lastname;
                        ObjUserNewProfile.StudentID = objRegistration.StudentId;
                        ObjUserNewProfile.RoleId = 2;
                        ObjUserNewProfile.CreatedBy = createdBy;
                        ObjUserNewProfile.IsDeleted = false;
                        ObjUserNewProfile.IsApproved = true;
                        ObjUserNewProfile.CreatedDate = DateTime.Now;
                        ObjOCRP.Users.Add(ObjUserNewProfile);
                        ObjOCRP.SaveChanges();

                        //Saving data student table.
                        var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == ObjUserNewProfile.ID));
                        if (ObjStudnentProfile == null)
                        {
                            ObjStudnentProfile = new Student();
                            ObjStudnentProfile.Dob = objRegistration.DOB;
                            ObjStudnentProfile.AcademicYear = objRegistration.AcademicYear;
                            ObjStudnentProfile.School = objRegistration.School;
                            ObjStudnentProfile.Address = objRegistration.Address;
                            ObjStudnentProfile.Gender = objRegistration.Gender;
                            ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                            ObjStudnentProfile.State = objRegistration.State;
                            ObjStudnentProfile.UserId = ObjUserNewProfile.ID;
                            ObjOCRP.Students.Add(ObjStudnentProfile);
                            ObjOCRP.SaveChanges();
                        }
                        returnResult = Models.Utility.Message.Add_Message;

                    }
                    else
                    {
                        returnResult = Models.Utility.Message.RegistrationNumbers;

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
        /// This method use to update student profile by student id.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserProfile(Int32 userID = Models.Utility.Number.Zero)
        {
            if (userID != null)
            {
                var ObjEditUserProfile = ObjOCRP.EditUserProfile(userID).ToList();
                return new JsonResult { Data = ObjEditUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }

        /// <summary>
        /// This method use to delete by Admin id.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserProfile(Int32 userID = Models.Utility.Number.Zero)
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
    }
}
