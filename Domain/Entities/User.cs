using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        public User()
        {
            UserId = 0;
            Password = "";
            Username = "";
            Decks = new List<Deck>();
        }
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        public List<Deck> Decks { get; set; }
    }
}
