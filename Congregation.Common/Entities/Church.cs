using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congregation.Common.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [NotMapped]
        public int IdDistrict { get; set; }

    }
}
