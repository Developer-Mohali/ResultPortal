using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineResultCheckPortal.Models;

namespace OnlineResultCheckPortal.Controllers
{
    public class UserProfileController : Controller
    {
        //
        // GET: /UserProfile/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            LoadUserProfile();
            LoadMyStudentList();
            return View();
        }
        /// <summary>
        /// <Description>This method is used to bind Country List to DropDown by ViewBag</Description>
        /// </summary>
        public void LoadUserProfile()
        {

            ViewBag.AcademicYear = new SelectList(ObjOCRP.Academic_Sessions.ToList(), "ID", "AcademicYear");

        }
        /// <summary>
        /// <Description>This method is used to bind Country List to DropDown by ViewBag</Description>
        /// </summary>
        public void LoadMyStudentList()
        {

            ViewBag.SchoolName = new SelectList(ObjOCRP.Schools.ToList(), "ID", "SchoolName");

        }
        /// <summary>
        /// This method use to select user profile details by user id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserProfile()
        {
            Int32 userId = Utility.Number.Zero;
            if (Session["ID"] != null)
            {
                userId = Convert.ToInt32(Session["ID"]);
            }
            var userProfile = (from c in ObjOCRP.Students
                               where c.UserId == userId
                               select new
                               {
                                   c.FirstName,
                                   c.LastName,
                                   c.FatherName,
                                   c.Address,
                                   c.UserId,
                               }).ToList();

            return new JsonResult { Data = userProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// <Description>This method is used to bind Academicyear List to DropDown by ViewBag</Description>
        /// </summary>
        public JsonResult AcademicList(int Id)
        {

            var academicyear = from s in ObjOCRP.Academic_Sessions
                        where s.ID == Id
                        select s;
            return Json(new SelectList(academicyear.ToArray(), "ID", "Academic_Sessions"), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// This method use to save user profile information.
        /// </summary>
        /// <param name="objUserList"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserProfile(Models.UsersProfile objUserList)
        {

            string returnResult = string.Empty;
            try
            {
                var objUser = ObjOCRP.Students.FirstOrDefault(c => c.UserId == objUserList.UserId);

                if (objUser != null)
                {
                    // Update  Usre profile.
                    objUser.FirstName = objUserList.FirstName;
                    objUser.LastName = objUserList.LastName;
                    objUser.FatherName = objUserList.FatherName;
                    objUser.Address = objUserList.Address;
                  //  objUser.Email = objUserList.Email
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;
                }

            }
            catch (Exception ex)
            {
                returnResult = Utility.Message.RecordUnsaved;
            }
            return Json(new { result = returnResult });
        }
        /// <summary>
        /// This method use to save/update user profile information.
        /// </summary>
        /// <param name="objUserList"></param>
        /// <returns></returns>
        public JsonResult SaveUserProfileAllDetails(Models.UsersProfile objUserList)
        {
            string returnResult = string.Empty;
            Int32 userId = Utility.Number.Zero;
            if (Session["ID"] != null)
            {
                userId = Convert.ToInt32(Session["ID"]);
            }
            try
            {
                var objUserProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == userId));

                if (objUserProfile == null)
                {
                    var objAddUserProfile = new Student();
                    {
                        //save
                        objAddUserProfile.FirstName = objUserList.FirstName;
                        objAddUserProfile.LastName = objUserList.LastName;
                        objAddUserProfile.FatherName = objUserList.FatherName;
                        objAddUserProfile.Contact = objUserList.Contact;
                        objAddUserProfile.AcademicYear = objUserList.AcademicYear;
                        objAddUserProfile.IsApproved = objUserList.IsApproved;
                        objAddUserProfile.Dob = objUserList.DOB;
                        objAddUserProfile.School = objUserList.School;
                        objAddUserProfile.Address = objUserList.Address;
                        objAddUserProfile.Email = objUserList.Email;
                        objAddUserProfile.Gender = objUserList.Gender;
                        objAddUserProfile.CreatedDate = DateTime.Now;
                        objAddUserProfile.IsDeleted = false;
                        objAddUserProfile.UserId = objUserList.UserId;
                        objAddUserProfile.Picture = objUserList.Picture;
                        ObjOCRP.Students.Add(objAddUserProfile);
                        ObjOCRP.SaveChanges();
                        returnResult = Utility.Message.Add_Message;
                    };

                }
                else
                {
                    // Update UserDetails.
                    objUserProfile.AcademicYear = objUserList.AcademicYear;
                    objUserProfile.Dob = objUserList.DOB;
                    objUserProfile.Contact = objUserList.Contact;
                    objUserProfile.Address = objUserList.Address;
                    objUserProfile.IsApproved = objUserList.IsApproved;
                    objUserProfile.Gender = objUserList.Gender;
                    objUserProfile.IsDeleted = false;
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;
                }

            }
            catch (Exception ex)
            {
            }
            returnResult = Utility.Message.RecordUnsaved;
            return Json(new { result = returnResult });


        }
           /// <summary>
        /// This method use to save user profile image.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveProfilePic()
        {
            //    bool isSavedSuccessfully = true;
          // string Title = string.Empty;
            string returnResult = string.Empty;
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                  //  Title = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        Int32 IdUser = Convert.ToInt32(Session["ID"]);
                        Int32 saveStudentCount = 0;
                        var Picture = Path.Combine(Server.MapPath("~/UploadFiles"));
                        string pathString = System.IO.Path.Combine(Picture.ToString());
                        var namepath = Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                        //Save.ImageGallery
                        var objUserProfile = ObjOCRP.Students.FirstOrDefault(c => (c.UserId == IdUser));
                        if (objUserProfile != null)
                            //   UserProfile objISM = new UserProfile();
                          //  objUserProfile.ProfileName = Title;
                        //    objUserProfile.Picture = ConvertToBytes(file);
                        //  objUserProfile.UserProfiles.Add(objISM);
                        ObjOCRP.SaveChanges();
                        saveStudentCount++;
                        // }

                        //file.SaveAs(Imagepath);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        /// <summary>
        /// This method use to convert usre profile image to byte.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            imageBytes = reader.ReadBytes((int)file.ContentLength);
            return imageBytes;
        }
    }
}