using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Responses
{
    public class ProfessionResponse
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
