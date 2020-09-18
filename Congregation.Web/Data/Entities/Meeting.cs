using Congregation.Common.Entities;
using System;
using System.Collections.Generic;

namespace Congregation.Web.Data.Entities
{
    public class Meeting
    {
        public int Id { get; set; }

        public Church Church { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Assistance> Assistances { get; set; }
    }
}
