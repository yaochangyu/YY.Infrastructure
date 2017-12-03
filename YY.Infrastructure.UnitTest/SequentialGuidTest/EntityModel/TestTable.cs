using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YY.Infrastructure.UnitTest.SequentialGuidTest.EntityModel
{
    [Table("TestTable")]
    public class TestTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        //public SequentialGuid Key { get; set; }
        public Guid Key { get; set; }
    }
}