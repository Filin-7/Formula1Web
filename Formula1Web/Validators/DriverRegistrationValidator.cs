
using FluentValidation;
using Formula1Web.Models;

namespace Formula1Web.Validators
{
    public class DriverRegistrationValidator
        : AbstractValidator<DriverRegistration>
    {
        public DriverRegistrationValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .Length(2, 50)
                .WithMessage("First name must contain 2–50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .Length(2, 50)
                .WithMessage("Last name must contain 2–50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Enter a valid email address.");

            RuleFor(x => x.Age)
                .InclusiveBetween(16, 80)
                .WithMessage("Age must be between 16 and 80.");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.");

            RuleFor(x => x.Team)
                .NotEmpty()
                .WithMessage("Team is required.");

            RuleFor(x => x.CarNumber)
                .InclusiveBetween(1, 99)
                .WithMessage("Car number must be between 1 and 99.");

            RuleFor(x => x.ExperienceYears)
                .InclusiveBetween(0, 60)
                .WithMessage("Experience must be between 0 and 60 years.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[0-9\s\-()]{7,20}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Enter a valid phone number.");

            RuleFor(x => x.Biography)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Biography))
                .WithMessage("Biography cannot exceed 500 characters.");
        }
    }
}
