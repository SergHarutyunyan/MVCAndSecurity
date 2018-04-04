using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MainApp.Models.Auth
{
    [Table("Users")]
    public class User
    {
        [Required]
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public bool Remember { get; set; }
    }
}