using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Library.LibraryService;

namespace Library.BookManage
{
    [Table("AppBookCopys")]
    public class Copy : Entity<long>
    {
        public Book Book { get; set; }
        public long BookId { get; set; }

        /// <summary>
        /// 当前占用的借书 id。如果存在，就说明这个副本已经借出去了
        /// </summary>
        public long? BorrowRecordId { get; set; }
        public BorrowRecord BorrowRecord { get; set; }
    }
}