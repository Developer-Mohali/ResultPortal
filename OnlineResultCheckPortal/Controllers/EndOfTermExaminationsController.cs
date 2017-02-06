using OnlineResultCheckPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
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
            try
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
                    var objEndofTerm = ObjOCRP.GetDetailsEndMockExamanation().ToList();
                    //Sorting
                    totalRecords = objEndofTerm.Count();
                    var data = objEndofTerm.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        /// <summary>
        /// This method use to get value and mock examanation table by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult GetEditEndMockExamanation(Int32 Id = Models.Utility.Number.Zero)
        {
            var objJSCEResults = ObjOCRP.GetEndMockExamanationEdit(Id).ToList();
            return new JsonResult { Data = objJSCEResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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
                    var objEndMockExam = ObjOCRP.EndOfTermExaminations.FirstOrDefault(C => (C.ID == endmockId));
                    if (objEndMockExam != null && data.Description != null)
                    {
                        objEndMockExam.Description = data.Description;
                        ObjOCRP.SaveChanges();
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
                        filename = Path.Combine(Server.MapPath("~/EndofTermExam/"), Title);
                        file.SaveAs(filename);


                        //Save profile Image.
                        var objEndMockExamantion = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => c.ID == UserId);
                        if (objEndMockExamantion != null)
                        {
                            objEndMockExamantion.ReportCardFile = Title;
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
                fileName = objEndofTermMockExamResult.ReportCardFile;
            }
            if (fileName == null)
            {
                fileName = "Result not uploaded";
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

            var filepath = System.IO.Path.Combine(Server.MapPath("/EndofTermExam/"), file);
            return File(filepath, MimeMapping.GetMimeMapping(filepath), file);

        }
        /// <summary>
        /// This method use to Import excel sheet  Result Import.
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase uploadFile)
        {
            int Addcount = 0;
            int update = 0;
            int notvalid = 0;
            int i = 0;
            int endOFtermId = 0;
            int studentID = 0;
            string School = string.Empty;
            // 
            Models.EndMockExamanation objEndofTermExam = new Models.EndMockExamanation();

            string returnResult = string.Empty;
            StringBuilder strValidations = new StringBuilder(string.Empty);

            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }

            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/StudentExecelSheet/"),
                    Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    DataSet ds = new DataSet();

                    //A 32-bit provider which enables the use of

                    //  string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    //filePath + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=2\"";
                    string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
                    using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    {
                        conn.Open();

                        using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                        {

                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            string query = "SELECT * FROM [" + sheetName + "]";
                            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                            //DataSet ds = new DataSet();
                            adapter.Fill(ds, "Items");
                            if (ds.Tables.Count > 0)
                            {
                                int totalColumns = dtExcelSchema.Columns.Count;
                                int totalRows = dtExcelSchema.Rows.Count;
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {

                                        //Now we can insert this data to database...
                                        objEndofTermExam.StudentID = (ds.Tables[0].Rows[i].ItemArray[0]).ToString();
                                        objEndofTermExam.RegistrationNumber = (ds.Tables[0].Rows[i].ItemArray[1].ToString());
                                        objEndofTermExam.SubjectName = (ds.Tables[0].Rows[i].ItemArray[2].ToString());
                                        objEndofTermExam.Grade = (ds.Tables[0].Rows[i].ItemArray[3].ToString());
                                        objEndofTermExam.Remarks = (ds.Tables[0].Rows[i].ItemArray[4].ToString());

                                        var objGetStudentName = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == objEndofTermExam.StudentID));
                                        if (objGetStudentName != null)
                                        {
                                            studentID = objGetStudentName.ID;
                                        }
                                        var objGetEndofTermID = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == objEndofTermExam.RegistrationNumber));
                                        if (objGetEndofTermID != null)
                                        {
                                            endOFtermId = objGetEndofTermID.ID;
                                        }
                                        if (StudentVarification(objEndofTermExam.StudentID) == true)
                                        {
                                            var objEndofTermExamDetailResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == objEndofTermExam.RegistrationNumber && c.StudentID == objEndofTermExam.ID));
                                            //  Idjsce = objJsceDetailResult.ID;
                                            if (objEndofTermExamDetailResult == null)
                                            {
                                                objEndofTermExamDetailResult = ObjOCRP.EndOfTermExaminations.FirstOrDefault(c => (c.RegistrationNumber == objEndofTermExam.RegistrationNumber));
                                                if (objEndofTermExamDetailResult == null)
                                                {
                                                    Addcount = Addcount + 1;
                                                    objEndofTermExamDetailResult = new EndOfTermExamination();
                                                    objEndofTermExamDetailResult.StudentID = studentID;
                                                    objEndofTermExamDetailResult.IsDeleted = false;
                                                    objEndofTermExamDetailResult.RegistrationNumber = objEndofTermExam.RegistrationNumber;
                                                    objEndofTermExamDetailResult.Description = objEndofTermExam.Description;
                                                    ObjOCRP.EndOfTermExaminations.Add(objEndofTermExamDetailResult);
                                                    ObjOCRP.SaveChanges();
                                                    endOFtermId = objEndofTermExamDetailResult.ID;
                                                }

                                                var objResults = ObjOCRP.EndOfTermExaminationsResults.FirstOrDefault(C => (C.EndofTermId == endOFtermId && C.SubjectName == objEndofTermExam.SubjectName));
                                                if (objResults == null)
                                                {

                                                    objResults = new EndOfTermExaminationsResult();
                                                    if (objEndofTermExam.Remarks == null && objEndofTermExam.Grade == null)
                                                    {

                                                    }
                                                    else
                                                    {

                                                        objResults.EndofTermId = endOFtermId;
                                                        objResults.SubjectName = objEndofTermExam.SubjectName;
                                                        objResults.Grade = objEndofTermExam.Grade;
                                                        objResults.Remarks = objEndofTermExam.Remarks;
                                                        objResults.CreatedBy = createdBy;
                                                        objResults.CreatedDate = DateTime.Now;
                                                        ObjOCRP.EndOfTermExaminationsResults.Add(objResults);
                                                        ObjOCRP.SaveChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    update = update + 1;
                                                    if (objEndofTermExam.Remarks == null && objEndofTermExam.Grade == null)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        update = update + 1;
                                                        objResults.EndofTermId = endOFtermId;
                                                        objResults.SubjectName = objEndofTermExam.SubjectName;
                                                        objResults.Grade = objEndofTermExam.Grade;
                                                        objResults.Remarks = objEndofTermExam.Remarks;
                                                        objResults.CreatedBy = createdBy;
                                                        objResults.UpdatedDate = DateTime.Now;

                                                        ObjOCRP.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            notvalid = notvalid + 1;
                                        }

                                        returnResult = "<br/><font color=white><b>Add new record total: " + Addcount + "</br></br>Not valid studentID total: " + notvalid + "</br></b></b></font><br/>";//edit it    
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "Already exists";
            }

            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to StudentVarification from Student Table.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        private bool StudentVarification(string studentID)
        {
            bool flag = false;
            try
            {
                var objToken = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == studentID));
                if (objToken != null)
                {
                    flag = true;
                }
            }
            catch (Exception EX)
            {

            }
            return flag;
        }
    }
}
