using System.ComponentModel.DataAnnotations;

namespace Library.LibraryService.Dto
{
    public class PublishNotificationInput
    {
        [Required]
        public string Content { get; set; }
    }
}