using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class AccessService : IAccessService
    {
        private ApplicationDbContext Database;
        private IUserService userService;

        public AccessService(ApplicationDbContext db, IUserService userService)
        {
            Database = db;
            this.userService = userService;
        }

        public bool GiveAccessRights(string from_user, string to_user)
        {
            int from_id = userService.GetUser(from_user).UserId;
            int to_id = userService.GetUser(to_user).UserId;

            Database.Add(new AccessRights { FromId = from_id, ToId = to_id });
            return Database.SaveChanges() == 1;
        }

        public bool DeleteAccessRights(string from_user, string to_user)
        {
            var rights = Database.AccessRights.Where(st => st.From.Username == from_user && st.To.Username == to_user).FirstOrDefault();
            Database.Remove(rights);
            return Database.SaveChanges() == 1;
        }

        public List<AccessRights> GivenAccessRightsList(string username)
        {
            return Database.AccessRights.Where(st => st.From.Username == username).ToList();
        }

        public List<AccessRights> TakenAccessRightsList(string username)
        {
            return Database.AccessRights.Where(st => st.To.Username == username).ToList();
        }

        public bool AccessExists(string from_user, string to_user)
        {
            return Database.AccessRights.Where(st => st.From.Username == from_user && st.To.Username == to_user).Any();
        }
    }
}
