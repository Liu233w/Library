using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Library.Authorization.Users;
using Library.BookManage;

namespace Library.LibraryService
{
    /// <summary>
    /// 用户的借阅记录
    /// </summary>
    [Table("AppBorrowRecords")]
    public class BorrowRecord : Entity<long>, ICreationAudited<User>, IDeletionAudited<User>, ISoftDelete
    {
        public Book Book { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public bool IsDeleted { get; set; }
        // 删除时间就是还书时间
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public User CreatorUser { get; set; }
        public User DeleterUser { get; set; }
    }
}