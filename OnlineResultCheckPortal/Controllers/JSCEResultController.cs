using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineResultCheckPortal.Models;
using System.IO;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace OnlineResultCheckPortal.Controllers
{
    public class JSCEResultController : Controller
    {
        //
        // GET: /JSCEResult/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {

            LoadManageJSCE();
            return View();
        }
        /// <summary>
        /// This method use to bind dropdown JSCE name.
        /// </summary>
        public void LoadManageJSCE()
        {

            ViewBag.FirstName = new SelectList(ObjOCRP.GetStudentName().ToList(), "ID", "Name");

        }
        //public void LoadJSCEID()
        //{
        //    ViewBag.JSCEID = new SelectList(ObjOCRP.JSCEResults.ToList(), "ID", "StudentID");
        //}
        /// <summary>
        /// This method use to get DisplayManageJSCE profile by userId. 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult DisplayManageJSCEResult()
        {
            string returnResult = string.Empty;
            Int32 createdBy = Utility.Number.Zero;
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var ObjAdmin = ObjOCRP.JSCEResults.Where(c => (c.ID == createdBy));
            if (ObjAdmin != null)
            {
                var objJSCEDetails = ObjOCRP.GetUploadResultDetails().ToList();
                return new JsonResult { Data = objJSCEDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
            return View();
    }
    ///// <summary>
    ///// This method use to Inser data mockExaminations table.
    ///// </summary>
    ///// <param name="objJSCEResults"></param>
    ///// <returns></returns>
    //[HttpPost]
    //    public ActionResult InsertJSCEResult(Models.JSCEDetails objJSCEResults)
    //    {
    //        string returnResult = string.Empty;
    //        Int32 createdBy = Models.Utility.Number.Zero;
    //        //Getting user id by session.
    //        if (Session["UserId"] != null)
    //        {
    //            createdBy = Convert.ToInt32(Session["UserId"]);
    //        }
    //        try
    //        {
    //            var objJSCEResult = ObjOCRP.JSCEDetails.FirstOrDefault(C => (C.ID == objJSCEResults.ID));
    //            if (objJSCEResult == null)
    //            {
    //                objJSCEResult = new JSCEDetail();
    //                objJSCEResult.RegistrationNumber = objJSCEResults.RegistrationNumber;
    //                objJSCEResult.Description = objJSCEResults.Description;
    //                //objJSCEResult.FileName = objJSCEResults.FileName;
    //                objJSCEResults.ReportCardFile = objJSCEResults.ReportCardFile;

    //                ObjOCRP.JSCEDetails.Add(objJSCEResult);
    //                ObjOCRP.SaveChanges();

    //                int jsceId = objJSCEResult.ID;
    //                returnResult = Models.Utility.Message.Add_Message;
    //            }
    //            else
    //            {
    //                var objUpdateJSCEResult = ObjOCRP.JSCEResults.FirstOrDefault(c => c.ID == objJSCEResults.ID);
    //                if (objUpdateJSCEResult != null)
    //                {

    //                    objJSCEResult.RegistrationNumber = objJSCEResults.RegistrationNumber;
    //                    objJSCEResult.Description = objJSCEResults.Description;
    //                    //    objJSCEResult.FileName = objJSCEResults.FileName;
    //                    objJSCEResults.ReportCardFile = objJSCEResults.ReportCardFile;
    //                    ObjOCRP.SaveChanges();
    //                    returnResult = Models.Utility.Message.Update_Message;
    //                }
    //                var objJSCE = ObjOCRP.JSCEResults.FirstOrDefault(C => (C.ID == objJSCEResults.ID));
    //                if (objJSCE == null)
    //                {
    //                    objJSCE = new JSCEResult();
    //                    objJSCE.JSCEID = objJSCEResult.ID;
    //                    objJSCE.SubjectName = objJSCEResults.SubjectName;
    //                    objJSCE.Grade = objJSCEResults.Grade;
    //                    objJSCE.Remarks = objJSCEResults.Remarks;
    //                    objJSCE.IsDeleted = false;
    //                    objJSCE.CreatedBy = objJSCE.CreatedBy;
    //                    objJSCE.UpdateBy = objJSCE.UpdateBy;
    //                    ObjOCRP.JSCEResults.Add(objJSCE);
    //                    ObjOCRP.SaveChanges();
    //                    // int jsceId = objJSCEResult.ID;
    //                    returnResult = Models.Utility.Message.Add_Message;
    //                }
    //                else
    //                {
    //                    var objUpdateJSCEResults = ObjOCRP.JSCEResults.FirstOrDefault(c => c.ID == objJSCEResults.ID);
    //                    if (objUpdateJSCEResults != null)
    //                    {
    //                        objJSCE.SubjectName = objJSCEResults.SubjectName;
    //                        objJSCE.Grade = objJSCEResults.Grade;
    //                        objJSCE.Remarks = objJSCEResults.Remarks;
    //                        objJSCE.UpdatedDate = System.DateTime.Now;
    //                        ObjOCRP.SaveChanges();
    //                        returnResult = Models.Utility.Message.Update_Message;
    //                    }
    //                }

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            returnResult = Models.Utility.Message.RecordUnsaved;
    //        }
    //        return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
    //    }
        /// <summary>
        /// This method use to get value and mock examanation table by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult EditJSCEResultDetails(Int32 Id = Models.Utility.Number.Zero)
        {
              var objJSCEResults = ObjOCRP.GetJSCEResultEdit(Id).ToList();
              return new JsonResult { Data = objJSCEResults, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
        }
        /// <summary>
        ///This method use to DeleteJSCEResult profile by User Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteJSCEResult(Int32 Id = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objJSCEResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.ID == Id));
            try
            {
                if (objJSCEResult != null)
                {
                    objJSCEResult.IsDeleted = true;
                     objJSCEResult.IsDeletedByDate = DateTime.Now;
                      objJSCEResult.IsDeletedBy = createdBy;
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
        /// This method use to upload student result.
        /// </summary>
        /// <param name="objJsceDetails"></param>
        /// <returns></returns>
        public ActionResult ResultUpload(List<Models.JSCEDetails> objJsceDetails)
        {
            int jSCEId = 0;
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            foreach (var data in objJsceDetails)
            {
              
                var objJsceDetailResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber && c.StudentID==data.ID));
              //  Idjsce = objJsceDetailResult.ID;
                if (objJsceDetailResult == null)
                {
                    objJsceDetailResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == data.RegistrationNumber));
                    if (objJsceDetailResult==null)
                    {
                        objJsceDetailResult = new JSCEDetail();
                        objJsceDetailResult.StudentID = data.ID;
                        objJsceDetailResult.IsDeleted = false;
                        objJsceDetailResult.RegistrationNumber = data.RegistrationNumber;
                        objJsceDetailResult.Description = data.Description;
                        ObjOCRP.JSCEDetails.Add(objJsceDetailResult);
                        ObjOCRP.SaveChanges();
                        jSCEId = objJsceDetailResult.ID;
                    }
                    else
                    {
                        returnResult = "Registration no already exists !";
                        return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    var objResults = ObjOCRP.JSCEResults.FirstOrDefault(C => (C.JSCEID == jSCEId && C.SubjectName == data.SubjectName));
                    if (objResults == null)
                    {
                        objResults = new JSCEResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                           

                            objResults.JSCEID = jSCEId;
                            objResults.SubjectName = data.SubjectName;
                            objResults.Grade = data.Grade;
                            objResults.Remarks = data.Remarks;
                            objResults.CreatedBy = createdBy;
                            objResults.CreatedDate = DateTime.Now;
                            ObjOCRP.JSCEResults.Add(objResults);
                            ObjOCRP.SaveChanges();
                        }
                    }
                    
                

                }
               
            else
                {
                   
                    if (jSCEId == Utility.Number.Zero)
                    {
                        jSCEId = objJsceDetailResult.ID;
                    }
                    else if(jSCEId!= Utility.Number.Zero)
                    {

                    }
                    else
                    {
                        jSCEId = data.UpdateID;
                    }
                    var objce = ObjOCRP.JSCEDetails.FirstOrDefault(C => (C.ID == jSCEId));
                    if (objce != null && data.Description!=null)
                    {
                        objce.Description = data.Description;
                        ObjOCRP.SaveChanges();
                    }
                    var objResults = ObjOCRP.JSCEResults.FirstOrDefault(C => (C.JSCEID == jSCEId && C.SubjectName == data.SubjectName));
                    if (objResults == null)
                    {
                        objResults = new JSCEResult();
                        if (data.Remarks == null && data.Grade == null)
                        {

                        }
                        else
                        {
                            objResults.JSCEID = jSCEId;
                            objResults.SubjectName = data.SubjectName;
                            objResults.Grade = data.Grade;
                            objResults.Remarks = data.Remarks;
                            objResults.CreatedBy = createdBy;
                            objResults.CreatedDate = DateTime.Now;
                            ObjOCRP.JSCEResults.Add(objResults);
                            ObjOCRP.SaveChanges();
                        }
                    }
                    else
                    {
                        objResults.JSCEID = data.UpdateID;
                        objResults.SubjectName = data.SubjectName;
                        objResults.Grade = data.Grade;
                        objResults.Remarks = data.Remarks;
                        objResults.UpdateBy = createdBy;
                        objResults.UpdatedDate = DateTime.Now;
                        ObjOCRP.SaveChanges();
                    }
                }
             
              

            }
            returnResult = jSCEId.ToString();
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to Upload excel sheet student result.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public JsonResult UploadJsceResultExcelSheet(int UserId, string fileUploadName)
        {
            string imageNameNew = Utility.NewNumber();
          
            string returnResult = string.Empty;
            string Title = string.Empty;
            string regitrationNo = string.Empty;
            var objJsceResult = ObjOCRP.JSCEDetails.FirstOrDefault(c=>(c.ID==UserId));
            if (objJsceResult!=null)
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
                        string filename= Path.GetFileName(Request.Files[i].FileName);
                        string strpath = Path.GetExtension(filename);
                        Title =regitrationNo + strpath;
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
                        filename = Path.Combine(Server.MapPath("~/JsceResult/"), Title);
                        file.SaveAs(filename);
                        var objUserProfile = ObjOCRP.JSCEDetails.FirstOrDefault(c => c.ID == UserId);
                        if (fileUploadName == "Result")
                        {
                            //Save profile Image.

                            if (objUserProfile != null)
                            {
                                objUserProfile.ReportCardFile = Title;
                                ObjOCRP.SaveChanges();
                            }
                           
                        }
                    
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
            var objJSCEResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == regitrationNo));
            if (objJSCEResult != null)
            {
                fileName = objJSCEResult.ReportCardFile;
            }
            if(fileName==null)
            {
                fileName = "Result not uploaded";
            }
            //var filepath = System.IO.Path.Combine(Server.MapPath("/StudentPhoto/"), RegistrationId);
            //return File(filepath, MimeMapping.GetMimeMapping(filepath), RegistrationId);
            return new JsonResult { Data = fileName, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This method use to download result.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult DownloadFile(string file)
        {

            var filepath = System.IO.Path.Combine(Server.MapPath("/JsceResult/"), file);
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
            int i = 0;
            int notvalid = 0;
            int jSCEId = 0;
            int studentID = 0;
            string School = string.Empty;
            // 
            Models.JSCEDetails objJSCEDetails = new Models.JSCEDetails();

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
                                        objJSCEDetails.StudentID = (ds.Tables[0].Rows[i].ItemArray[0]).ToString();
                                        objJSCEDetails.RegistrationNumber = (ds.Tables[0].Rows[i].ItemArray[1].ToString());
                                        objJSCEDetails.SubjectName = (ds.Tables[0].Rows[i].ItemArray[2].ToString());
                                        objJSCEDetails.Grade = (ds.Tables[0].Rows[i].ItemArray[3].ToString());
                                        objJSCEDetails.Remarks = (ds.Tables[0].Rows[i].ItemArray[4].ToString());


                                        var objGetStudentName = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == objJSCEDetails.StudentID));
                                        if (objGetStudentName != null)
                                        {
                                            studentID = objGetStudentName.ID;
                                        }
                                        var objGetJSCEID = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == objJSCEDetails.RegistrationNumber));
                                        if (objGetJSCEID != null)
                                        {
                                            jSCEId = objGetJSCEID.ID;
                                        }
                                        if (StudentVarification(objJSCEDetails.StudentID)==true)
                                        {
                                        var objJsceDetailResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == objJSCEDetails.RegistrationNumber && c.StudentID == objJSCEDetails.ID));
                                        //  Idjsce = objJsceDetailResult.ID;
                                        if (objJsceDetailResult == null)
                                        {
                                            objJsceDetailResult = ObjOCRP.JSCEDetails.FirstOrDefault(c => (c.RegistrationNumber == objJSCEDetails.RegistrationNumber));
                                            if (objJsceDetailResult == null)
                                            {
                                                Addcount = Addcount + 1;
                                                objJsceDetailResult = new JSCEDetail();
                                                objJsceDetailResult.StudentID = studentID;
                                                objJsceDetailResult.IsDeleted = false;
                                                objJsceDetailResult.RegistrationNumber = objJSCEDetails.RegistrationNumber;
                                                objJsceDetailResult.Description = objJSCEDetails.Description;
                                                ObjOCRP.JSCEDetails.Add(objJsceDetailResult);
                                                ObjOCRP.SaveChanges();
                                                jSCEId = objJsceDetailResult.ID;
                                            }
                                           
                                            var objResults = ObjOCRP.JSCEResults.FirstOrDefault(C => (C.JSCEID == jSCEId && C.SubjectName == objJSCEDetails.SubjectName));
                                            if (objResults == null)
                                            {
                                              
                                                objResults = new JSCEResult();
                                                if (objJSCEDetails.Remarks == null && objJSCEDetails.Grade == null)
                                                {

                                                }
                                                else
                                                {

                                                    objResults.JSCEID = jSCEId;
                                                    objResults.SubjectName = objJSCEDetails.SubjectName;
                                                    objResults.Grade = objJSCEDetails.Grade;
                                                    objResults.Remarks = objJSCEDetails.Remarks;
                                                    objResults.CreatedBy = createdBy;
                                                    objResults.CreatedDate = DateTime.Now;
                                                    ObjOCRP.JSCEResults.Add(objResults);
                                                    ObjOCRP.SaveChanges();
                                                }
                                            }
                                            else {
                                                
                                                if (objJSCEDetails.Remarks == null && objJSCEDetails.Grade == null)
                                                {

                                                }
                                                else
                                                {
                                                        update = update + 1;
                                                    objResults.JSCEID = jSCEId;
                                                    objResults.SubjectName = objJSCEDetails.SubjectName;
                                                    objResults.Grade = objJSCEDetails.Grade;
                                                    objResults.Remarks = objJSCEDetails.Remarks;
                                                    objResults.CreatedBy = createdBy;
                                                    objResults.UpdatedDate = DateTime.Now;

                                                    ObjOCRP.SaveChanges();
                                                }
                                            }
                                        }
                                        }
                                        else
                                        {
                                             notvalid = notvalid+1;
                                        }
                                        returnResult = "<br/><font color=white><b>Add new record total: " + Addcount + "</br></br>Not valid studentID total: " + notvalid + "</br></b></br>Update record total: " + update + "</br></b></font><br/>";//edit it    
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
                if (objToken!=null)
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