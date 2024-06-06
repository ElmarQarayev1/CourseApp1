using System;

using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Course.Service.Dtos.GroupDtos
{
	public class GroupCreateDto
	{
        public string No { get; set; }

        public byte Limit { get; set; }
        
        public IFormFile? ImageFile { get; set; }
    }
    public class CourseCreateDtoValidator : AbstractValidator<GroupCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.No).NotEmpty().MaximumLength(5).MinimumLength(4);

            RuleFor(x => (int)x.Limit).NotNull().InclusiveBetween(5, 20);

            RuleFor(x => x.ImageFile)
               .Must(file => file == null || file.Length <= 2 * 1024 * 1024)
               .WithMessage("File must be less or equal than 2MB")
               .Must(file => file == null || new[] { "image/png", "image/jpeg" }.Contains(file.ContentType))
               .WithMessage("File type must be png, jpeg, or jpg");
        }
    }

}

