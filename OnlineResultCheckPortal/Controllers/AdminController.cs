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
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        int StudendtId;
        public ActionResult Index()
        {
            SchoolName();
            AcademicYear();
           
            return View();
        }


        /// <summary>
        /// This method use to get user profile  by created By ID for Admin.
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProfile()
        {

            string returnResult = string.Empty;
            try
            {
                // get Start (paging start index) and length (page size for paging)
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                string search = Request.Form.GetValues("search[value]")[0];
                if (search != string.Empty)
                {
                    try
                    {
                        //Get Sort columns value
                        var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                        var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                        int pageSize = length != null ? Convert.ToInt32(length) : 0;
                        int skip = start != null ? Convert.ToInt32(start) : 0;
                        int totalRecords = 0;

                        using (OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal())
                        {
                            var ObjUserProfile = ObjOCRP.SearchStudent(search).ToList();

                            //Sorting
                           
                            totalRecords = ObjUserProfile.Count();
                            var data = ObjUserProfile.Skip(skip).Take(pageSize).ToList();
                            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    catch (Exception Ex)
                    {

                    }

                }
                else
                {
                    //Get Sort columns value
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int totalRecords = 0;

                    using (OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal())
                    {
                        var ObjUserProfile = ObjOCRP.AdminUserProfileDisplay().ToList();

                        //Sorting
                        totalRecords = ObjUserProfile.Count();
                        var data = ObjUserProfile.Skip(skip).Take(pageSize).ToList();
                        return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        /// <summary>
        /// This method use get get amdin profile by Admin id.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAdminPrfoile()
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            Int32 roleId = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["RoleId"] != null)
            {
                roleId = Convert.ToInt32(Session["RoleId"]);
            }
           


                var ObjAdminProfile = ObjOCRP.AdminProfile(createdBy).ToList();
                return new JsonResult { Data = ObjAdminProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This method use to delete by Admin id.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUserProfile(Int32 userID = Models.Utility.Number.Zero)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objUsersProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == userID));

            if (objUsersProfile != null)
            {
                objUsersProfile.IsDeleted = true;
                objUsersProfile.IsDeletedBy = createdBy;
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Delete_Message;

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }
        /// <summary>
        /// This method use to update student profile by student id.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserProfile(Int32 userID = Models.Utility.Number.Zero)
        {
            if (userID !=null) {
                var ObjEditUserProfile = ObjOCRP.EditUserProfile(userID).ToList();
                return new JsonResult { Data = ObjEditUserProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }
       
        /// <summary>
        /// This method use to get AcademicYear.
        /// </summary>
        public void AcademicYear()
        {

            ViewBag.AcademicYear = new SelectList(ObjOCRP.Academic_Sessions.ToList(), "ID", "AcademicYear");

        }
        /// <summary>
        /// This method use to get Scholl name.
        /// </summary>
        public void SchoolName()
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.GetSchool().ToList(), "ID", "SchoolName");

        }

        /// <summary>
        /// This method use to Update user profile by Admin id.
        /// </summary>
        /// <param name="objRegistration"></param>
        /// <returns></returns>
        public ActionResult UpdateUserProfile(Models.Student objRegistration)
        {
            string returnResult = string.Empty;
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            try
            {
                var ObjUserNewProfile = ObjOCRP.Users.FirstOrDefault(c => (c.ID == objRegistration.ID));
                if (ObjUserNewProfile != null)
                {
                    ObjUserNewProfile.FirstName = objRegistration.FirstName;
                    ObjUserNewProfile.LastName = objRegistration.Lastname;
                    ObjUserNewProfile.StudentID = objRegistration.StudentId;
                    ObjOCRP.SaveChanges();

                    //Updat data student table.
                    var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == objRegistration.ID));
                    if (ObjStudnentProfile != null)
                    {
             
                        ObjStudnentProfile.Dob = objRegistration.DOB;
                        ObjStudnentProfile.AcademicYear = objRegistration.AcademicYear;
                        ObjStudnentProfile.School = objRegistration.School;
                        ObjStudnentProfile.Address = objRegistration.Address;
                        ObjStudnentProfile.Gender = objRegistration.Gender;
                        ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                        ObjStudnentProfile.State = objRegistration.State;
                        ObjOCRP.SaveChanges();
                    }
                    returnResult = Models.Utility.Message.Update_Message;
                }
                else
                {
                     ObjUserNewProfile = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == objRegistration.StudentId));
                    if (ObjUserNewProfile==null)
                    {
                        // Saving User Detail in User Table.
                        ObjUserNewProfile = new User();
                        ObjUserNewProfile.FirstName = objRegistration.FirstName;
                        ObjUserNewProfile.LastName = objRegistration.Lastname;
                        ObjUserNewProfile.StudentID = objRegistration.StudentId;
                        ObjUserNewProfile.RoleId =2;
                        ObjUserNewProfile.CreatedBy = createdBy;
                        ObjUserNewProfile.IsDeleted = false;
                        ObjUserNewProfile.IsApproved = true;
                        ObjUserNewProfile.CreatedDate = DateTime.Now;
                        ObjOCRP.Users.Add(ObjUserNewProfile);
                        ObjOCRP.SaveChanges();

                        //Saving data student table.
                        var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == ObjUserNewProfile.ID));
                        if (ObjStudnentProfile == null)
                        {
                            ObjStudnentProfile = new Student();
                            ObjStudnentProfile.Dob = objRegistration.DOB;
                            ObjStudnentProfile.AcademicYear = objRegistration.AcademicYear;
                            ObjStudnentProfile.School = objRegistration.School;
                            ObjStudnentProfile.Address = objRegistration.Address;
                            ObjStudnentProfile.Gender = objRegistration.Gender;
                            ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                            ObjStudnentProfile.State = objRegistration.State;
                            ObjStudnentProfile.UserId = ObjUserNewProfile.ID;
                            ObjOCRP.Students.Add(ObjStudnentProfile);
                            ObjOCRP.SaveChanges();
                        }
                        returnResult = Models.Utility.Message.Add_Message;

                    }
                    else
                    {
                        returnResult = Models.Utility.Message.RegistrationNumbers;

                    }
                  
                   
                }
               
            }
            catch(Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to approved Student 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult ApprovedStudent(Int32 userID = Models.Utility.Number.Zero)
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
            if (ObjUserProfile!=null)
            {
                ObjUserProfile.IsApproved = true;
                ObjUserProfile.IsApprovedBy =createdBy;
                ObjOCRP.SaveChanges();

                //Get firstname and lastname from user.
                var name = ObjUserProfile.FirstName + " " + ObjUserProfile.LastName;
                //emailId = ObjUserProfile.EmailID;
                emailId = "rajuk@rezinfo.co.in";
                subject = "Result Masta : Account approval";
                body = "Hi " + name + ",<br/><br/><font color=green><b>You account has been approved by Administrator.</font><br/></b><br/>Please click here to <a href='http://resultmasta.com.yew.arvixe.com/Login/Index' >signIn</a><br/><br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it
                Models.Utility.sendMail(emailId, subject, body);

                returnResult = Models.Utility.Message.IsApproved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// This method use to get display student profile by Admin id.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult GetStudentDetails(Int32 userID = Models.Utility.Number.Zero)
        {
               var objStudenDetails = ObjOCRP.StudentDetails(userID).ToList();
               return new JsonResult { Data = objStudenDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        /// <summary>
        /// This method use to Update user profile by Admin id.
        /// </summary>
        /// <returns></returns>
      public ActionResult UpdateStudentProfile(Models.Student objUserPdrofile)
      {
            string returnResult = string.Empty;

            var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == objUserPdrofile.UserId));
            if (ObjStudnentProfile == null)
            {
                ObjStudnentProfile = new Student();
                ObjStudnentProfile.FatherName = objUserPdrofile.FatherName;
                ObjStudnentProfile.Dob = objUserPdrofile.DOB;
                ObjStudnentProfile.Contact = objUserPdrofile.Contact;
                ObjStudnentProfile.AcademicYear = objUserPdrofile.AcademicYear;
                ObjStudnentProfile.School = objUserPdrofile.School;
                ObjStudnentProfile.Address = objUserPdrofile.Address;
                ObjStudnentProfile.Gender = objUserPdrofile.Gender;
                ObjStudnentProfile.UserId = objUserPdrofile.UserId;
                ObjOCRP.Students.Add(ObjStudnentProfile);
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Update_Message;
               
            }
            else
            {
                ObjStudnentProfile.FatherName = objUserPdrofile.FatherName;
                ObjStudnentProfile.Dob = objUserPdrofile.DOB;
                ObjStudnentProfile.Contact = objUserPdrofile.Contact;
                ObjStudnentProfile.AcademicYear = objUserPdrofile.AcademicYear;
                ObjStudnentProfile.School = objUserPdrofile.School;
                ObjStudnentProfile.Address = objUserPdrofile.Address;
                ObjStudnentProfile.Gender = objUserPdrofile.Gender;
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Update_Message;

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            StudendtId = objUserPdrofile.UserId;
        }
        /// <summary>
        /// This method use to UnApprovedStudent by User Id.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult UnApprovedStudent(Int32 userID = Models.Utility.Number.Zero)
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
                ObjUserProfile.IsApproved = false;
                ObjUserProfile.IsApprovedBy = createdBy;
                ObjOCRP.SaveChanges();

                //Get firstname and lastname from user.
                var name = ObjUserProfile.FirstName + " " + ObjUserProfile.LastName;
                emailId = ObjUserProfile.EmailID;
              
                subject = "Result Masta : Account Unapproval";
                body = "Hi " + name + ",<br/><br/><font color=green><b>You account has been Unapproved by Administrator.</font><br/></b><br/>Please contact admin for more information.<br/><br/><br/><br/><br/>" + " Thanks and Regards," + "<br/>";//edit it
                Models.Utility.sendMail(emailId, subject, body);

                returnResult = Models.Utility.Message.IsUnApproved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveStudentProfileImage(string studentName)
        {
            int studentId = Convert.ToInt16(studentName);
            string title = string.Empty;
            string returnResult = string.Empty;
            try
            {
                // save file
                foreach (string fileName in Request.Files)
                {
                    //get Auto Generate number.
                    string imageNameNew = Models.Utility.NewNumber();
                    HttpPostedFileBase file = Request.Files[fileName];
                    title = imageNameNew + ' ' + file.FileName;
                    if (file != null && file.ContentLength > Models.Utility.Number.Zero)
                    {
                        // Getting folder path to save image
                        var Imagepath = Path.Combine(Server.MapPath("~/StudentPhoto/"));
                        string pathString = System.IO.Path.Combine(Imagepath.ToString());
                        var namepath = Path.GetFileName(file.FileName);
                        //Checking image already exist in folder
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{" + Models.Utility.Number.Zero + "}\\{" + Models.Utility.Number.One + "}", pathString, imageNameNew + ' ' + file.FileName);
                        file.SaveAs(uploadpath);
                        {
                            var objStudentProfile = ObjOCRP.Students.FirstOrDefault(c => c.UserId == studentId);
                            if (objStudentProfile == null)
                            {
                                objStudentProfile = new Student();
                                objStudentProfile.Picture = title;
                                objStudentProfile.UserId = studentId;
                                ObjOCRP.Students.Add(objStudentProfile);
                                ObjOCRP.SaveChanges();
                                returnResult = Models.Utility.Message.Add_Message;
                            }
                            else
                            {
                                objStudentProfile.Picture = title;
                                ObjOCRP.SaveChanges();

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                returnResult = Models.Utility.Message.RecordUnsaved;
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

     
        /// <summary>
        /// This method use to Import excel sheet  student register.
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase uploadFile)
        {
            int Addcount = 0;
            int update = 0;
            int i = 0;
           
            string School=string.Empty;
            // 
            Models.Student objRegistration = new Models.Student();
            
            string returnResult = string.Empty;
            StringBuilder strValidations = new StringBuilder(string.Empty);

            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            if (uploadFile.ContentLength > 0)
            {
                
                try
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/StudentExecelSheet/"),
                    Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);
                    DataSet ds = new DataSet();
                    string ConnectionString = string.Empty;
                    //A 32-bit provider which enables the use of

                    //  string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    //filePath + ";Extended Properties=\"Excel 12.0;HDR=No;IMEX=2\"";

                    ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
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

                                        string studentId = (ds.Tables[0].Rows[i].ItemArray[0]).ToString();
                                        objRegistration.FirstName = (ds.Tables[0].Rows[i].ItemArray[1]).ToString();
                                        objRegistration.Lastname = (ds.Tables[0].Rows[i].ItemArray[2]).ToString();
                                        objRegistration.DOB = (ds.Tables[0].Rows[i].ItemArray[3]).ToString();
                                        objRegistration.Address = (ds.Tables[0].Rows[i].ItemArray[4]).ToString();
                                        School = (ds.Tables[0].Rows[i].ItemArray[5]).ToString();
                                        objRegistration.LocalGoverment = (ds.Tables[0].Rows[i].ItemArray[6]).ToString();
                                        objRegistration.State = (ds.Tables[0].Rows[i].ItemArray[7]).ToString();
                                        objRegistration.Gender = (ds.Tables[0].Rows[i].ItemArray[8]).ToString();
                                        objRegistration.AcademicYear = (ds.Tables[0].Rows[i].ItemArray[9]).ToString();
                                        if (School != null)
                                        {
                                            var objCreateSchool = ObjOCRP.Schools.FirstOrDefault(c => (c.SchoolName == School));
                                            if (objCreateSchool == null)
                                            {
                                                objCreateSchool = new School();
                                                objCreateSchool.SchoolName = School;
                                                objCreateSchool.IsDeleted = false;
                                                ObjOCRP.Schools.Add(objCreateSchool);
                                                ObjOCRP.SaveChanges();
                                                objRegistration.School = objCreateSchool.ID;
                                            }
                                            else
                                            {
                                                objRegistration.School = objCreateSchool.ID;
                                            }
                                            if (objRegistration.AcademicYear != null)
                                            {
                                                var objAcadmicSchool = ObjOCRP.Academic_Sessions.FirstOrDefault(c => (c.AcademicYear == objRegistration.AcademicYear));
                                                if (objAcadmicSchool == null)
                                                {
                                                    objAcadmicSchool = new Academic_Session();
                                                    objAcadmicSchool.AcademicYear = objRegistration.AcademicYear;
                                                    ObjOCRP.Academic_Sessions.Add(objAcadmicSchool);
                                                    ObjOCRP.SaveChanges();
                                                    objRegistration.AcadmicID = objAcadmicSchool.ID;
                                                }
                                                else
                                                {
                                                    objRegistration.AcadmicID = objAcadmicSchool.ID;
                                                }
                                            }

                                            var objStudentRegister = ObjOCRP.Users.FirstOrDefault(c => (c.StudentID == studentId));
                                            if (objStudentRegister == null)
                                            {

                                                Addcount = Addcount + 1;
                                                objStudentRegister = new User();
                                                objStudentRegister.StudentID = studentId;
                                                objStudentRegister.RoleId = 2;
                                                objStudentRegister.IsDeleted = false;
                                                objStudentRegister.CreatedDate = DateTime.Now;
                                                objStudentRegister.CreatedBy = createdBy;
                                                objStudentRegister.IsApproved = true;
                                                objStudentRegister.FirstName = objRegistration.FirstName;
                                                objStudentRegister.LastName = objRegistration.Lastname;
                                                ObjOCRP.Users.Add(objStudentRegister);
                                                ObjOCRP.SaveChanges();

                                                //save data in student table.
                                                var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == objStudentRegister.ID));
                                                if (ObjStudnentProfile == null)
                                                {

                                                    ObjStudnentProfile = new Student();
                                                    ObjStudnentProfile.Dob = objRegistration.DOB;
                                                    ObjStudnentProfile.AcademicYear = objRegistration.AcadmicID.ToString();
                                                    ObjStudnentProfile.School = objRegistration.School;
                                                    ObjStudnentProfile.Address = objRegistration.Address;
                                                    ObjStudnentProfile.Gender = objRegistration.Gender;
                                                    ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                                                    ObjStudnentProfile.State = objRegistration.State;
                                                    ObjStudnentProfile.UserId = objStudentRegister.ID;
                                                    ObjOCRP.Students.Add(ObjStudnentProfile);
                                                    ObjOCRP.SaveChanges();

                                                }

                                                //returnResult = "<br/><font color=white><b>Import new record sccessfully.</font><br/></b>Add total columns:" + count + "<br/>Add total row:"+totalRows+"<br/><br/>";//edit it
                                            }
                                            else
                                            {
                                                update = update + 1;
                                                objStudentRegister.FirstName = objRegistration.FirstName;
                                                objStudentRegister.LastName = objRegistration.Lastname;
                                                ObjOCRP.SaveChanges();

                                                //Updat data student table.
                                                var ObjStudnentProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == objRegistration.ID));
                                                if (ObjStudnentProfile != null)
                                                {

                                                    ObjStudnentProfile.Dob = objRegistration.DOB;
                                                    ObjStudnentProfile.AcademicYear = objRegistration.AcadmicID.ToString();
                                                    ObjStudnentProfile.School = objRegistration.School; ;
                                                    ObjStudnentProfile.Address = objRegistration.Address;
                                                    ObjStudnentProfile.Gender = objRegistration.Gender;
                                                    ObjStudnentProfile.LocalGovernment = objRegistration.LocalGoverment;
                                                    ObjStudnentProfile.State = objRegistration.State;
                                                    ObjOCRP.SaveChanges();
                                                }
                                            }
                                        }

                                        returnResult = "<br/><font color=white><b>Add new record total: " + Addcount + "</br></br>Update record total: " + update + "</br></b></font><br/>";//edit it    
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    returnResult = "Your file not correct format.";
                }  
            }
            else
            {
                returnResult = "Excel sheet empty.";
            }

            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }

}
