using WebLT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLT.Security
{
    public static class SessionPersister
    {
        static string usernameSessionvar = "username";
        public static TAIKHOAN UserName
        {
            get{
                if (HttpContext.Current == null)
                {
                    return null;
                }
                var sessionVar = HttpContext.Current.Session[usernameSessionvar];
                if (sessionVar != null)
                {
                    return sessionVar as TAIKHOAN;
                }
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionvar] = value;
            }
        }
    }
}