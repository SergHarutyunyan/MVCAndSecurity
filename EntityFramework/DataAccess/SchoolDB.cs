using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EntityFramework.Model;

namespace EntityFramework.DataAccess
{
    public class SchoolDB : DbContext
    {
        public SchoolDB() : base("SchoolDB")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SchoolDB>());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}
