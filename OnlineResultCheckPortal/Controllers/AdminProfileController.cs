using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineResultCheckPortal.Models;

namespace OnlineResultCheckPortal.Controllers
{
    public class AdminProfileController : Controller
    {
        //
        // GET: /AdminProfile/
        OnlineResultCheckPortal ObjOCRP = new OnlineResultCheckPortal();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This method use to get display Admin profile by userId.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult GetAdminProfileDetails()
        {
            Int32 createdBy = Utility.Number.Zero;
            if (Session["UserId"]!= null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
            var objAdminProfileDetails = ObjOCRP.AdminManageProfile(createdBy).ToList();
            return new JsonResult { Data = objAdminProfileDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        /// <summary>
        /// This method use to Update user profile by user id.
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateAdminProfile(Models.ManageMangement objAdminProfiles)
        {
            string returnResult = string.Empty;

            var ObjManageProfile = ObjOCRP.ManageManagements.FirstOrDefault(c => (c.UserId == objAdminProfiles.UserId));
            if (ObjManageProfile == null)
            {
                ObjManageProfile = new ManageManagement();
                ObjManageProfile.FatherName = objAdminProfiles.FatherName;
                ObjManageProfile.DateofBirth = objAdminProfiles.DateofBirth;
                ObjManageProfile.ContactNo = objAdminProfiles.ContactNo;
                ObjManageProfile.Address = objAdminProfiles.Address;
                ObjManageProfile.Gender = objAdminProfiles.Gender;
                ObjManageProfile.UserId = objAdminProfiles.UserId;
                ObjManageProfile.Photo = objAdminProfiles.Photo;
                ObjOCRP.ManageManagements.Add(ObjManageProfile);
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Update_Message;

            }
            else
            {
                ObjManageProfile.FatherName = objAdminProfiles.FatherName;
                ObjManageProfile.DateofBirth = objAdminProfiles.DateofBirth;
                ObjManageProfile.ContactNo = objAdminProfiles.ContactNo;
                ObjManageProfile.Address = objAdminProfiles.Address;
                ObjManageProfile.Gender = objAdminProfiles.Gender;
                ObjManageProfile.Photo = objAdminProfiles.Photo;
                ObjOCRP.SaveChanges();
                returnResult = Models.Utility.Message.Update_Message;

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditAdminProfile(Int32 userID = Models.Utility.Number.Zero)
        {
            if (userID != null)
            {
                var ObjEditAdminProfile = ObjOCRP.AdminManageProfile(userID).ToList();
                return new JsonResult { Data = ObjEditAdminProfile, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View();
        }

        [HttpPost]
        public JsonResult SaveAdminProfileImage()
        {
            Int32 createdBy = Models.Utility.Number.Zero;
            //Getting user id by session.
            if (Session["UserId"] != null)
            {
                createdBy = Convert.ToInt32(Session["UserId"]);
            }
              string picturename = string.Empty;
              string returnResult = string.Empty;
               try
               {
                // save file
                    foreach (string fileName in Request.Files)
                {
                    //get Auto Generate number.
                    string imageNameNew = Models.Utility.NewNumber();
                    HttpPostedFileBase file = Request.Files[fileName];
                    picturename = imageNameNew + ' ' + file.FileName;
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
                            var objAdminProfile = ObjOCRP.ManageManagements.FirstOrDefault(c => c.UserId == createdBy);
                            if (objAdminProfile == null)
                            {
                                objAdminProfile = new ManageManagement();
                                objAdminProfile.Photo = picturename;
                                objAdminProfile.UserId = createdBy;
                                ObjOCRP.ManageManagements.Add(objAdminProfile);
                                ObjOCRP.SaveChanges();
                                returnResult = Models.Utility.Message.Add_Message;
                            }
                            else
                            {
                                objAdminProfile.Photo = picturename;
                                ObjOCRP.SaveChanges();

                            }
                        }
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
