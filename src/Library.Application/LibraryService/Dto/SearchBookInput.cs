namespace Library.LibraryService.Dto
{
    public class SearchBookInput
    {
        public SearchBookType Type { get; set; }
        public string KeyWord { get; set; }
    }
}