using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moq.EF.Tests
{
    [TestClass]
    public class MockDbSetTests
    {
        private Mock<FakeDbContext> _dataContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _dataContext = new Mock<FakeDbContext>();
            _dataContext.Setup(x => x.SaveChanges()).Returns(1);
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        [TestMethod]
        [DeploymentItem(@"test-data.json")]
        public void Test_GetQueryableMockDbSet_With_Json()
        {
            var fakeTableDbSet = MockDbSet.GetQueryableMockDbSet<FakeTable>(@"test-data.json");
            _dataContext.Setup(d => d.FakeTables).Returns(fakeTableDbSet.Object);
            Assert.IsNotNull(_dataContext.Object.FakeTables);
            Assert.AreEqual(_dataContext.Object.FakeTables.First().Name, "test-name");
        }

        [TestMethod]
        public void Test_GetQueryableMockDbSet_Empty()
        {
            var fakeTableDbSet = MockDbSet.GetQueryableMockDbSet<FakeTable>(@"");
            _dataContext.Setup(d => d.FakeTables).Returns(fakeTableDbSet.Object);
            Assert.IsNotNull(_dataContext.Object.FakeTables);
        }

        [TestMethod]
        public void Test_GetQueryableMockDbSet_With_Enumerable_Objects()
        {
            var fakeTableDbSet = MockDbSet.GetQueryableMockDbSet<FakeTable>(new List<FakeTable>()
            {
                new FakeTable()
                {
                    Id = 2,
                    Name = "test-object"
                }
            });
            _dataContext.Setup(d => d.FakeTables).Returns(fakeTableDbSet.Object);
            Assert.IsNotNull(_dataContext.Object.FakeTables);
            Assert.AreEqual(_dataContext.Object.FakeTables.First().Name, "test-object");
            Assert.AreEqual(_dataContext.Object.FakeTables.Count(), 1);
        }
    }
}
