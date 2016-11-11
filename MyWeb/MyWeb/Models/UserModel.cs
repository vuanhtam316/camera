using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class UserModel
    {
        public long USER_ID { get; set; }
        public string NAME { get; set; }
        public string DISPLAY_NAME { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORDS { get; set; }
        public string EMAIL { get; set; }
        public string CREATE_BY { get; set; }
        public int USER_STATUS { get; set; }
        public short USER_ACTIVE { get; set; }
        public int AGENCY_ID { get; set; }
        public string TAX_CODE { get; set; }
        public int PHONE { get; set; }
        public string REPRESENTATIVE { get; set; }
        public string ADDRESS { get; set; }
        public string IMAGE_URL { get; set; }
        public string CONFIGPASSWORDS { get; set; }
        public int REVIEW { get; set; }
    }
    public class GetUser
    {
        public IEnumerable<UserModel> Getuser { get; set; }
    }
}