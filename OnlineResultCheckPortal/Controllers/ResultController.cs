using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Result/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This method use to get result student by Registration no.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetResult(Models.JSCEResult objRegistrationNumber)
        {
            String returnResult = string.Empty;
            try
            {
               
                var ObjGetResult = ObjOCRP.GetOnlyPurchaseTokenResult(objRegistrationNumber.Registration,objRegistrationNumber.ExamTypes).ToList();
                return new JsonResult { Data = ObjGetResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch(Exception ex)
            {
                returnResult = "1";
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use result download by registration no.
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <returns></returns>
        public ActionResult ResultDownloadFile(Models.JSCEResult objRegistrationNumber)
        {
            
            var objJSCEResult = ObjOCRP.GetResutFileDownload(objRegistrationNumber.Registration, objRegistrationNumber.ExamTypes).ToList();
            //var filepath = System.IO.Path.Combine(Server.MapPath("/StudentPhoto/"), RegistrationId);
            //return File(filepath, MimeMapping.GetMimeMapping(filepath), RegistrationId);
            return new JsonResult { Data = objJSCEResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to download result.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult DownloadFile(string file)
        {

            var filepath = System.IO.Path.Combine(Server.MapPath("/StudentExecelSheet/"), file);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), file);

        }
        /// <summary>
        /// This method use to ReportCardDownload by registration.
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <returns></returns>
        public ActionResult ReportDownload(Int32 RegistrationId = Models.Utility.Number.Zero)
        {
            string reportCard = string.Empty;
            string regitrationNo = Convert.ToString(RegistrationId);
            var objJSCEResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == regitrationNo));
            if (objJSCEResult != null)
            {
                reportCard = objJSCEResult.ReportCardFile;
            }
            //var filepath = System.IO.Path.Combine(Server.MapPath("/StudentPhoto/"), RegistrationId);
            //return File(filepath, MimeMapping.GetMimeMapping(filepath), RegistrationId);
            return new JsonResult { Data = reportCard, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to download report card by registration.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult ReportDownloadFile(string file)
        {

            var filepath = System.IO.Path.Combine(Server.MapPath("/StudentExecelSheet/"), file);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), file);

        }
    }
}
