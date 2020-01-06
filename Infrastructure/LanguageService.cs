using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class LanguageService : ILanguageService
    {
        private ApplicationDbContext Database;
        public LanguageService(ApplicationDbContext db)
        {
            Database = db;
        }

        public Language GetLanguage(string languageCode)
        {
            return Database.Language.Where(l => l.LanguageCode == languageCode).FirstOrDefault();
        }

        public List<Language> GetLanguageList()
        {
            return Database.Language.OrderBy(l => l.LanguageName).ToList();
        }
    }
}
