using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class ContextCardDto : CardDto
    {
        public ContextCardDto(Card card, string sentence, string type)
            : base(card, type)
        {
            Sentence = sentence;
        }
        public string Sentence { get; set; }
    }
}
