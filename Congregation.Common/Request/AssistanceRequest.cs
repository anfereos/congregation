using Congregation.Common.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Request
{
    public class AssistanceRequest
    {
        [Required]
        public int MeetingId { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public ICollection<UserResponse> users { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}
