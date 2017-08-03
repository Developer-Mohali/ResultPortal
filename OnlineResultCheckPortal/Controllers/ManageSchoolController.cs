using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class ManageSchoolController : Controller
    {
        //
        // GET: /ManageSchool/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
       
        public ActionResult Index()
        {
            
            return View();
        }
      
   
        /// <summary>
        /// This method Add School.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>

        [HttpPost]

        public ActionResult AddSchool(Models.School objManageSchool) 
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
                var objSchools = ObjOCRP.Schools.FirstOrDefault(c => (c.SchoolName == objManageSchool.SchoolName));
                if (objSchools == null)
                {
                    objSchools = new School();

                    objSchools.SchoolName = objManageSchool.SchoolName;
                    objSchools.Address = objManageSchool.Address;
                    objSchools.Zipcode = objManageSchool.Zipcode;
                    objSchools.Zone = objManageSchool.Zone;
                    objSchools.CreatedBy = createdBy;
                    objSchools.CreatedDate = System.DateTime.Now;
                    objSchools.IsDeleted = false;
                    ObjOCRP.Schools.Add(objSchools);
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Add_Message;
                }
                else
                {

                    objSchools.SchoolName = objManageSchool.SchoolName;
                    objSchools.Address = objManageSchool.Address;
                    objSchools.Zipcode = objManageSchool.Zipcode;
                    objSchools.Zone = objManageSchool.Zone;
                    objSchools.UpdatedBy = createdBy;
                    objSchools.UpdatedDate = System.DateTime.Now;
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;
                }
            }
            catch(Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This Method Is To Be The Dsplay List To The Schools.
        /// </summary>
        /// <returns></returns>

        [HttpPost]

        public ActionResult GetSchoolList()
        {
            string returnResult = string.Empty;
            try
            {
                // get Start (paging start index) and length (page size for paging)
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                string search = Request.Form.GetValues("search[value]")[0];
                if (search != string.Empty)
                {
                    try
                    {
                        //Get Sort columns value
                        var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                        var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                        int pageSize = length != null ? Convert.ToInt32(length) : 0;
                        int skip = start != null ? Convert.ToInt32(start) : 0;
                        int totalRecords = 0;

                        using (OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal())
                        {
                            var objGetSchoolList = ObjOCRP.SearchSchool(search).ToList();

                            //Sorting
                            totalRecords = objGetSchoolList.Count();
                            var data = objGetSchoolList.Skip(skip).Take(pageSize).ToList();
                            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    catch (Exception Ex)
                    {

                    }

                }
                else
                {
                    //Get Sort columns value
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int totalRecords = 0;

                    using (OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal())
                    {
                        var objGetSchoolList = ObjOCRP.GetDetailsSchool().ToList();
                        totalRecords = objGetSchoolList.Count();
                        var data = objGetSchoolList.Skip(skip).Take(pageSize).ToList();
                        return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                    }

                    //var objGetSchoolList = ObjOCRP.GetDetailsSchool().ToList();
                    //return new JsonResult { Data = objGetSchoolList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                returnResult = "";
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       
        /// <summary>
        ///This Method Is To The Edit The Detail Records By ID.  
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       
     

        public ActionResult EditSchoolDetails(Int32 schoolId = Utility.Number.Zero) 
        {
            var objSchoolEdit = ObjOCRP.EditSchoolList(schoolId).ToList();

            return new JsonResult { Data = objSchoolEdit, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        } 

        /// <summary>
        /// This Method is use to delete record by ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleteSchoolRecords(Int32 schoolId = Utility.Number.Zero) 
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objSchools = ObjOCRP.Schools.FirstOrDefault(c => (c.ID == schoolId));
            if(objSchools!=null)
            {
             objSchools.IsDeleted = true;
                objSchools.DeletedBy = createdBy;
             ObjOCRP.SaveChanges();
              returnResult = Utility.Message.Delete_Message;
            }
       return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
      }   
   }
}

