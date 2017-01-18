using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class UsersProfile
    {

        public Int32 ID { get; set; }

        //Get userprofile table.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string DOB { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Picture { get; set; }
        public string AcademicYear { get; set; }
        public int School { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public int UserId { get; set; }
    }
}