using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Request
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
