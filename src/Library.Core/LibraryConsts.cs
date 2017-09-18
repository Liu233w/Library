using System;

namespace Library
{
    public class LibraryConsts
    {
        public const string LocalizationSourceName = "Library";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = false;

        /// <summary>
        /// 用户最多能借几本书
        /// </summary>
        public const int UserMaxBorrowCount = 5;

        /// <summary>
        /// 一本书最多能借几天
        /// </summary>
        public static readonly TimeSpan UserMaxBorrowDuration = new TimeSpan(14, 0, 0, 0);

        /// <summary>
        /// 续借一次可以借几天
        /// </summary>
        public static readonly TimeSpan RenewDuration = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// 最多能续借几次
        /// </summary>
        public const int MaxRenewTime = 3;
    }
}