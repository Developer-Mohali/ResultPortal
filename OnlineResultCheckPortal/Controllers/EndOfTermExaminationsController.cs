using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class EndOfTermExaminationsController : Controller
    {
        //
        // GET: /EndOfTermExaminations/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();

        public ActionResult Index()
        {
            GetStudentNameManageEndofTermMockExam();
            return View();
        }

        //  /// <summary>
        //  /// This method use to bind student name.
        //  /// </summary>
        public void GetStudentNameManageEndofTermMockExam()
        {

            ViewBag.FirstName = new SelectList(ObjOCRP.GetStudentName().ToList(), "ID", "Name");

        }
        /// <summary>
        /// This method use to get details mock examination.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDetailsEndMockExaminations()
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
                var ObjUserProfile = ObjOCRP.GetDetailsEndMockExamanation().ToList();

                return new JsonResult { Data = ObjUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }

        /// <summary>
        /// This method use to get value and mock examanation table by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GetEditEndMockExamanation(Int32 Id = Models.Utility.Number.Zero)
        {
            var objEndMockExamanation = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.ID == Id));
            if (objEndMockExamanation != null)
            {
                var objEndMockExam = ObjOCRP.GetEndMockExamanationEdit(Id).ToList();
                return new JsonResult { Data = objEndMockExam, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteEndMockExamanation(Int32 Id = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objEndMockExamanation = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.ID == Id));
            try
            {
                if (objEndMockExamanation != null)
                {
                    objEndMockExamanation.IsDeleted = true;
                    objEndMockExamanation.IsDeletedByDate = DateTime.Now;
                    objEndMockExamanation.IsDeletedBy = createdBy;
                    ObjOCRP.SaveChanges();
                    returnResult = Models.Utility.Message.Delete_Message;

                }
            }
            catch (Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This method use to upload student result MockExamantion.
        /// </summary>
        /// <param name="objJsceDetails"></param>
        /// <returns></returns>
        public ActionResult ResultUploadEndOfTermMockExam(List<Models.EndMockExamanation> objEndofTermMockExamDetails)
        {
            int endmockId = 0;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            foreach (var data in objEndofTermMockExamDetails)
            {

                var objEndMockExamDetailResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber && c.StudentID == data.ID));
                //  Idjsce = objJsceDetailResult.ID;
                if (objEndMockExamDetailResult == null)
                {
                    objEndMockExamDetailResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber));
                    if (objEndMockExamDetailResult == null)
                    {
                        objEndMockExamDetailResult = new EndOfTermExamination();
                        objEndMockExamDetailResult.StudentID = data.ID;
                        objEndMockExamDetailResult.IsDeleted = false;
                        objEndMockExamDetailResult.RegistrationNumber = data.RegistrationNumber;
                        objEndMockExamDetailResult.Description = data.Description;
                        ObjOCRP.EndOfTermExaminations.Add(objEndMockExamDetailResult);
                        ObjOCRP.SaveChanges();
                        endmockId = objEndMockExamDetailResult.ID;
                    }
                    else
                    {
                        returnResult = "Registration no already exists !";
                        return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    var objEndofTermMockResult = ObjOCRP.EndOfTermExaminationsResults.FirstOrDefault(C => (C.EndofTermId == endmockId && C.SubjectName == data.SubjectName));
                    if (objEndofTermMockResult == null)
                    {
                        objEndofTermMockResult = new EndOfTermExaminationsResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                            objEndofTermMockResult.EndofTermId = endmockId;
                            objEndofTermMockResult.SubjectName = data.SubjectName;
                            objEndofTermMockResult.Grade = data.Grade;
                            objEndofTermMockResult.Remarks = data.Remarks;
                            objEndofTermMockResult.CreatedBy = createdBy;
                            objEndofTermMockResult.CreatedDate = DateTime.Now;
                            ObjOCRP.EndOfTermExaminationsResults.Add(objEndofTermMockResult);
                            ObjOCRP.SaveChanges();
                        }
                    }


                }

                else
                {
                    if (endmockId == Utility.Number.Zero)
                    {
                        endmockId = objEndMockExamDetailResult.ID;
                    }
                    else if (endmockId != Utility.Number.Zero)
                    {

                    }
                    else
                    {
                        endmockId = data.UpdateID;
                    }

                    var objEndMockExamResults = ObjOCRP.EndOfTermExaminationsResults.FirstOrDefault(C => (C.EndofTermId == endmockId && C.SubjectName == data.SubjectName));
                    if (objEndMockExamResults == null)
                    {
                        objEndMockExamResults = new EndOfTermExaminationsResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                            objEndMockExamResults.EndofTermId = endmockId;
                            objEndMockExamResults.SubjectName = data.SubjectName;
                            objEndMockExamResults.Grade = data.Grade;
                            objEndMockExamResults.Remarks = data.Remarks;
                            objEndMockExamResults.CreatedBy = createdBy;
                            objEndMockExamResults.CreatedDate = DateTime.Now;
                            ObjOCRP.EndOfTermExaminationsResults.Add(objEndMockExamResults);
                            ObjOCRP.SaveChanges();
                        }
                    }
                    else
                    {
                        objEndMockExamResults.EndofTermId = data.UpdateID;
                        objEndMockExamResults.SubjectName = data.SubjectName;
                        objEndMockExamResults.Grade = data.Grade;
                        objEndMockExamResults.Remarks = data.Remarks;
                        objEndMockExamResults.UpdateBy = createdBy;
                        objEndMockExamResults.UpdatedDate = DateTime.Now;
                        ObjOCRP.SaveChanges();
                    }
                }



            }
            returnResult = endmockId.ToString();
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to upload result excel sheet by mockExamID.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult UploadEndofTermMockExamExcelSheet(int UserId)
        {
            string imageNameNew = Utility.NewNumber();

            string returnResult = string.Empty;
            string Title = string.Empty;
            string regitrationNo = string.Empty;
            var objJsceResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.ID == UserId));
            if (objJsceResult != null)
            {
                regitrationNo = objJsceResult.RegistrationNumber;
            }
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    //  Title = file.FileName;
                    for (int i = 0; i < files.Count; i++)
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                        string filename = Path.GetFileName(Request.Files[i].FileName);
                        string strpath = Path.GetExtension(filename);
                        Title = regitrationNo + strpath;
                        //title =  file.FileName;
                        HttpPostedFileBase file = files[i];
                        // string ID = Request.QueryString["/AdminUserList/SaveProfilePicture?id"].ToString();
                        //  string filename;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            filename = testfiles[testfiles.Length - 1];
                        }
                        else
                        {

                            //filename = file.FileName;
                        }

                        //returnResult = filename;

                        //  Get the complete folder path and store the file inside it.  
                        filename = Path.Combine(Server.MapPath("~/StudentExecelSheet/"), Title);
                        file.SaveAs(filename);


                        //Save profile Image.
                        var objEndMockExamantion = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => c.ID == UserId);
                        if (objEndMockExamantion != null)
                        {
                            objEndMockExamantion.FileName = Title;
                        }
                        ObjOCRP.SaveChanges();
                        returnResult = Models.Utility.Message.Add_Message;
                    }


                    return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        /// <summary>
        /// This method use result download by registration no.
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <returns></returns>
        public ActionResult ResultDownloadFile(Int32 RegistrationId = Models.Utility.Number.Zero)
        {
            string fileName = string.Empty;
            string regitrationNo = Convert.ToString(RegistrationId);
            var objEndofTermMockExamResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == regitrationNo));
            if (objEndofTermMockExamResult != null)
            {
                fileName = objEndofTermMockExamResult.FileName;
            }
            return new JsonResult { Data = fileName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
    }
}
