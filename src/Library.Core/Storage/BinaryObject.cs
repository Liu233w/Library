using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace Library.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant, ICreationAudited, IHasCreationTime, IExtendableObject
    {
        public virtual int? TenantId { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string FilePath { get; set; }
        [StringLength(256)]
        public virtual string FileName { get; set; }

        public string ExtensionData { get; set; }
        //
        // 摘要:
        //     Creation time of this entity.
        public virtual DateTime CreationTime { get; set; }
        //
        // 摘要:
        //     Creator of this entity.
        public virtual long? CreatorUserId { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        //public BinaryObject(int? tenantId, byte[] bytes)
        public BinaryObject(int? tenantId, string path, string filename)
            : this()
        {
            TenantId = tenantId;
            FilePath = path;
            FileName = filename;
        }
    }
}
