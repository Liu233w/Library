﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Library.LibraryService.Dto;

namespace Library.LibraryService
{
    public interface ILibraryAppService : IApplicationService
    {
        /// <summary>
        /// 获取书籍列表（给用户看的）
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BookWithStatusAndMine>> GetBookList();

        /// <summary>
        /// 获取某本书的情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BookWithStatusAndMine> GetBook(GetBookInput input);

        /// <summary>
        /// 续借图书，如果续借次数超过限制，会抛出异常。最多续借次数参见 <see cref="LibraryConsts.MaxRenewTime"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task RenewBook(RenewBookInput input);

        /// <summary>
        /// 获取当前用户借书情况（所有未还的图书）
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BookWithStatusAndMine>> GetUserBook();

        /// <summary>
        /// 获取借阅记录，包含当前正在借的和已经还了的所有图书
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<UserBorrowRecord>> GetBorrowRecords();

        /// <summary>
        /// 获取当前用户的通知，可以指定通知类型
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<BroadcastUserNotificationDto>> GetMyNotifications(GetMyNotificationsInput input);

        /// <summary>
        /// 将某通知标记为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MarkNotificationAsRead(MarkNotificationAsReadInput input);

        /// <summary>
        /// 获取通知数量，包括已读通知和未读通知
        /// </summary>
        /// <returns></returns>
        Task<GetNotificationCountOutput> GetNotificationCount();

        /// <summary>
        /// 获取用户的照片 id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetUserPhotoOutput> GetUserPhoto(GetUserPhotoInput input);

        /// <summary>
        /// 设定本用户的照片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SetUserPhoto(SetUserPhotoInput input);
    }
}