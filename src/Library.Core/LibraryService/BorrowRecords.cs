using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Library.BookManage;

namespace Library.LibraryService
{
    /// <summary>
    /// 用户的借阅记录
    /// </summary>
    [Table("AppBorrowRecords")]
    public class BorrowRecords : Entity<long>, ICreationAudited, IDeletionAudited, ISoftDelete
    {
        public Book Book { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public bool IsDeleted { get; set; }
        // 删除时间就是还书时间
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
    }
}