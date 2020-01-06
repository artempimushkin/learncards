using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private ApplicationDbContext Database;
        private IDeckService _deckService;
        private IAccessService _accessService;

        public AnalyticsService(ApplicationDbContext db, IDeckService deckService, IAccessService accessService)
        {
            Database = db;
            _deckService = deckService;
            _accessService = accessService;
        }

        public List<DeckAnalyticsDto> GetAnalytics(string username)
        {
            var analytics_list = Database.Deck.Include(d => d.Analytics).Where(d => d.User.Username == username).ToList();

            List<DeckAnalyticsDto> decks = new List<DeckAnalyticsDto>();
            foreach (var d in analytics_list)
            {
                decks.Add(new DeckAnalyticsDto(d));
            }
            return decks;
        }

        public bool SaveAnalytics(int deck_id) // Сохранение аналитики после повторения карточек колоды
        {
            var deck = _deckService.GetDeckWithCards(deck_id);

            int knowledge = 0;
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                knowledge += deck.Cards[i].BoxIndex;
            }
            knowledge = Convert.ToInt32(Math.Floor(knowledge * 100.0 / (5 * deck.Cards.Count)));

            Database.Add(new Analytics { DeckId = deck_id, KnowledgeLevel = knowledge, RepeatDate = DateTime.UtcNow });
            return Database.SaveChanges() == 1;
        }

        public bool AuthorizeUserAnalytics(string from_user, string to_user) // Проверка наличия прав доступа к аналитике другого пользователя
        {
            if (from_user == to_user) return true;
            return _accessService.AccessExists(from_user, to_user);
        }
    }
}
