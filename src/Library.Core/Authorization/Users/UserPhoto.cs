using System;
using Abp.Domain.Entities;
using Library.Storage;

namespace Library.Authorization.Users
{
    public class UserPhoto : Entity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public Guid PhotoId { get; set; }
        public BinaryObject Photo { get; set; }
    }
}