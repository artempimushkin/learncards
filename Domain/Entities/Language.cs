using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Language
    {
        public Language()
        {
            LanguageCode = "";
            LanguageName = "";
            VoiceId = "";
        }

        [Key]
        public string LanguageCode { get; set; }
        [Required]
        public string LanguageName { get; set; }
        [Required]
        public string VoiceId { get; set; } // Название голоса, озвучивающего слова на данном языке (необходимо для Amazon Polly)
    }
}
