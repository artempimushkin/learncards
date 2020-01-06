using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{    
    public interface ICardService
    {
        public bool AddCard(int deck_id, String front, String back);
        public bool UpdateCard(int card_id, String front, String back);
        public bool DeleteCard(int id);
        public CardDto GetNextCard(int deck_id);
        public bool KnowledgeLevelIncrement(int id);
        public bool KnowledgeLevelSetNull(int id);
        public bool AuthorizeUserCard(string username, int card_id);
    }
}
