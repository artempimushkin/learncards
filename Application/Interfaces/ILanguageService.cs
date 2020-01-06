using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ILanguageService
    {
        public Language GetLanguage(string languageCode);
        public List<Language> GetLanguageList();
    }
}
