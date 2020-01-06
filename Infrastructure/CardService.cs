using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class CardService : ICardService
    {
        private ApplicationDbContext Database;
        public CardService(ApplicationDbContext db)
        {
            Database = db;
        }

        public bool AddCard(Card card)
        {
            Database.Add(card);
            return Database.SaveChanges() == 1;
        }

        public Card GetCard(int id)
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
