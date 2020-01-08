using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    /// <summary>
    /// The analytics service controls viewing and saving analytics
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary>
        /// Gets a list of user decks with analytics for each
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<DeckAnalyticsDto> GetAnalytics(string username);

        /// <summary>
        /// Saves the last deck state including box indexes of each card
        /// </summary>
        /// <param name="deck_id"></param>
        /// <returns>True - if analytics has been successfully saved</returns>
        public bool SaveAnalytics(int deck_id);

        /// <summary>
        /// Checks if one user can view analytics of another user
        /// </summary>
        /// <param name="from_user">The user who is trying to view analytics</param>
        /// <param name="to_user">The user whose analytics the other user is trying to view</param>
        /// <returns>True - if from_user can view analytics of to_user</returns>
        public bool AuthorizeUserAnalytics(string from_user, string to_user);
    }
}
