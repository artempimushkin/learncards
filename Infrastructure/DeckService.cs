using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class DeckService : IDeckService
    {
        private ApplicationDbContext Database;
        private IUserService userService;
        public DeckService(ApplicationDbContext db, IUserService userService)
        {
            Database = db;
        }

        public bool CreateDeck(string username, Deck deck)
        {
            deck.UserId = userService.GetUser(username).UserId;
            Database.Add(deck);
            return Database.SaveChanges() == 1;
        }

        public bool DeleteDeck(int id)
        {
            var deck = GetDeck(id);
            Database.Remove(deck);
            return Database.SaveChanges() == 1;
        }

        public List<Deck> GetDeckList(string username)
        {
            return Database.Deck.Include(c => c.Color).Include(d => d.Cards).Where(d => d.User.Username == username).ToList();
        }

        public List<DeckElementDto> GetDeckElementList(string username)
        {
            var decklist = Database.Deck.Include(c => c.Color).Include(d => d.Cards).Where(d => d.User.Username == username).ToList();

            List<DeckElementDto> decks = new List<DeckElementDto>();
            foreach (var d in decklist)
            {
                decks.Add(new DeckElementDto(d));
            }

            return decks;
        }

        public Deck GetDeck(int id)
        {
            return Database.Deck.Where(d => d.DeckId == id).FirstOrDefault();
        }

        public bool UpdateDeck(int DeckId, string DeckName, string LanguageCode, bool IsPublic)
        {
            var deck = GetDeck(DeckId);

            deck.DeckName = DeckName;
            deck.LanguageCode = LanguageCode;
            deck.IsPublic = IsPublic;

            return Database.SaveChanges() == 1;
        }

        public bool CopyDeck(string username, int deck_id) // Копирование публичной колоды из общедоступного профиля пользователя
        {
            var user_id = userService.GetUser(username).UserId; // получаем Id юзера
            var deck = GetDeck(deck_id); // получение колоды, которую надо скопировать
            var new_deck = new Deck()
            {
                UserId = user_id,
                DeckName = deck.DeckName,
                LanguageCode = deck.LanguageCode,
                IsPublic = false,
                ColorId = deck.ColorId,
                Cards = new List<Card>()
            };

            for (int i = 0; i < deck.Cards.Count; i++)
            {
                var card = deck.Cards[i];
                new_deck.Cards.Add(new Card() { DeckId = new_deck.DeckId, Front = card.Front, Back = card.Back, BoxIndex = 0, LastDate = DateTime.UtcNow - TimeSpan.FromDays(2) });
            }

            Database.Add(new_deck);
            return Database.SaveChanges() == 1;
        }

        public bool AuthorizeUserDeck(string username, int deck_id) // проверка принадлежит ли эта колода пользователю
        {
            return Database.Deck.Where(d => d.DeckId == deck_id && d.User.Username == username).Any();
        }
    }
}
