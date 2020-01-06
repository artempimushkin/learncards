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
    public class DeckService : IDeckService
    {
        private ApplicationDbContext Database;
        private IUserService _userService;
        private IColorService _colorService;
        public DeckService(ApplicationDbContext db, IUserService userService, IColorService colorService)
        {
            Database = db;
            _userService = userService;
            _colorService = colorService;
        }

        public bool CreateDeck(string username, string DeckName, string LanguageCode, bool IsPublic)
        {
            var deck = new Deck
            {
                UserId = _userService.GetUser(username).UserId,
                DeckName = DeckName,
                LanguageCode = LanguageCode,
                IsPublic = IsPublic,
                ColorId = _colorService.GetRandomColor().ColorId
            };

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
            var decklist = GetDeckList(username);

            List<DeckElementDto> decks = new List<DeckElementDto>();
            foreach (var d in decklist)
            {
                decks.Add(new DeckElementDto(d));
            }

            return decks;
        }

        public List<DeckElementDto> GetPublicDecksWithCards(string username)
        {
            var decks = GetDeckElementList(username);
            return (from d in decks where d.IsPublic && d.CardNumber > 0 select d).ToList();
        }

        private Deck GetDeck(int id)
        {
            return Database.Deck.Where(d => d.DeckId == id).FirstOrDefault();
        }

        public Deck GetDeckWithCards(int id)
        {
            return Database.Deck.Include(d => d.Cards).Where(d => d.DeckId == id).FirstOrDefault();
        }

        public DeckSettingsDto GetDeckSettings(int id)
        {
            return new DeckSettingsDto(GetDeck(id));
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
            var deck = GetDeckWithCards(deck_id); // получение колоды, которую надо скопировать
            if (deck.IsPublic) // чужие колоды можно копировать только если они публичные
            {
                var user_id = _userService.GetUser(username).UserId; // получаем Id юзера
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
                return Database.SaveChanges() > 0;
            }
            else
                return false;
        }

        public bool AuthorizeUserDeck(string username, int deck_id) // проверка принадлежит ли эта колода пользователю
        {
            return Database.Deck.Where(d => d.DeckId == deck_id && d.User.Username == username).Any();
        }
    }
}
