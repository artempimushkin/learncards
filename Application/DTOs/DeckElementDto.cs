using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.DTOs
{
    public class DeckElementDto : DeckSettingsDto
    {
        public DeckElementDto() { }

        public DeckElementDto(Deck deck)
            : base(deck)
        {
            DeckId = deck.DeckId;
            ColorCode = deck.Color.ColorCode;
            CardNumber = deck.Cards.Count();

            CardsToRepeat = 0;
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                if (deck.Cards[i].NeedToRepeat())
                {
                    CardsToRepeat++;
                }
            }
        }

        public int DeckId { get; set; }
        public string ColorCode { get; set; }
        public int CardNumber { get; set; }
        public int CardsToRepeat { get; set; }
    }
}
