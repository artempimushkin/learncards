using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    /// <summary>
    /// The access rights service controls editing and using access rights
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Gives one user rights to access analytics of another user
        /// </summary>
        /// <param name="from_user">user that gives rights to another user to see his analytics</param>
        /// <param name="to_user">user that revieves rights from another user to view his analytics</param>
        /// <returns>True - if rights have been successfully added to database</returns>
        public bool GiveAccessRights(string from_user, string to_user);

        /// <summary>
        /// Deletes rights to access analytics of another user
        /// </summary>
        /// <param name="from_user">user that gives rights to another user to see his analytics</param>
        /// <param name="to_user">user that recieves rights from another user to view his analytics</param>
        /// <returns>True - if rights have been successfully deleted from database</returns>
        public bool DeleteAccessRights(string from_user, string to_user);

        /// <summary>
        /// Gets a list of users which have access rights to see analytics of the current user
        /// </summary>
        /// <param name="username">the current user</param>
        /// <returns>List of usernames</returns>
        public List<string> GivenAccessRightsList(string username);

        /// <summary>
        /// Gets a list of users which have given access rights to the current user
        /// </summary>
        /// <param name="username">the current user</param>
        /// <returns>List of usernames</returns>
        public List<string> TakenAccessRightsList(string username);

        /// <summary>
        /// Checks if access rights from one user to another exist
        /// </summary>
        /// <param name="from_user">The user who gave access rights</param>
        /// <param name="to_user">The user who have access rights</param>
        /// <returns>True - if access rights from_user -> to_user exist</returns>
        public bool AccessExists(string from_user, string to_user);
    }
}
