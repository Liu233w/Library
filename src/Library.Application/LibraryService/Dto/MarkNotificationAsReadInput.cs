using System;
using System.ComponentModel.DataAnnotations;

namespace Library.LibraryService.Dto
{
    public class MarkNotificationAsReadInput
    {
        [Required]
        public Guid UserNotificationId { get; set; }
    }
}