using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class JSCEResult
    {
        public string ExamTypes { get; set; }
        public Int32 ID { get; set; }
      public int JSCEID { get; set; }
        public string Registration { get; set; }
      public string TestName { get; set; }
      public string Marks { get; set; }
      public string Result { get; set; }
      public string Description { get; set; }
      public string Session { get; set; }
      public string FileName { get; set; }
      public bool IsDeleted { get; set; }
      public int CreatedBy { get; set; }
      public int UpdateBy { get; set; }
      public string TokenNumber { get; set; }
      public int SchoolID { get; set; }
    }
}





