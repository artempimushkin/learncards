using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAnalyticsService
    {
        public List<DeckAnalyticsDto> GetAnalytics(string username);
        public bool SaveAnalytics(int deck_id);
        public bool AuthorizeUserAnalytics(string from_user, string to_user);
    }
}
