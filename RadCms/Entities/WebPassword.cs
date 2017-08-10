using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Entities
{
    /// <summary>
    /// Password History
    /// </summary>
    public class WebPassword
    {
        [Key]
        public virtual Guid UserId { get; set; }
   
        public virtual String Passwords { get; set; }
    }
}