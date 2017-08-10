using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class Favorite: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        public string Name { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        public string Url { get; set; }

        public virtual CmsPage Page{ get; set;}

        [MaxLength(100)]
        public string CreatedBy { get; set; }
        public System.DateTime Created { get; set; }

        [MaxLength(100)]
        public string ModifiedBy { get; set; }
        public System.DateTime Modified { get; set; }
    }
}