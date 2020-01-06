using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class DeckSettingsDto
    {
        public DeckSettingsDto(Deck deck)
        {
            DeckName = deck.DeckName;
            LanguageCode = deck.LanguageCode;
            IsPublic = deck.IsPublic;
        }

        public string DeckName { get; set; }
        public string LanguageCode { get; set; }
        public bool IsPublic { get; set; }
    }
}
