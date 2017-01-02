using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class ManageSchoolManagementController : Controller
    {
        //
        // GET: /ManageSchoolManagement/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
                GetSchoolName(createdBy);
            }
            return View();
        }
        /// <summary>
        /// This method use to get value and bind dropdown list  by management id.
        /// </summary>
        /// <param name="ManagementId"></param>
        public void GetSchoolName(Int32 userID = Models.Utility.Number.Zero)
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.GetSchoolNameDetails(userID).ToList(), "SchoolID", "SchoolName");
 
        }
        /// <summary>
        /// This method use to Get student details by manager id.
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentProfile()
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
                var ObjGetStudentProfile = ObjOCRP.GetStudentProfile().ToList();

                return new JsonResult { Data = ObjGetStudentProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
        /// <summary>
        /// This method use to save school name in table by manager Id.
        /// </summary>
        /// <param name="objStudent"></param>
        /// <returns></returns>
        public ActionResult SaveSchoolName(Models.Student objStudent)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            //This use to save School Id in SchoolName table.
            string strValue = Convert.ToString(objStudent.StudentId.Replace("multiselect-all,", "")).TrimEnd(',', ' ');

            string[] strArray = strValue.Split(',');

            foreach (var objSchoolName in strArray)
            {
                int StudentId = Convert.ToInt32(objSchoolName);


                var objStudentSave = ObjOCRP.Students.FirstOrDefault(c => (c.UserId ==StudentId));
                if (objStudentSave != null)
                {
                  
                    objStudentSave.School = objStudent.School;
                    objStudentSave.ProvideSchoolBy = createdBy;
                    objStudentSave.ProvideSchoolDate = DateTime.Now;
                    ObjOCRP.SaveChanges();
                    returnResult = Models.Utility.Message.Add_Message;
                }

                //This use to save School Id in SchoolName table.
                

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SchoolProfile()
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
                var ObjGetSchoolDetails = ObjOCRP.GetSchoolDetails(createdBy).ToList();

                return new JsonResult { Data = ObjGetSchoolDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }



        public ActionResult AddSchoolDisplayStudentProfile(Int32 SchoolID= Models.Utility.Number.Zero)
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
                var ObjGetSchoolDetails = ObjOCRP.GetDisplayStudentProfile(SchoolID).ToList();

                return new JsonResult { Data = ObjGetSchoolDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
    }
}
