using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class ManageMangement
    { 
        public Int32 ID { get; set; }

        //Get userprofile table.
        public string FatherName { get; set; }
        public string DateofBirth { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Photo { get; set; }
        public string Gender { get; set; }
        public int CreatedBy { get; set; }
        public int UserId { get; set; }

        public string ContactNo { get; set; }
    }
}