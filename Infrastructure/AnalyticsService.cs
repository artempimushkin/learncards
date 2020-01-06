using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class AnalyticsService : IAnalyticsService
    {
        private ApplicationDbContext Database;
        public AnalyticsService(ApplicationDbContext db)
        {
            Database = db;
        }

        public List<Deck> GetAnalytics(string username)
        {
            return Database.Deck.Include(d => d.Analytics).Where(d => d.User.Username == username).ToList();
        }

        public void SaveAnalytics(int deck_id) // Сохранение аналитики после повторения карточек колоды
        {
            var deck = Database.Deck.Include(d => d.Cards).Where(d => d.DeckId == deck_id).FirstOrDefault();

            int knowledge = 0;
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                knowledge += deck.Cards[i].BoxIndex;
            }
            knowledge = Convert.ToInt32(Math.Floor(knowledge * 100.0 / (5 * deck.Cards.Count)));

            Database.Add(new Analytics { DeckId = deck_id, KnowledgeLevel = knowledge, RepeatDate = DateTime.UtcNow });
            Database.SaveChanges();
        }

        public bool AuthorizeUserAnalytics(string from_user, string to_user) // Проверка наличия прав доступа к аналитике другого пользователя
        {
            if (from_user == to_user) return true;
            return Database.AccessRights.Where(st => st.From.Username == from_user && st.To.Username == to_user).Any();
        }
    }
}
