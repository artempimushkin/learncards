using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Color
    {
        public Color()
        {
            ColorId = 0;
            ColorCode = "";
        }

        [Key]
        public int ColorId { get; set; }
        public string ColorCode { get; set; } // Шестнадцатеричный код цвета
    }
}
