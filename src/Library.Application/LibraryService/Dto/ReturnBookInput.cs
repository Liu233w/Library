namespace Library.LibraryService.Dto
{
    public class ReturnBookInput
    {
        public string UserNameOrEmail { get; set; }
        public long BookId { get; set; }
    }
}