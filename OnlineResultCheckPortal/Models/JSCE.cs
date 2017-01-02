using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class JSCE
    {
      public Int32 ID { get; set; }
      public int StudentID { get; set; }
      public string JSCERegNumber { get; set; }
      public int CreatedBy { get; set; }
    }
}