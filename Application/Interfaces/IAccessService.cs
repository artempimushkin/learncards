using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{    
    public interface IAccessService
    {
        public bool GiveAccessRights(string from_user, string to_user);
        public bool DeleteAccessRights(string from_user, string to_user);
        public List<string> GivenAccessRightsList(string username);
        public List<string> TakenAccessRightsList(string username);
        public bool AccessExists(string from_user, string to_user);
    }
}
