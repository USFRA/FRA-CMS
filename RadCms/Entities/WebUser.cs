using RadCms.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Entities
{
    public class WebUser : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ADDOT Login")]
        [MaxLength(50)]
        public string AdName { get; set; }

        [Required]
        [MaxLength(256)]
        public virtual string UserName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [MaxLength(256)]
        public virtual string Email { get; set; }

        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [MaxLength(50)]
        public virtual string LastName { get; set; }

        public virtual Organization Organization { get; set; }

        [Required, DataType(DataType.Password)]
        [MaxLength(256)]
        public virtual string Password { get; set; }
        public virtual DateTime? LastPasswordFailureDate { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        public virtual DateTime? LastPasswordChangedDate { get; set; }

        [MaxLength(100)]
        public virtual string PasswordResetToken { get; set; }
        public virtual DateTime? PasswordResetTokenDate { get; set; }
        public virtual int PasswordResetTokenFailures { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(256)]
        public virtual string Comment { get; set; }

        public virtual DateTime? LastActivityDate { get; set; }
        public virtual DateTime? LastLoginDate { get; set; }

        public virtual bool IsApproved { get; set; }
        public virtual bool IsLockedOut { get; set; }
        public virtual DateTime? LastLockoutDate { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual ICollection<WebRole> Roles { get; set; }
    }
}