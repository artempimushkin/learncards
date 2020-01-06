using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Analytics
    {
        public Analytics()
        {
            DeckId = 0;
            KnowledgeLevel = 0;
            RepeatDate = DateTime.UtcNow;
        }
        public int DeckId { get; set; } // Номер колоды. Входит в состав первичного ключа.
        public int KnowledgeLevel { get; set; } // Уровень знания колоды (в процентах)
        public DateTime RepeatDate { get; set; } // Дата повторения. Входит в состав первичного ключа.
    }
}
