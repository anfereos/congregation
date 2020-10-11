using Congregation.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Request
{
    public class MeetingRequest
    {
        [Required]
        public int MeetingId { get; set; }

        [Required]
        public int ChurchId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

        public ICollection<AssistanceResponse> Assistances { get; set; }
    }
}
