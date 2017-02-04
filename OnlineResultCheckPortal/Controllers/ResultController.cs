using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
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
            SchoolName();
            return View();
        }
        /// <summary>
        /// This method use to get result student by Registration no.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetResult(Models.JSCEResult objRegistrationNumber)
        {

            int messages = 0;
           string returnResult = string.Empty;
            try
            {
                if(ValidTokenID(objRegistrationNumber.TokenNumber)==true)
                {
                   if(GetValidTokenId(objRegistrationNumber.TokenNumber) ==true)
                    {
                        var getUseofTokenNumber = ObjOCRP.Tokens.FirstOrDefault(c => (c.TokenID == objRegistrationNumber.TokenNumber));
                        var objStudentDownload = ObjOCRP.StudentDownloadResults.FirstOrDefault(c => (c.TokenID == objRegistrationNumber.TokenNumber));
                        if(objStudentDownload!=null)
                        {
                           var numberofuseToken =Convert.ToInt16(objStudentDownload.UsedTokenNumber);
                           string registrationNumber= objStudentDownload.RegitrationNumber;
                            if (registrationNumber == objRegistrationNumber.Registration)
                            {
                                if (getUseofTokenNumber.NumberOfTimeUse != numberofuseToken)
                                {
                                    var ObjGetResult = ObjOCRP.GetOnlyPurchaseTokenResult(objRegistrationNumber.TokenNumber, objRegistrationNumber.ExamTypes, objRegistrationNumber.Registration, objRegistrationNumber.SchoolID).ToList();
                                    if (ObjGetResult != null)
                                    {
                                        int sum = numberofuseToken + 1;
                                        objStudentDownload.UsedTokenNumber = sum;
                                        ObjOCRP.SaveChanges();
                                    }
                                    return new JsonResult { Data = ObjGetResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                }
                                else
                                {
                                    returnResult = "5";
                                }

                            }
                            else
                            {
                                returnResult = "1"; //Models.Utility.Message.TokenIdProvided;
                            }
                        }
                    }
                   else
                    {
                        var objStudentDownload = ObjOCRP.StudentDownloadResults.FirstOrDefault(c=>(c.TokenID==objRegistrationNumber.TokenNumber && c.RegitrationNumber==objRegistrationNumber.Registration));
                        if (objStudentDownload == null)
                        {
                            var objGetStudentProfile = ObjOCRP.DownloadResultGetProfile(objRegistrationNumber.Registration,objRegistrationNumber.ExamTypes).ToList();
                            if (objGetStudentProfile!=null)
                            {
                            foreach (var data in objGetStudentProfile)
                             {
                                var getUseofTokenNumber = ObjOCRP.Tokens.FirstOrDefault(c=>(c.TokenID==objRegistrationNumber.TokenNumber));
                                int studentID =Convert.ToInt32(data.StudentID);
                                string registration = data.RegistrationNumber;
                                int schoolID = data.ID;
                                    if (PurchaseVarification(studentID) == true)
                                    {
                                        var ObjGetResult = ObjOCRP.GetOnlyPurchaseTokenResult(objRegistrationNumber.TokenNumber, objRegistrationNumber.ExamTypes, objRegistrationNumber.Registration, objRegistrationNumber.SchoolID).ToList();
                                        if (ObjGetResult!=null)
                                        {


                                            objStudentDownload = new StudentDownloadResult();
                                            objStudentDownload.RegitrationNumber = objRegistrationNumber.Registration;
                                            objStudentDownload.TokenID = objRegistrationNumber.TokenNumber;
                                            objStudentDownload.SchoolID = Convert.ToString(schoolID);
                                            objStudentDownload.StudentID = studentID;
                                            objStudentDownload.ExamTypes = objRegistrationNumber.ExamTypes;
                                            objStudentDownload.UsedTokenNumber = 1;
                                            objStudentDownload.ResultDownloadDate = DateTime.Now;
                                            ObjOCRP.StudentDownloadResults.Add(objStudentDownload);
                                            ObjOCRP.SaveChanges();

                                            return new JsonResult { Data = ObjGetResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                        }
                                        //else
                                        //{
                                        //    returnResult = "6";
                                        //}

                                    }
                                    else
                                    {
                                        returnResult = "2"; ///Models.Utility.Message.TokenPurchase;
                                    }
                            }
                            }
                            else
                            {
                                returnResult = "4";
                            }

                        }
                    }
                    
                }
                else
                {
                    returnResult = "3";             // Models.Utility.Message.TokenNotValid;
                }
               
               
            }
            catch (Exception ex)
            {
                returnResult = "4";
            }
          
                return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
       
        }
        /// <summary>
        /// This method use to check token id from token table.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        private bool ValidTokenID(string tokenId)
        {
            bool flag = false;
            try
            {
                var objToken = ObjOCRP.Tokens.FirstOrDefault(c=>(c.TokenID==tokenId));
                if(objToken!=null)
                {
                    flag = true;
                }
            }
            catch(Exception EX)
            {
            
            }
            return flag;
        }
        /// <summary>
        /// This method use to checkTokenID from Student Table.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        private bool GetValidTokenId(string tokenId)
        {
            bool flag = false;
            try
            {
                var objToken = ObjOCRP.StudentDownloadResults.FirstOrDefault(c => (c.TokenID == tokenId));
                if (objToken != null)
                {
                    string registrationNumber = objToken.RegitrationNumber;
                    flag = true;
                }
            }
            catch (Exception EX)
            {

            }
            return flag;
        }
        /// <summary>
        /// This method use to checkTokenID from Student Table.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        private bool PurchaseVarification(int studentID)
        {
            bool flag = false;
            try
            {
                var objToken = ObjOCRP.Users.FirstOrDefault(c => (c.ID == studentID));
                if (objToken.PurchaseToken==true)
                {
                    flag = true;
                }
            }
            catch (Exception EX)
            {

            }
            return flag;
        }

        public void SchoolName()
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.GetSchool().ToList(), "ID", "SchoolName");

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
        public ActionResult DownloadFile(string file, string checkfolder)
        {
            string returnResult = string.Empty;
            string filepath = string.Empty;
            try
            {
                if (checkfolder == "1")
                {
                    filepath = System.IO.Path.Combine(Server.MapPath("/JsceResult/"), file);
                }
                else if (checkfolder == "2")
                {
                    filepath = System.IO.Path.Combine(Server.MapPath("/MockExamination/"), file);

                }
                else if (checkfolder == "3")
                {
                    filepath = System.IO.Path.Combine(Server.MapPath("/EndofTermExam/"), file);

                }
                return File(filepath, MimeMapping.GetMimeMapping(filepath), file);
            }
            catch (Exception ex)
            {
                returnResult = "Server Error";
                return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        ///// <summary>
        ///// This method use to ReportCardDownload by registration.
        ///// </summary>
        ///// <param name="RegistrationId"></param>
        ///// <returns></returns>
        //public ActionResult ReportDownload(Int32 RegistrationId = Models.Utility.Number.Zero)
        //{
        //    string reportCard = string.Empty;
        //    string regitrationNo = Convert.ToString(RegistrationId);
        //    var objJSCEResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == regitrationNo));
        //    if (objJSCEResult != null)
        //    {
        //        reportCard = objJSCEResult.ReportCardFile;
        //    }
        //    //var filepath = System.IO.Path.Combine(Server.MapPath("/StudentPhoto/"), RegistrationId);
        //    //return File(filepath, MimeMapping.GetMimeMapping(filepath), RegistrationId);
        //    return new JsonResult { Data = reportCard, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        ///// <summary>
        ///// This method use to download report card by registration.
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //public ActionResult ReportDownloadFile(string file)
        //{

        //    var filepath = System.IO.Path.Combine(Server.MapPath("/StudentExecelSheet/"), file);
        //    return File(filepath, MimeMapping.GetMimeMapping(filepath), file);

        //}
    }
}
