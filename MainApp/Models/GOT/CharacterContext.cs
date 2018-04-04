using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MainApp.Models.GOT
{
    public class CharacterContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
    }
}