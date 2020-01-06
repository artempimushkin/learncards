using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Card
    {
        public Card()
        {
            CardId = 0;
            DeckId = 0;
            Front = "";
            Back = "";
            BoxIndex = 0;
            LastDate = DateTime.UtcNow;
        }

        [Key]
        public int CardId { get; set; }
        [Required]
        public int DeckId { get; set; }

        [ForeignKey("DeckId")]
        public Deck Deck { get; set; }
        [Required]
        public string Front { get; set; }
        [Required]
        public string Back { get; set; }
        [Required]
        public short BoxIndex { get; set; } // Индекс группы карточек
        [Required]
        public DateTime LastDate { get; set; } // Дата последнего повторения карточки

        public bool NeedToRepeat() // вычисляет, пора ли повторить карточку
        {
            var today = DateTime.UtcNow - LastDate;
            if (Math.Pow(2, BoxIndex) <= today.TotalDays)
                return true;
            else
                return false;
        }
    }
}
