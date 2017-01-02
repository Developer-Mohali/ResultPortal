using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using OnlineResultCheckPortal.Models;
namespace OnlineResultCheckPortal.Controllers
{
    public class ManageJSCEController : Controller
    {
        //
        // GET: /ManageJSCE/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index(Int32 StudentId = Utility.Number.Zero)
        {
            ViewBag.StudentId = StudentId;
            LoadManageJSCE();
            return View();
        }

        /// <summary>
        /// <Description>This method is used to bind ManageJSCE List to DropDown by ViewBag</Description>
        /// </summary>
        public void LoadManageJSCE()
        {

            ViewBag.FirstName = new SelectList(ObjOCRP.GetStudentName().ToList(), "ID", "Name");

        }
        /// <summary>
        /// This method use to save/update Manage JSCE list information.
        /// </summary>
        /// <param name="objUserList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUpdateManageJSCEListDetails(Models.JSCE objJSCE)
        {
            bool JSCEExistance = false;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            try
            {
                var objJSCElist = ObjOCRP.JSCEs.FirstOrDefault(c => c.StundentID == objJSCE.StudentID && c.JSCERegNumber==objJSCE.JSCERegNumber);

                if (objJSCElist == null)
                {
                    JSCEExistance = JSJCEExistance(objJSCE.JSCERegNumber);
                    if (JSCEExistance == true)
                    {
                        returnResult ="Already Exist";
                    }
                    else
                    {
                        //save

                        objJSCElist = new JSCE();
                        objJSCElist.StundentID = objJSCE.StudentID;
                        objJSCElist.JSCERegNumber = objJSCE.JSCERegNumber;
                        objJSCElist.CreatedDate = System.DateTime.Now;
                        objJSCElist.CreatedBy = createdBy;
                        objJSCElist.IsDeleted = false;
                        ObjOCRP.JSCEs.Add(objJSCElist);
                        ObjOCRP.SaveChanges();
                        returnResult = Utility.Message.Add_Message;

                    }
                }
                else
                {
                    //Update
                   
                    objJSCElist.JSCERegNumber = objJSCE.JSCERegNumber;
                    //objJSCElist.IsDeleted = true;
                    objJSCElist.CreatedBy = createdBy;
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;
                }

            }
            catch (Exception ex)
            {
                returnResult = Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This method use to get DisplayManageJSCE profile by userId. 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult DisplayManageJSCE()
        {
            Int32 createdBy = Utility.Number.Zero;
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objJSCEDetails = ObjOCRP.GetDisplayManageListJSCE(createdBy).ToList();
            return new JsonResult { Data = objJSCEDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        //<summary>
        //This method use to DeleteManageJSCE profile by User Id.
        //</summary>
        //<param name="userID"></param>
        //<returns></returns>
        public ActionResult DeleteManageJSCE(Int32 userID = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjUserProfileJSCE = ObjOCRP.JSCEs.FirstOrDefault(c => (c.ID == userID));

            if (ObjUserProfileJSCE != null)
            {
                ObjUserProfileJSCE.IsDeleted = true;
                // ObjUserProfileJSCE.IsDeletedBy = createdBy;
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Delete_Message;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditManageJSCE(Int32 userID = Models.Utility.Number.Zero)
        {
            if (userID != null)
            {
                var ObjEditManageJSCEProfile = ObjOCRP.GetManageJSCEEdit(userID).ToList();
                return new JsonResult { Data = ObjEditManageJSCEProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
        //Checking JSCEUID in JSCE Table. 
        private bool JSJCEExistance(string JSCERegNumber)
        {
            bool returnResult = false;
            var objJSCE = ObjOCRP.JSCEs.FirstOrDefault(c => c.JSCERegNumber == JSCERegNumber);
            if (objJSCE != null)
            {
                returnResult = true;
            }
            else
            {
                returnResult = false;
            }

            return returnResult;
        }
    }
}