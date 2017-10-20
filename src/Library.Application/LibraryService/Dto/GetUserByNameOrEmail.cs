using System.ComponentModel.DataAnnotations;

namespace Library.LibraryService.Dto
{
    public class GetUserByNameOrEmail
    {
        [Required]
        public string UserNameOrEmail { get; set; }
    }
}