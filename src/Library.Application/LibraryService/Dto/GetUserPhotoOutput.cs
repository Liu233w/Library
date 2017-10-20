using System;

namespace Library.LibraryService.Dto
{
    public class GetUserPhotoOutput
    {
        /// <summary>
        /// 照片ID。如果用户没有照片，为 null
        /// </summary>
        public Guid? PhotoId { get; set; }
    }
}