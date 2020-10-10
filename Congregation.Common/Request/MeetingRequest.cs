using Congregation.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Request
{
    public class MeetingRequest
    {
        public MeetingResponse Meeting { get; set; }
        public ChurchResponse Church { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }

        public ICollection<AssistanceResponse> Assistances { get; set; }
    }
}
