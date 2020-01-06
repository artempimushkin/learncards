using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Deck
    {
        public Deck()
        {
            DeckId = 0;
            UserId = 0;
            DeckName = "";
            LanguageCode = "";
            IsPublic = false;
            Cards = new List<Card>();
            Analytics = new List<Analytics>();
        }

        [Key]
        public int DeckId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        public string DeckName { get; set; }
        public string LanguageCode { get; set; }

        [ForeignKey("LanguageCode")]
        public Language Language { get; set; }
        public int ColorId { get; set; }

        [ForeignKey("ColorId")]
        public Color Color { get; set; }
        public bool IsPublic { get; set; }
        public List<Card> Cards { get; set; }
        public List<Analytics> Analytics { get; set; } // Список точек графика аналитики обучения колоды
    }
}
