using System.ComponentModel.DataAnnotations;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace Library.BookManage.Dto
{
    [AutoMap(typeof(Book))]
    public class BookDto : EntityDto<long>, ICustomValidate
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publish { get; set; }
        public int Count { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!Isbn.IsNullOrEmpty())
            {
                if (!Isbn.Length.IsIn(10, 13))
                {
                    context.Results.Add(new ValidationResult("The length of ISBN must be 10 or 13"));
                }

                if (Isbn.Any(letter => !letter.IsIn('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')))
                {
                    context.Results.Add(new ValidationResult("ISBN must be numbers"));
                }
            }

            if (Count <= 0)
            {
                context.Results.Add(new ValidationResult("Count must be greater than 0"));
            }
        }
    }
}