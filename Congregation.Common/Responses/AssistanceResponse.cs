using System;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Responses
{
    public class AssistanceResponse
    {
        public int Id { get; set; }

        public UserResponse User { get; set; }

        public MeetingResponse Meeting { get; set; }

        public bool IsPresent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

    }
}
