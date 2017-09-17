using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Library.BookManage.Dto
{
    [AutoMap(typeof(Book))]
    public class BookDto : EntityDto<long>
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publish { get; set; }
    }
}