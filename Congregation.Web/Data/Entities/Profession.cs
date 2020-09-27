using System.ComponentModel.DataAnnotations;

namespace Congregation.Web.Data.Entities
{
    public class Profession
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
