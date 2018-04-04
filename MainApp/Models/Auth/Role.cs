using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MainApp.Models.Auth
{
    [Table("Roles")]
    public class Role
    {
        [Required]
        [Key]
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}