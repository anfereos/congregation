using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
