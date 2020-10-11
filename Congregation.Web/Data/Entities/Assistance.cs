﻿using System.ComponentModel.DataAnnotations;

namespace Congregation.Web.Data.Entities
{
    public class Assistance
    {
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Meeting Meeting { get; set; }

        [Display(Name = "Is Present")]
        public bool IsPresent { get; set; }
    }
}
