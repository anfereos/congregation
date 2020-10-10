using Congregation.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Congregation.Common.Request
{
    public class AssistanceRequest
    {
        [Required]
        public MeetingResponse Meeting { get; set; }

        [Required]
        public UserResponse user { get; set; }//traigo la lista de usuarios

        [Required]
        public bool IsPresent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime Date { get; set; }


    }
}
