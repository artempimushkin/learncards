using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class AccessService : IAccessService
    {
        private ApplicationDbContext Database;
        private IUserService _userService;

        public AccessService(ApplicationDbContext db, IUserService userService)
        {
            Database = db;
            _userService = userService;
        }

        public bool GiveAccessRights(string from_user, string to_user)
        {
            int from_id = _userService.GetUser(from_user).UserId;
            int to_id = _userService.GetUser(to_user).UserId;

            Database.Add(new AccessRights { FromId = from_id, ToId = to_id });
            return Database.SaveChanges() == 1;
        }

        public bool DeleteAccessRights(string from_user, string to_user)
        {
            var rights = Database.AccessRights.Where(st => st.From.Username == from_user && st.To.Username == to_user).FirstOrDefault();
            Database.Remove(rights);
            return Database.SaveChanges() == 1;
        }

        public List<string> GivenAccessRightsList(string username)
        {
            var teacherslist = Database.AccessRights.Include(st => st.To).Where(st => st.From.Username == username).ToList();

            List<string> list = new List<string>();
            for (int i = 0; i < teacherslist.Count; i++)
            {
                list.Add(teacherslist[i].To.Username);
            }

            return list;
        }

        public List<string> TakenAccessRightsList(string username)
        {
            var studentslist = Database.AccessRights.Include(st => st.From).Where(st => st.To.Username == username).ToList();

            List<string> list = new List<string>();
            for (int i = 0; i < studentslist.Count; i++)
            {
                list.Add(studentslist[i].From.Username);
            }

            return list;
        }

        public bool AccessExists(string from_user, string to_user)
        {
            return Database.AccessRights.Where(st => st.From.Username == from_user && st.To.Username == to_user).Any();
        }
    }
}
