using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class JSCEDetails
    {

      public Int16 ID { get; set; }
      public string RegistrationNumber { get; set; }
      public string Description { get; set; }
      public string FileName { get; set; }
      public string ReportCardFile { get; set; }
      public string  StudentID { get; set; }
      public int JSCEID { get; set; }
      public string SubjectName { get; set; }
      public string Grade { get; set; }
      public string Remarks { get; set; }
      public int CreatedBy { get; set; }
      public int UpdateBy { get; set; }
      public int UpdateID { get; set; }
    }
}