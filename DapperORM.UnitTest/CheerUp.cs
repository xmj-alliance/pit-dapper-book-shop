using DapperORM.App.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using System.Data.Common;
using System.Threading.Tasks;
using Xunit;

namespace DapperORM.UnitTest
{
    [Collection("Sequential")]
    public class CheerUpTest : IClassFixture<ServiceFixture>, IAsyncLifetime
    {
        private readonly IDBContext dbContext;
        private readonly IHost testHost;
        private readonly Checkpoint checkpoint = new ();

        public CheerUpTest(ServiceFixture fixture)
        {
            testHost = fixture.TestHost;
            dbContext = testHost.Services.GetService<IDBContext>();
        }

        [Fact]
        public void HaveFun()
        {
            Assert.Equal("😙", "😙");
        }

        public Task InitializeAsync() => checkpoint.Reset(dbContext.Connection as DbConnection);

        public Task DisposeAsync() => Task.CompletedTask;
    }

}
