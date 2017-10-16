using System;
using Abp.AutoMapper;

namespace Library.LibraryService.Dto
{
    [AutoMapFrom(typeof(BorrowRecord))]
    public class BorrowRecordDto
    {
        public long CopyId { get; set; }
        public long BorrowerUserId { get; set; }
        public int RenewedTimes { get; set; }
        public DateTime CreationTime { get; set; }
    }
}