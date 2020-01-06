using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class RegisterDto : UserDto
    {
        public RegisterDto() :
            base()
        {
            ConfirmPassword = "";
        }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    }
}
