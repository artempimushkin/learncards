using Application.DTOs;
using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class CardService : ICardService
    {
        private ApplicationDbContext Database;

        public CardService(ApplicationDbContext db)
        {
            Database = db;
        }

        public bool AddCard(int deck_id, String front, String back) // Добавление новой карточки в колоду пользователя
        {
            var card = new Card() { DeckId = deck_id, Front = front, Back = back, BoxIndex = 0, LastDate = DateTime.UtcNow - TimeSpan.FromDays(2) };
            Database.Add(card);
            return Database.SaveChanges() == 1;
        }

        private Card GetCard(int id)
        {
            return Database.Card.Where(c => c.CardId == id).FirstOrDefault();
        }

        public bool UpdateCard(int card_id, String front, String back) // Изменение карточки
        {
            var card = GetCard(card_id);
            card.Front = front;
            card.Back = back;
            return Database.SaveChanges() == 1;
        }

        public bool DeleteCard(int id)
        {
            var card = GetCard(id);
            Database.Remove(card);
            return Database.SaveChanges() == 1;
        }

        public CardDto GetNextCard(int deck_id)
        {
            string[] types = { "flip-card", "type-card", "audio-card" };

            var CardList = Database.Card.Where(c => c.DeckId == deck_id).OrderBy(c => c.BoxIndex).ToList(); // получение списка всех карточек
            int index = 0;
            while ((index < CardList.Count) && (!CardList[index].NeedToRepeat())) // поиск первой попавшейся карточки, которую надо повторить
                index++;

            if (index == CardList.Count) // если не найдена ни одна карточка, которую надо повторить
                return null;

            int currenttype = (new Random()).Next(0, 1000) % types.Length; // рандомный выбор типа карточки

            return new CardDto(CardList[index], types[currenttype]);
        }

        public bool KnowledgeLevelIncrement(int id)
        {
            var card = GetCard(id);
            if (card.BoxIndex < 5)
                card.BoxIndex++;
            card.LastDate = DateTime.UtcNow;
            return Database.SaveChanges() == 1;
        }

        public bool KnowledgeLevelSetNull(int id)
        {
            var card = GetCard(id);
            card.BoxIndex = 0;
            card.LastDate = DateTime.UtcNow;
            return Database.SaveChanges() == 1;
        }

        public bool AuthorizeUserCard(string username, int card_id) // проверка принадлежит ли эта карточка пользователю
        {
            return Database.Card.Where(c => c.CardId == card_id && c.Deck.User.Username == username).Any();
        }
    }
}
