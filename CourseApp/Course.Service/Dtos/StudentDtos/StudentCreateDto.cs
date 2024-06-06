using System;
using FluentValidation;

namespace Course.Service.Dtos.StudentDtos
{
	public class StudentCreateDto
	{
        public string FullName { get; set; }

        public string Email { get; set; }

        public DateTime Birthdate { get; set; }

        public int GroupId { get; set; }
    }
    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(35).MinimumLength(6);

            RuleFor(x => x.Email).NotNull().EmailAddress();

            RuleFor(x => x.Birthdate).NotEmpty().Must(BeAtLeast14YearsOld);

            RuleFor(x => x.GroupId).NotNull().GreaterThan(0);

        }
        private bool BeAtLeast14YearsOld(DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;

            if (birthdate.Date > today.AddYears(-age))
            {
                age--;
            }
            return age >= 14;
        }
    }

}

