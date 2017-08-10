using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RadCms.Data;

namespace RadCms.Entities
{
    public class FormSubmitRequest: IEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastSubmit { get; set; }
        public string ClientIP { get; set; }
    }
}
