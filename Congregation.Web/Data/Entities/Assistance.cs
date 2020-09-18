namespace Congregation.Web.Data.Entities
{
    public class Assistance
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Meeting Meeting { get; set; }

        public bool IsPresent { get; set; }
    }
}
