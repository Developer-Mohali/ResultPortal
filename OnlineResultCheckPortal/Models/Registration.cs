using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class Registration
    {
        public string FirstName { get; set; }

        public int ID { get; set; }
        public string EmailID { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
   
        public Int16 RoleId { get; set; }
        public string ConfirmPassword { get; set; }
    }
}