using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;

using ImageResizer.Util;
using MyWeb.Models;

namespace MyWeb.Business
{
    public class CustomRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            if (roleName.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }
            if (RoleExists(roleName))
            {
                throw new ProviderException("Role name already exists.");
            }
            try
            {
                var db = new Entities1();
                var role = new TBL_ROLE { ROLE_NAME = roleName, ROLE_DESCRIPTION = null };
                db.TBL_ROLE.Add(role);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ProviderException("Role name already exists.", ex);
            }
        }

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }
            try
            {
                var db = new Entities1();
                var role = db.TBL_ROLE.FirstOrDefault(x => x.ROLE_NAME == rolename);
                if (role != null)
                {
                    try
                    {
                        db.TBL_ROLE.Remove(role);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ProviderException("Role name already exists.", ex);
            }
            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var db = new Entities1();
            var user = db.TBL_USER.SingleOrDefault(u => u.USER_NAME == username);
            if (user == null)
                return new string[] { };
            return user.TBL_USER_ROLE == null ? new string[] { } :
                user.TBL_USER_ROLE.Select(u => u.TBL_ROLE).Select(u => u.ROLE_NAME).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var db = new Entities1();
            return db.TBL_USER_ROLE.Where(x => x.ROLE_NAME == roleName).Select(x => x.USER_NAME).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var db = new Entities1();
            var user = db.TBL_USER.SingleOrDefault(u => u.USER_NAME == username);
            if (user == null)
                return false;
            return user.TBL_USER_ROLE != null && user.TBL_USER_ROLE.Select(
                 u => u.TBL_ROLE).Any(r => r.ROLE_NAME == roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            var db = new Entities1();
            var checkRole = db.TBL_ROLE.Count(x => x.ROLE_NAME == roleName);
            return checkRole > 0;
        }
    }
}