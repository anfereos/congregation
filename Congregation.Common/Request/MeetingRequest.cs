using System;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Request
{
    public class MeetingRequest
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

    }
}
