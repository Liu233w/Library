using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Library.LibraryService;

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
        public string Location { get; set; }

        public ICollection<Copy> Copys { get; set; }

        public Guid? BookPhotoId { get; set; }
    }
}