using EntityFramework.DataAccess;
using EntityFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SchoolDB())
            {
                var stud = new Student() { StudentName = "Bill" };

                ctx.Students.Add(stud);
                ctx.SaveChanges();
            }
        }
    }
}
