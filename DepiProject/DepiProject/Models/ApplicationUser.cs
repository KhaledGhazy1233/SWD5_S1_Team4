using System.ComponentModel.DataAnnotations;

namespace DepiProject.Models
{
    public class ApplicationUser
    {
        [Required]

        public string? City { get; set; }

        public string? State { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? Gender { get; set; }


        public string? Country { get; set; }
        public string? Street { get; set; }
    }
}
