using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MainApp.Models.GOT
{
    [Table("GOTCharacters")]
    public class Character
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname  { get; set; }
        [Required]
        public string Gender { get; set; }
        [Display(Name = "Kingdom")]
        public string Region { get; set; }
    }
}