using Abp.Domain.Entities;

namespace Library.BookManage
{
    public class Book : Entity<long>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}