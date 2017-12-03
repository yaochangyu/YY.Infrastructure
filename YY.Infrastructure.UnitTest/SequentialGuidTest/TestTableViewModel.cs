using System.ComponentModel.DataAnnotations.Schema;
using YY.Infrastructure;

namespace YY.Infrastructure.UnitTest.SequentialGuidTest.EntityModel
{
    public class TestTableViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public SequentialGuid Key { get; set; }
    }
}