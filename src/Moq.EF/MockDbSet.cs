using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Moq.EF
{
    public static class MockDbSet
    {
        public static Mock<IDbSet<T>> GetQueryableMockDbSet<T>(string fileName) where T : class
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var jsonString = System.IO.File.ReadAllText(fileName);
                var obj = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return GetQueryableMockDbSet(obj);
            }

            return GetQueryableMockDbSet(new List<T>());
        }

        public static Mock<IDbSet<T>> GetQueryableMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var enumerable = data as IList<T> ?? data.ToList();
            var queryable = enumerable.AsQueryable();

            var dbSet = new Mock<IDbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());            
            dbSet.Setup(x => x.Add(It.IsAny<T>())).Callback<T>((s) => enumerable.Add(s));

            // Return the mock
            return dbSet;
        }
    }
}
