using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class MediaFile: IEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [MaxLength(31)]
        [Display(Name = "File Type")]
        public string FileType { get; set; }

        public int FileSize { get; set; }

        [Column(TypeName = "varbinary(MAX)")]
        public byte[] FileContent { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public System.DateTime Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public System.DateTime Modified { get; set; }
    }
}