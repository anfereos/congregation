using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congregation.Common.Entities
{
    public class District
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The campo {0} must contain less than {1} caracteres.")]

        public string Name { get; set; }

        public ICollection<Church> Churches { get; set; }

        [DisplayName("Cities Number")]
        public int ChurchesNumber => Churches == null ? 0 : Churches.Count;

        [NotMapped]
        public int IdCountry { get; set; }

    }
}
