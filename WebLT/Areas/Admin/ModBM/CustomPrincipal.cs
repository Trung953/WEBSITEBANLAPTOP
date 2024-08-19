using WebLT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebLT.ModBM
{
    public class CustomPrincipal:IPrincipal
    {
        private TAIKHOAN Account;
        public CustomPrincipal(TAIKHOAN account)
        {
            this.Account = account;
            this.Identity = new GenericIdentity(account.username);
        }

        public IIdentity Identity
        {
            get;
            set;
        }

        public bool IsInRole(string role)
        {
            var roles = role.Split(new char[] { ',' });

            // Check if any role matches the user's roles
            foreach (var r in roles)
            {
                if (this.Account.PHANQUYENs.Any(p => p.TENQUYEN == r))
                {
                    return true;
                }
            }

            return false;
        }

    }
}