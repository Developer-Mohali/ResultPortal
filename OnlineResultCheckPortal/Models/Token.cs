using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineResultCheckPortal.Models
{
    public class Token
    {
        public int ID { get; set; }
        public string TokenName { get; set; }
        public string TokenPrice { get; set; }
        public int NumberOfTimeUse { get; set; }
        public string TokenDescription { get; set; }
        public string TokenPicture { get; set; }
    }
}