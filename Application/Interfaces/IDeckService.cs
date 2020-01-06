using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IDeckService
    {
        public bool CreateDeck(string username, string DeckName, string LanguageCode, bool IsPublic);
        public bool DeleteDeck(int id);
        public List<Deck> GetDeckList(string username);
        public List<DeckElementDto> GetDeckElementList(string username);
        public List<DeckElementDto> GetPublicDecksWithCards(string username);
        public Deck GetDeckWithCards(int id);
        public DeckSettingsDto GetDeckSettings(int id);
        public bool UpdateDeck(int DeckId, string DeckName, string LanguageCode, bool IsPublic);
        public bool CopyDeck(string username, int deck_id);
        public bool AuthorizeUserDeck(string username, int deck_id);
    }
}
