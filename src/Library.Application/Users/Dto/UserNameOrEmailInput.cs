using System.ComponentModel.DataAnnotations;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace Library.Users.Dto
{
    /// <summary>
    /// 两项填一个就行
    /// </summary>
    public class UserNameOrEmailInput : ICustomValidate
    {
        public string UserName { get; set; }

        public string Email { get; set; }


        public void AddValidationErrors(CustomValidationContext context)
        {
            var nullName = UserName.IsNullOrEmpty();
            var nullEmail = Email.IsNullOrEmpty();

            if (nullName && nullEmail)
            {
                context.Results.Add(new ValidationResult("One of the UserName and Email is required"));
            }

            if (!nullEmail && !nullName)
            {
                context.Results.Add(new ValidationResult("Just fill one of the two params"));
            }
        }
    }
}