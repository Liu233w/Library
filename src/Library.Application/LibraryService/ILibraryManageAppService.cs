using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Library.LibraryService.Dto;

namespace Library.LibraryService
{
    /// <summary>
    /// 让图书管理员操作的，关于图书借阅方面的功能
    /// </summary>
    public interface ILibraryManageAppService
    {
        /// <summary>
        /// 获取某本书的情况，包括借阅了这本书的用户、应还时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BookWithStatusAndRecord> GetBookStatus(GetBookStatusInput input);

        /// <summary>
        /// 借书，如果书籍不存在、用户不存在或者用户已经借过了这本书，都会抛出异常
        /// 用户借书的数量不能超过最大借书数量限制，否则抛出异常。
        /// 参见 <see cref="LibraryConsts.UserMaxBorrowCount"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task BorrowBook(BorrowBookInput input);

        /// <summary>
        /// 还书，如果书籍不存在、用户不存在或者用户没有借过这本书，都会抛出异常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ReturnBook(ReturnBookInput input);

        /// <summary>
        /// 获取过期未还书籍的记录
        /// </summary>
        /// <returns></returns>
        Task<GetOutdatedBorrowRecordOutput> GetOutdatedBorrowRecord();

        /// <summary>
        /// 获取所有未还图书的记录
        /// </summary>
        /// <returns></returns>
        Task<GetUnreturnedRecordOutput> GetUnreturnedRecord();

        /// <summary>
        /// 发布通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task PublishNotification(PublishNotificationInput input);

        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BroadcastNotificationDto>> GetNotificationList();

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteNotification(DeleteNotificationInput input);
    }
}