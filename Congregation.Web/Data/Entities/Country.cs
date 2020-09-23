using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Web.Data.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }

        [DisplayName("District Number")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;

    }

}
