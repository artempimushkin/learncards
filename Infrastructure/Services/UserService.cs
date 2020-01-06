using Application.DTOs;
using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext Database;
        public UserService(ApplicationDbContext db)
        {
            Database = db;
        }

        public User GetUser(string username)
        {
            return Database.User.FirstOrDefault(u => u.Username == username);
        }

        public bool Create(RegisterDto model)
        {
            Database.Add(new User { Username = model.Username, Password = MD5Hash(model.Password) });
            return Database.SaveChanges() == 1;
        }

        public bool Update(string username, UserDto model)
        {
            var user = GetUser(username);
            user.Username = model.Username;
            user.Password = MD5Hash(model.Password);
            return Database.SaveChanges() == 1;
        }

        public bool Exist(string username) // проверка существования юзернейма в БД
        {
            return Database.User.Any(u => u.Username == username);
        }

        public bool Exist(UserDto model) // проверка существования пользователя
        {
            return Database.User.Any(u => u.Username == model.Username && u.Password == MD5Hash(model.Password));
        }

        private string MD5Hash(string input) // Хэширование пароля
        {
            return Encoding.UTF8.GetString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input)));
        }
    }
}
