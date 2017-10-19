using System;

namespace Library.Controllers.Dto
{
    public class GetFileInput
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
    }
}
