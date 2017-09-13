using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Library.BookManage
{
    [Table("AppBooks")]
    public class Book : Entity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Publish { get; set; }
    }
}