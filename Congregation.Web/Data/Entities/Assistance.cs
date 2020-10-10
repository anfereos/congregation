﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Web.Data.Entities
{
    public class Assistance
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Meeting Meeting { get; set; }

        public bool IsPresent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

    }
}
