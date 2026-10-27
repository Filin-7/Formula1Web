using System.ComponentModel.DataAnnotations;

namespace Formula1Web.Models
{
    public class EmailMessageViewModel
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Введіть коректний email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Введіть повідомлення")]
        public string Message { get; set; } = "";
    }
}