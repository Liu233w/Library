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

        Task BorrowBook(BorrowBookInput input);

        Task ReturnBook(ReturnBookInput input);

        Task<GetOutdatedBorrowRecordOutput> GetOutdatedBorrowRecord();
    }
}