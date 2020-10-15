using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Congregation.Web.Data.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int IdDistrict { get; set; }

        [JsonIgnore]
        public District District { get; set; }

        [JsonIgnore]//TODO:esto mio para tomar el id en MeetingController
        public ICollection<User> Users { get; set; }

        [JsonIgnore]
        public ICollection<Meeting> Meetings { get; set; }

    }
}
