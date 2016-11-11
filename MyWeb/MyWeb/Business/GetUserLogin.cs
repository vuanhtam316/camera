using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models;

namespace MyWeb.Business
{
    public class GetUserLogin
    {
        public static int CurrentUserId
        {
            get
            {
                int userId = 0;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name.Split('|')[1]);
                }

                return userId;
            }
        }
        public static int CurentCustomerId
        {
            get
            {
                int userId = 0;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    var db = new Entities1();
                    var username = HttpContext.Current.User.Identity.Name;
                    var firstOrDefault = db.TBL_USER.FirstOrDefault(x => x.USER_NAME == username);
                    if (firstOrDefault != null)
                        userId = (int)firstOrDefault.USER_ID;
                }

                return userId;
            }
        }

        public string CurrentUserName
        {
            get
            {
                string userName = string.Empty;

                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    userName = HttpContext.Current.User.Identity.Name.Split('|')[0];
                }

                return userName;
            }
        }
    }
}