using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq.EF.Tests
{
    public class FakeDbContext : DbContext
    {
        public virtual IDbSet<FakeTable> FakeTables { get; set; }
    }
}
