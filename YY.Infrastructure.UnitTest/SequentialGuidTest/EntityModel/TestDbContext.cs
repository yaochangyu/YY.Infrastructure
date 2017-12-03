using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YY.Infrastructure.UnitTest.SequentialGuidTest.EntityModel
{
    internal class TestDbContext : DbContext
    {
        public DbSet<TestTable> TestTables { get; set; }
    }
}
