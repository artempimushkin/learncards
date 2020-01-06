using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class CardDto
    {
        public CardDto(Card card, string type)
        {
            CardId = card.CardId;
            Front = card.Front;
            Back = card.Back;
            Type = type;
        }
        public int CardId { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public string Type { get; set; }
    }
}
