using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Congregation.Common.Responses
{
    public class CountryResponse
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<DistrictResponse> Districts { get; set; }
    }
}
