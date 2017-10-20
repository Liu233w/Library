namespace Library.LibraryService.Dto
{
    public class GetUserPhotoInput
    {
        /// <summary>
        /// 用户ID。留空时为本用户
        /// </summary>
        public long? UserId { get; set; }
    }
}