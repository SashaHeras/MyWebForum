﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MyWebForum.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Surname")]
        public string? Surname { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        public string? Created { get; set; }

        public byte[] Picture { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsVerified { get; set; }
    }
}
