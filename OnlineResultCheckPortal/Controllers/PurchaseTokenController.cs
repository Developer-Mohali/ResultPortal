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
                var ObjUserProfile = ObjOCRP.PurchaseToken().ToList();

                return new JsonResult { Data = ObjUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
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
