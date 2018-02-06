using Xunit;

namespace Athena.Data.Tests
{
    public class MigrationTests : DataTest
    {
        [Fact]
        public void Valid() => Assert.True(true, "Migrations passed without error");
    }
}