using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.DTOs
{
    public class DeckAnalyticsDto
    {
        public DeckAnalyticsDto(Deck deck)
        {
            DeckName = deck.DeckName;

            Labels = new List<string>();
            Data = new List<int>();
            var analytica = deck.Analytics.OrderBy(a => a.RepeatDate).ToList(); // упорядочить по возрастанию даты
            for (int i = 0; i < analytica.Count; i++)
            {
                string month = "";
                if (analytica[i].RepeatDate.Month < 10)
                {
                    month = "0";
                }

                Labels.Add(analytica[i].RepeatDate.Day + "." + month + analytica[i].RepeatDate.Month + "." + (analytica[i].RepeatDate.Year % 100));
                Data.Add(analytica[i].KnowledgeLevel);
            }
        }
        public string DeckName { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}
