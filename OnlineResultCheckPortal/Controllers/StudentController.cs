using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace OnlineResultCheckPortal.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
         
            RoleId();
            SchoolName();
            return View();
        }

        public ActionResult StudentProfile()
        {
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjAdminProfile = ObjOCRP.StudentProfile(createdBy).ToList();
            return new JsonResult { Data = ObjAdminProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to get role name.
        /// </summary>
        public void RoleId()
        {

            ViewBag.RoleName = new SelectList(ObjOCRP.Roles.ToList(), "ID", "RoleName");

        }
     
        /// <summary>
        /// This method use to get Scholl name.
        /// </summary>
        public void SchoolName()
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.Schools.ToList(), "ID", "SchoolName");

        }
        /// <summary>
        /// This method use to Update user profile by user id.
        /// </summary>
        /// <returns></returns>
        //public ActionResult UpdateStudentProfile(Models.Student objUserPdrofile)
        //{
        //    string returnResult = string.Empty;
        //    Int32 createdBy = Models.Utility.Number.Zero;
        //    //Getting user id by session.
        //    if (Session["UserId"] != null)
        //    {
        //        createdBy = Convert.ToInt32(Session["UserId"]);
        //    }
        //    var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == createdBy));
        //    if (ObjStudnentProfile == null)
        //    {
        //        var objUsers = ObjOCRP.Users.FirstOrDefault(c => (c.ID == createdBy));
        //        if (objUsers != null)
        //        {
        //            objUsers.FirstName = objUserPdrofile.FirstName;
        //            objUsers.LastName = objUserPdrofile.Lastname;
        //            ObjOCRP.SaveChanges();
        //        }
        //            ObjStudnentProfile = new Student();
        //        ObjStudnentProfile.FatherName = objUserPdrofile.FatherName;
        //        ObjStudnentProfile.Dob = objUserPdrofile.DOB;
        //        ObjStudnentProfile.Contact = objUserPdrofile.Contact;
        //        ObjStudnentProfile.AcademicYear = objUserPdrofile.AcademicYear;
        //        ObjStudnentProfile.School = objUserPdrofile.School;
        //        ObjStudnentProfile.Address = objUserPdrofile.Address;
        //        ObjStudnentProfile.Gender = objUserPdrofile.Gender;
        //        ObjStudnentProfile.UserId = createdBy;
        //        ObjOCRP.Students.Add(ObjStudnentProfile);
        //        ObjOCRP.SaveChanges();
        //        returnResult = Models.Utility.Message.Update_Message;

        //    }
        //    else
        //    {
        //        var objUsers = ObjOCRP.Users.FirstOrDefault(c=>(c.ID==createdBy));
        //        if(objUsers!=null)
        //        {
        //            objUsers.FirstName = objUserPdrofile.FirstName;
        //            objUsers.LastName = objUserPdrofile.Lastname;
        //            ObjOCRP.SaveChanges();

        //            ObjStudnentProfile.FatherName = objUserPdrofile.FatherName;
        //            ObjStudnentProfile.Dob = objUserPdrofile.DOB;
        //            ObjStudnentProfile.Contact = objUserPdrofile.Contact;
        //            ObjStudnentProfile.School = objUserPdrofile.School;
        //            ObjStudnentProfile.Address = objUserPdrofile.Address;
        //            ObjStudnentProfile.Gender = objUserPdrofile.Gender;
        //            ObjOCRP.SaveChanges();
        //            returnResult = Models.Utility.Message.Update_Message;
        //        }
              

        //    }
        //    return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        [HttpPost]
        public JsonResult SaveStudentProfileImage()
        {

            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            string title = string.Empty;
            string returnResult = string.Empty;
            try
            {
                // save file
                foreach (string fileName in Request.Files)
                {
                    //get Auto Generate number.
                    string imageNameNew = Models.Utility.NewNumber();
                    HttpPostedFileBase file = Request.Files[fileName];
                    title = imageNameNew + ' ' + file.FileName;
                    if (file != null && file.ContentLength > Models.Utility.Number.Zero)
                    {
                        // Getting folder path to save image
                        var Imagepath = Path.Combine(Server.MapPath("~/StudentPhoto/"));
                        string pathString = System.IO.Path.Combine(Imagepath.ToString());
                        var namepath = Path.GetFileName(file.FileName);
                        //Checking image already exist in folder
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{" + Models.Utility.Number.Zero + "}\\{" + Models.Utility.Number.One + "}", pathString, imageNameNew + ' ' + file.FileName);
                        file.SaveAs(uploadpath);
                        {
                            var objStudentProfile = ObjOCRP.Students.FirstOrDefault(c => c.UserId == createdBy);
                            if (objStudentProfile == null)
                            {
                                objStudentProfile = new Student();
                                objStudentProfile.Picture = title;
                                objStudentProfile.UserId = createdBy;
                                ObjOCRP.Students.Add(objStudentProfile);
                                ObjOCRP.SaveChanges();
                                returnResult = Models.Utility.Message.Add_Message;
                            }
                            else
                            {
                                objStudentProfile.Picture = title;
                                ObjOCRP.SaveChanges();

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
