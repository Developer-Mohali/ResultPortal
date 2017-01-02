using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class TokenController : Controller
    {
        //
        // GET: /Token/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();

        public ActionResult Index()
        {
            return View();
        }
       /// <summary>
       /// This method use to add new token and update.
       /// </summary>
       /// <param name="token"></param>
        
        [HttpPost]

        public ActionResult AddTokens(Models.Token objtoken)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objToken = ObjOCRP.Tokens.FirstOrDefault(c => (c.TokenName == objtoken.TokenName));
            if(objToken==null)
            {
            objToken=new Token();
           
            objToken.TokenName= objtoken.TokenName;
            objToken.TokenPrice= objtoken.TokenPrice;
            objToken.NumberOfTimeUse=Convert.ToInt16(objtoken.NumberOfTimeUse);
            objToken.TokenDescription= objtoken.TokenDescription;
            objToken.TokenPicture= objtoken.TokenPicture;
            objToken.CreatedBy= createdBy;
            objToken.CreatedDate=System.DateTime.Now;
            objToken.IsDeleted=false;
            ObjOCRP.Tokens.Add(objToken);
            ObjOCRP.SaveChanges();
            returnResult= Utility.Message.Add_Message;
            
            }
            else
            
            {
             
                objToken.TokenName= objtoken.TokenName;
                objToken.TokenPrice= objtoken.TokenPrice;
                objToken.NumberOfTimeUse=Convert.ToInt16(objtoken.NumberOfTimeUse);
                objToken.TokenDescription= objtoken.TokenDescription;
                objToken.TokenPicture= objtoken.TokenPicture;
                objToken.UpdatedBy=createdBy;
                objToken.UpdatedDate=System.DateTime.Now;
                ObjOCRP.SaveChanges();
                returnResult= Utility.Message.Update_Message;

            }
              return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This Method Is To Be The Display The List.
        /// </summary>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult TokenList() 
        {
            var objTokenList = ObjOCRP.GetTokenList().ToList();
            return new JsonResult { Data = objTokenList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        
        /// <summary>
        /// This Method Is Use To Editing The List By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        
      
        public ActionResult EditList(Int32 tokenId = Utility.Number.Zero)
        {
            var objEditTokenList = ObjOCRP.EditTokenLists(tokenId).ToList();
            return new JsonResult { Data = objEditTokenList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        ///This Method Is Delete The Token Records In Table. 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult DeleteTokenDetails(Int32 tokenId = Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objDeleteTokenDetails = ObjOCRP.Tokens.FirstOrDefault(c => (c.ID == tokenId));
            if (objDeleteTokenDetails != null) 
            {
                objDeleteTokenDetails.IsDeleted = true;
                objDeleteTokenDetails.DeletedBy = createdBy;
                ObjOCRP.SaveChanges();
                returnResult = Utility.Message.Delete_Message;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
