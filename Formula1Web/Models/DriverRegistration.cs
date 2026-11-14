
namespace Formula1Web.Models
{
    public class DriverRegistration
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Country { get; set; } = string.Empty;

        public string Team { get; set; } = string.Empty;

        public int CarNumber { get; set; }

        public int ExperienceYears { get; set; }

        // Необов'язкові поля (nullable)
        public string? PhoneNumber { get; set; }

        public string? Biography { get; set; }
    }
}
