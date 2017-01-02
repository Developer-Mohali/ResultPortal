using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineResultCheckPortal.Models;

namespace OnlineResultCheckPortal.Controllers
{
    public class MyProfileController : Controller
    {
        //
        // GET: /MyProfile/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            Int64 UserId = Utility.Number.Zero;
            //if (Session["UserId"] != null)
            //{
            //    UserId = Convert.ToInt64(Session["UserId"]);
            //        var objStudent = ObjOCRP.Students.FirstOrDefault(c => c.UserId == UserId);

            //        return View(objStudent);
            //    }
            var objStudent = ObjOCRP.Students.FirstOrDefault(c => c.UserId == 1);
            ViewBag.SchoolID = objStudent.ID;
            ViewBag.SchoolName = new SelectList(ObjOCRP.Schools.ToList(), "ID", "SchoolName", objStudent.ID);

            //ViewBag.BillingAddressState = new SelectList(ObjOCRP.States.ToList(), "ID", "StateName", objAddress.StateID);

            // return View(objAddress);

            return View(objStudent);

            //  LoadMyProfile();
            // LoadStudentProfile();
            // return View();
        }

        /// <summary>
        /// <Description>This method is used to bind School List to DropDown by ViewBag</Description>
        /// </summary>
        public void LoadStudentProfile()
        {

            ViewBag.School = new SelectList(ObjOCRP.Students.ToList(), "ID", "School");

        }
        /// <summary>
        /// <Description>This method is used to bind AcademicYear List to DropDown by ViewBag</Description>
        /// </summary>
        public void LoadMyProfile()
        {

            ViewBag.AcademicYear = new SelectList(ObjOCRP.Academic_Sessions.ToList(), "ID", "AcademicYear");

        }
       
        //bind role name.
        private void BindUserRoleDropDownList()
        {
            ViewBag.RoleName = new SelectList(ObjOCRP.Users.ToList(), "FirstName", "LastName");
        }
        /// <summary>
        /// <Description>This method is used the save addres </Description>
        /// <CreatedDate>27-Jan-16</CreatedDate>
        /// </summary>
        /// <param name="objDisplay"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveStudentProfileDisplay(Student objDisplay)
        {
            Int64 userId = Convert.ToInt64(Session["ID"]);
            string returnResult = string.Empty;

            if (Session["userId"] != null)
            {
                userId = Convert.ToInt64(Session["userId"]);
            }
            try
            {
                var objStudent = ObjOCRP.Students.FirstOrDefault(c => c.ID == userId);
                if (objStudent == null)
                {
                    var objAccountStudent = new Student();
                    objAccountStudent.FirstName = objDisplay.FirstName;
                    objAccountStudent.LastName = objDisplay.LastName;
                    objAccountStudent.FatherName = objDisplay.FatherName;
                    objAccountStudent.Dob = objDisplay.Dob;
                    objAccountStudent.Address = objDisplay.Address;
                    objAccountStudent.Contact = objDisplay.Contact;
                    objAccountStudent.Gender = objDisplay.Gender;
                    objAccountStudent.Email = objDisplay.Email;
                    objAccountStudent.AcademicYear = objDisplay.AcademicYear;
                    objAccountStudent.School = objDisplay.School;
                    objAccountStudent.CreatedBy = objDisplay.UserId;
                    objAccountStudent.CreatedDate = System.DateTime.Now;
                    objAccountStudent.IsApproved = true;
                    ObjOCRP.Students.Add(objAccountStudent);
                    ObjOCRP.SaveChanges();
                    Session["userId"] = objAccountStudent.ID;
                    returnResult = Utility.Message.Add_Message;

                }
                else
                {
                    objStudent.FirstName = objDisplay.FirstName;
                    objStudent.LastName = objDisplay.LastName;
                    objStudent.FatherName = objDisplay.FatherName;
                    objStudent.Email = objDisplay.Email;
                    objStudent.Address = objDisplay.Address;
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;
                }
            }
            catch (Exception ex)
            {

                returnResult = Utility.Message.RecordUnsaved;
            }

            return Json(new { result = returnResult });
        }

    }
}

