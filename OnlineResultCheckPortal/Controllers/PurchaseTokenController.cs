using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class PurchaseTokenController : Controller
    {
        //
        // GET: /PurchaseToken/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This method use to get user profile  by created By ID for Admin.
        /// </summary>
        /// <returns></returns>
        public ActionResult PurchaseTokenDetails()
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
                var ObjUserProfile = ObjOCRP.PurchaseToken().ToList();
                //Sorting
             
                totalRecords = ObjUserProfile.Count();
                var data = ObjUserProfile.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }

         
        }

        /// <summary>
        /// This method use to approved Student 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult StudentPurchaseToken(Int32 userID = Models.Utility.Number.Zero)
        {
            string emailId = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            string password = string.Empty;
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
                ObjUserProfile.PurchaseToken = true;
                ObjUserProfile.PurchaseTokenBy = createdBy;
                ObjUserProfile.PurchaseTokenDate = DateTime.Now;
                ObjOCRP.SaveChanges();

                returnResult = Models.Utility.Message.PurchaseToken;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This method use to purchase token by management Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult StudentUnPurchaseToken(Int32 userID = Models.Utility.Number.Zero)
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
                ObjUserProfile.PurchaseToken = false;
               
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.UnPurchaseToken;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
