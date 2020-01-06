using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class UserDto
    {
        public UserDto()
        {
            Password = "";
            Username = "";
        }

        [RegularExpression(@"[a-z0-9]+", ErrorMessage = "Имя пользователя может содержать только латинские символы и цифры")]
        [Required(ErrorMessage = "Не указано имя пользователя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 и не более 50 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
