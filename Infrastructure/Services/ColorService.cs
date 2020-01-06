using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class ColorService : IColorService
    {
        private ApplicationDbContext Database;
        public ColorService(ApplicationDbContext db)
        {
            Database = db;
        }

        public Color GetRandomColor()
        {
            var list = Database.Color.ToList();
            return list[(new Random()).Next(0, list.Count)];
        }
    }
}
