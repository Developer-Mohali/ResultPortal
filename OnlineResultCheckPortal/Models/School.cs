using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class School
    {
        public int ID { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string Zone { get; set; }
    }
}