using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class DeckIdNameDto
    {
        public DeckIdNameDto(Deck deck)
        {
            DeckId = deck.DeckId;
            DeckName = deck.DeckName;
        }

        public int DeckId { get; set; }
        public string DeckName { get; set; }
    }
}
