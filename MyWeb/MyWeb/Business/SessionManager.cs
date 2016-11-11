using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Business
{
    public class SessionManager
    {
        public const string UserInfo = "UserInfo";
        public const string ListPermisstion = "LIST_PERMISSTION";

        public static void SetValue(string Key, object Value)
        {
            HttpContext context = HttpContext.Current;
            context.Session[Key] = Value;
        }

        public static object GetValue(string Key)
        {
            HttpContext context = HttpContext.Current;
            return context.Session[Key];
        }

        public static void Remove(string Key)
        {
            HttpContext context = HttpContext.Current;
            context.Session.Remove(Key);
        }

        public static void Clear()
        {
            HttpContext context = HttpContext.Current;
            context.Session.RemoveAll();
        }

        public static bool HasValue(string Key)
        {
            HttpContext context = HttpContext.Current;
            return context.Session[Key] != null;
        }

        public static object GetUserInfo()
        {
            HttpContext context = HttpContext.Current;
            return context.Session[UserInfo];
        }

        public static object GetListPermistion()
        {
            HttpContext context = HttpContext.Current;
            return context.Session[ListPermisstion];
        }
    }
}