using System.ComponentModel.DataAnnotations;

namespace Congregation.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
