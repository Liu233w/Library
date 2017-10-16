namespace Library.LibraryService.Dto
{
    public class BorrowBookInput
    {
        public string UserNameOrEmail { get; set; }
        public long CopyId { get; set; }
    }
}