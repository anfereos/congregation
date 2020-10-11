using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Congregation.Common.Responses
{
    public class DistrictResponse
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The campo {0} must contain less than {1} caracteres.")]
        public string Name { get; set; }

        public ICollection<ChurchResponse> Churches { get; set; }

        public int IdCountry { get; set; }

        public CountryResponse Country { get; set; }
    }
}
