using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineResultCheckPortal.Controllers
{
    public class MockExaminationsController : Controller
    {
        //
        // GET: /MockExaminations/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        
        public ActionResult Index()
        {
            GetStudentNameManageMockExam();
            return View();
        }
        /// <summary>
        /// This method use to get details mock examination.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDetailsMockExaminations()
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
                var ObjUserProfile = ObjOCRP.GetDetailsMockExamanation().ToList();

                return new JsonResult { Data = ObjUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
        
        /// <summary>
        /// This method use to get value and mock examanation by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GetEditMockExamanation(Int32 Id = Models.Utility.Number.Zero)
        {
            var objMockExamanation = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.ID == Id));
            if (objMockExamanation != null)
            {
                var objMockExam = ObjOCRP.GetMockExamanationEdit(Id).ToList();
                return new JsonResult { Data = objMockExam, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            return View();
        }
        /// <summary>
        /// This method use to deleted mock exam result by ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteMockExamanation(Int32 Id = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objMockExamanation = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.ID == Id));
            try
            {
                if (objMockExamanation != null)
                {
                    objMockExamanation.IsDeleted = true;
                    objMockExamanation.IsDeletedByDate = DateTime.Now;
                    objMockExamanation.IsDeletedBy = createdBy;
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
        //  /// <summary>
        //  /// This method use to bind student name.
        //  /// </summary>
        public void GetStudentNameManageMockExam()
        {

            ViewBag.FirstName = new SelectList(ObjOCRP.GetStudentName().ToList(), "ID", "Name");

        }
        /// <summary>
        /// This method use to upload student result MockExamantion.
        /// </summary>
        /// <param name="objJsceDetails"></param>
        /// <returns></returns>
        public ActionResult ResultUploadMockExam(List<Models.MockExam> objMockExamDetails)
        {
            int mockId = 0;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            foreach (var data in objMockExamDetails)
            {

                var objMockExamDetailResult = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber && c.StudentID == data.ID));
                //  Idjsce = objJsceDetailResult.ID;
                if (objMockExamDetailResult == null)
                {
                    objMockExamDetailResult = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber));
                    if (objMockExamDetailResult == null)
                    {
                        objMockExamDetailResult = new MockExamination();
                        objMockExamDetailResult.StudentID = data.ID;
                        objMockExamDetailResult.IsDeleted = false;
                        objMockExamDetailResult.RegistrationNumber = data.RegistrationNumber;
                        objMockExamDetailResult.Description = data.Description;
                        ObjOCRP.MockExaminations.Add(objMockExamDetailResult);
                        ObjOCRP.SaveChanges();
                        mockId = objMockExamDetailResult.ID;
                    }
                    else
                    {
                        returnResult = "Registration no already exists !";
                        return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    var objMockResults = ObjOCRP.MockExamResults.FirstOrDefault(C => (C.MockExamId == mockId && C.SubjectName == data.SubjectName));
                    if (objMockResults == null)
                    {
                        objMockResults = new MockExamResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                            objMockResults.MockExamId = mockId;
                            objMockResults.SubjectName = data.SubjectName;
                            objMockResults.Grade = data.Grade;
                            objMockResults.Remarks = data.Remarks;
                            objMockResults.CreatedBy = createdBy;
                            objMockResults.CreatedDate = DateTime.Now;
                            ObjOCRP.MockExamResults.Add(objMockResults);
                            ObjOCRP.SaveChanges();
                        }
                    }


                }

                else
                {
                    if (mockId == Utility.Number.Zero)
                    {
                        mockId = objMockExamDetailResult.ID;
                    }
                    else if (mockId != Utility.Number.Zero)
                    {

                    }
                    else
                    {
                        mockId = data.UpdateID;
                    }
                  
                    var objMockExamResults = ObjOCRP.MockExamResults.FirstOrDefault(C => (C.MockExamId == mockId && C.SubjectName == data.SubjectName));
                    if (objMockExamResults == null)
                    {
                        objMockExamResults = new MockExamResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                            objMockExamResults.MockExamId = mockId;
                            objMockExamResults.SubjectName = data.SubjectName;
                            objMockExamResults.Grade = data.Grade;
                            objMockExamResults.Remarks = data.Remarks;
                            objMockExamResults.CreatedBy = createdBy;
                            objMockExamResults.CreatedDate = DateTime.Now;
                            ObjOCRP.MockExamResults.Add(objMockExamResults);
                            ObjOCRP.SaveChanges();
                        }
                    }
                    else
                    {
                        objMockExamResults.MockExamId = data.UpdateID;
                        objMockExamResults.SubjectName = data.SubjectName;
                        objMockExamResults.Grade = data.Grade;
                        objMockExamResults.Remarks = data.Remarks;
                        objMockExamResults.UpdateBy = createdBy;
                        objMockExamResults.UpdatedDate = DateTime.Now;
                        ObjOCRP.SaveChanges();
                    }
                }
               


            }
            returnResult = mockId.ToString();
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to upload result excel sheet by mockExamID.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult UploadMockExamExcelSheet(int UserId)
        {
            string imageNameNew = Utility.NewNumber();

            string returnResult = string.Empty;
            string Title = string.Empty;
            string regitrationNo = string.Empty;
            var objJsceResult = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.ID == UserId));
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
                        var objMockExamantion = ObjOCRP.MockExaminations.FirstOrDefault(c => c.ID == UserId);
                        if (objMockExamantion != null)
                        {
                            objMockExamantion.FileName = Title;
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
            var objMockExamantionResult = ObjOCRP.MockExaminations.FirstOrDefault(c => (c.RegistrationNumber == regitrationNo));
            if (objMockExamantionResult != null)
            {
                fileName = objMockExamantionResult.FileName;
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
