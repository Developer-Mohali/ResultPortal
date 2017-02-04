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
            try
            {
                var objToken = ObjOCRP.Tokens.FirstOrDefault(c => (c.ID == objtoken.ID && c.TokenID == objtoken.TokenID));
                if (objToken == null)
                {
                    if (ValidToken(objtoken.TokenID) == true)
                    {
                        objToken = new Token();
                        objToken.TokenID = Convert.ToInt16(objtoken.TokenID).ToString();
                        objToken.TokenName = objtoken.TokenName;
                        objToken.TokenPrice = objtoken.TokenPrice;
                        objToken.NumberOfTimeUse = Convert.ToInt16(objtoken.NumberOfTimeUse);
                        objToken.TokenDescription = objtoken.TokenDescription;
                        objToken.CreatedBy = createdBy;
                        objToken.CreatedDate = System.DateTime.Now;
                        objToken.IsDeleted = false;
                        ObjOCRP.Tokens.Add(objToken);
                        ObjOCRP.SaveChanges();
                        returnResult = Utility.Message.Add_Message;
                    }
                    else
                    {
                        returnResult = "Already Exists";
                    }
                }
                else

                {
                    objToken.TokenName = objtoken.TokenName;
                    objToken.TokenPrice = objtoken.TokenPrice;
                    objToken.NumberOfTimeUse = Convert.ToInt16(objtoken.NumberOfTimeUse);
                    objToken.TokenDescription = objtoken.TokenDescription;
                    objToken.UpdatedBy = createdBy;
                    objToken.UpdatedDate = System.DateTime.Now;
                    ObjOCRP.SaveChanges();
                    returnResult = Utility.Message.Update_Message;

                }
            }
            catch (Exception ex)
            {
                returnResult = "Server Error";
            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        private bool ValidToken(string TokenId)
        {
            string returnResult = string.Empty;
            bool flag = false;
            var objToken = ObjOCRP.Tokens.FirstOrDefault(c => (c.TokenID == TokenId));
            if (objToken == null)
            {
                flag =true;

            }
            else
            {
                flag = false;
            
            }
            return flag;
        }

        /// <summary>
        /// This Method Is To Be The Display The List.
        /// </summary>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult TokenList() 
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
                    var objTokenList = ObjOCRP.GetTokenList().ToList();

                    //Sorting
                    totalRecords = objTokenList.Count();
                    var data = objTokenList.Skip(skip).Take(pageSize).ToList();
                    return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);

                }
            }
            catch(Exception ex)
            {

            }
            return new JsonResult { Data = returnResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //var objTokenList = ObjOCRP.GetTokenList().ToList();
            //return new JsonResult { Data = objTokenList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                objDeleteTokenDetails.CreatedBy = createdBy;
                ObjOCRP.SaveChanges();
                returnResult = Utility.Message.Delete_Message;
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

            string School = string.Empty;
            // 
            Models.Token objToken = new Models.Token();

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
                                        string TokenID = (ds.Tables[0].Rows[i].ItemArray[0]).ToString();
                                        objToken.TokenName = (ds.Tables[0].Rows[i].ItemArray[1]).ToString();
                                        objToken.TokenPrice = (ds.Tables[0].Rows[i].ItemArray[2].ToString());
                                        string numberToken  = (ds.Tables[0].Rows[i].ItemArray[3].ToString());
                                        objToken.TokenDescription = (ds.Tables[0].Rows[i].ItemArray[4].ToString());
                                        objToken.NumberOfTimeUse = Convert.ToInt32(numberToken);

                                            var objTokens = ObjOCRP.Tokens.FirstOrDefault(c => (c.TokenID == TokenID));
                                            if (objTokens == null)
                                           {

                                                Addcount = Addcount + 1;
                                            //This is save a data in Token table.
                                                objTokens = new Token();
                                                objTokens.TokenID = Convert.ToInt16(TokenID).ToString();
                                                objTokens.TokenName = objToken.TokenName;
                                                objTokens.TokenDescription = objToken.TokenDescription;
                                                objTokens.NumberOfTimeUse = objToken.NumberOfTimeUse;
                                                objTokens.TokenPrice = objToken.TokenPrice;
                                                objTokens.CreatedBy = createdBy;
                                                objTokens.CreatedDate = DateTime.Now;
                                                objTokens.IsDeleted = false;
                                                ObjOCRP.Tokens.Add(objTokens);
                                                ObjOCRP.SaveChanges();
                                            }
                                            else
                                             {
                                            update = update + 1;
                                            objTokens.TokenName = objToken.TokenName;
                                            objTokens.TokenDescription = objToken.TokenDescription;
                                            objTokens.NumberOfTimeUse = objToken.NumberOfTimeUse;
                                            objTokens.TokenPrice = objToken.TokenPrice;
                                            objTokens.UpdatedDate =DateTime.Now;
                                            objTokens.UpdatedBy = createdBy;
                                            ObjOCRP.SaveChanges();
                                        }

                                        returnResult = "<br/><font color=white><b>Add new record total: " + Addcount + "</br></br>Update record total: " + update + "</br></b></font><br/>";//edit it    
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
    }
}
