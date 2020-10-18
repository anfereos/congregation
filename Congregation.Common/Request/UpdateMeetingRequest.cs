using Congregation.Common.Responses;
using System;
using System.Collections.Generic;

namespace Congregation.Common.Request
{
    public class UpdateMeetingRequest
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public ChurchResponse Church { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateLocal { get; set; }
        public List<AssistanceResponse> Assistances { get; set; }
    }
}
