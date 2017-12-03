using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YY.Infrastructure.UnitTest.SequentialGuidTest.EntityModel;

namespace YY.Infrastructure.UnitTest.SequentialGuidTest
{
    [TestClass]
    public class SequentialGuidUnitTest
    {
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)

        {
            //EF預熱
            using (var dbcontext = new TestDbContext())
            {
                var objectContext = ((IObjectContextAdapter) dbcontext).ObjectContext;
                var mappingCollection =
                    (StorageMappingItemCollection) objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 產生三個具有順序性的SequentialGuid_排序_得到連續順序()
        {
            var guid1 = SequentialGuid.NewGuid();

            //System.Threading.Thread.Sleep(500);
            var guid2 = SequentialGuid.NewGuid();

            //System.Threading.Thread.Sleep(500);
            var guid3 = SequentialGuid.NewGuid();

            var wrongOrder = new[] {guid2, guid3, guid1};
            var goodOrder = wrongOrder.OrderBy(g => g).ToList(); // sGuid1, sGuid2, sGuid3

            Assert.IsTrue(guid2 > guid1);
            Assert.IsTrue(guid3 > guid2);

            Assert.AreEqual(goodOrder[0], guid1);
            Assert.AreEqual(goodOrder[1], guid2);
            Assert.AreEqual(goodOrder[2], guid3);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入記憶體_Guid_調用ICompare_由大到小排序_得到連續順序()
        {
            var testTables = new List<TestTable>();

            //arrange

            for (var i = 1; i < 10000; i++)
            {
                var key = SequentialGuid.NewGuid();
                var name = i.ToString("000");
                var table = new TestTable {Id = i, Name = name, Key = key};
                testTables.Add(table);
            }

            //act
            var tables = testTables.OrderByDescending(p => p.Key, new SequentialGuidComparer())
                                   .ToList();

            var targetArray = tables.Select(p => p.Id).ToArray();

            var actual = this.IsReverseSequential(targetArray);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入記憶體_Guid_調用ICompare_由小到大排序_得到連續順序()
        {
            var testTables = new List<TestTable>();

            //arrange

            for (var i = 1; i < 10000; i++)
            {
                var key = SequentialGuid.NewGuid();
                var name = i.ToString("000");
                var table = new TestTable {Id = i, Name = name, Key = key};
                testTables.Add(table);
            }

            //act
            var tables = testTables.OrderBy(p => p.Key, new SequentialGuidComparer())
                                   .ToList();

            var targetArray = tables.Select(p => p.Id).ToArray();

            var actual = this.IsSequential(targetArray);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入記憶體_SequentialGuid_由大到小排序_得到連續順序()
        {
            var testTables = new List<TestTableViewModel>();

            //arrange

            for (var i = 1; i < 10000; i++)
            {
                var key = SequentialGuid.NewGuid();
                var name = i.ToString("000");
                var table = new TestTableViewModel {Id = i, Name = name, Key = key};
                testTables.Add(table);
            }

            //act
            var tables = testTables.OrderByDescending(p => p.Key)
                                   .ToList();

            var targetArray = tables.Select(p => p.Id).ToArray();

            var actual = this.IsReverseSequential(targetArray);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入記憶體_SequentialGuid_由小到大排序_得到連續順序()
        {
            var testTables = new List<TestTableViewModel>();

            //arrange

            for (var i = 1; i < 10000; i++)
            {
                var key = SequentialGuid.NewGuid();
                var name = i.ToString("000");
                var table = new TestTableViewModel {Id = i, Name = name, Key = key};
                testTables.Add(table);
            }

            //act
            var tables = testTables.OrderBy(p => p.Key)
                                   .ToList();

            var targetArray = tables.Select(p => p.Id).ToArray();

            var actual = this.IsSequential(targetArray);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入記憶體_時間相同_Guid_調用ICompare_由大到小排序_得到連續順序()
        {
            var testTables = new List<TestTable>();

            //arrange
            SequentialGuid.TimeNow = new DateTime(2010, 1, 1, 0, 0, 0, 0);
            for (var i = 1; i < 10000; i++)
            {
                var key = SequentialGuid.NewGuid();
                var name = i.ToString("000");
                var table = new TestTable {Id = i, Name = name, Key = key};
                testTables.Add(table);
            }

            //act
            var tables = testTables.OrderByDescending(p => p.Key, new SequentialGuidComparer())
                                   .ToList();

            var targetArray = tables.Select(p => p.Id).ToArray();

            var actual = this.IsReverseSequential(targetArray);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入資料庫_Guid_由大到小排序_得到反向連續順序()
        {
            var dbContext = new TestDbContext();
            try
            {
                //arrange
                dbContext.Database.Delete();

                for (var i = 1; i < 100; i++)
                {
                    var key = SequentialGuid.NewGuid();
                    var name = i.ToString("000");
                    var table = new TestTable {Name = name, Key = key};
                    dbContext.TestTables.Add(table);
                }

                dbContext.SaveChanges();

                //act
                var tables = dbContext.TestTables
                                      .OrderByDescending(p => p.Key)
                                      .AsNoTracking()
                                      .ToList();

                var targetArray = tables.Select(p => p.Id).ToArray();

                var actual = this.IsReverseSequential(targetArray);

                //assert
                Assert.IsTrue(actual);
            }
            finally
            {
                dbContext.Database.Delete();
                dbContext.Dispose();
            }
        }

        [TestMethod]
        [TestCategory("SequentialGuid")]
        public void 寫入資料庫_Guid_由小到大排序_得到連續順序()
        {
            var dbContext = new TestDbContext();
            try
            {
                //arrange
                dbContext.Database.Delete();

                for (var i = 1; i < 100; i++)
                {
                    var key = SequentialGuid.NewGuid();
                    var name = i.ToString("000");
                    var table = new TestTable {Name = name, Key = key};
                    dbContext.TestTables.Add(table);
                }

                dbContext.SaveChanges();

                //act
                var tables = dbContext.TestTables
                                      .OrderBy(p => p.Key)
                                      .AsNoTracking()
                                      .ToList();

                var targetArray = tables.Select(p => p.Id).ToArray();

                var actual = this.IsSequential(targetArray);

                //assert
                Assert.IsTrue(actual);
            }
            finally
            {
                dbContext.Database.Delete();
                dbContext.Dispose();
            }
        }

        private bool IsSequentialId(int[] array)
        {
            return array.Zip(array.Skip(1), (a, b) => a + 1 == b).All(x => x);
        }

        private bool IsReverseSequential(int[] array)
        {
            var result = true;

            var expected = Enumerable.Range(1, array.Length).Reverse().ToArray();

            var index = expected.Length;
            foreach (var p in expected)
            {
                if (array[p - index] - 1 != array[p - index + 1])
                {
                    result = false;
                    break;
                }

                index--;
            }

            return result;
        }

        private bool IsSequential(int[] array)
        {
            var result = false;

            result = Enumerable.Range(1, array.Length - 1).All(i => array[i] - 1 == array[i - 1]);
            return result;
        }
    }
}