using Application.DTOs;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public User GetUser(string username);
        public bool Create(RegisterDto model);
        public bool Update(string username, UserDto model);
        public bool Exist(string username);
        public bool Exist(UserDto model);
    }
}
