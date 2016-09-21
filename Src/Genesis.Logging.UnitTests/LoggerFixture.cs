namespace Genesis.Logging.UnitTests
{
    using System;
    using Xunit;

    public sealed class LoggerFixture
    {
        [Fact]
        public void current_throws_if_no_current_has_been_assigned()
        {
            Assert.Throws<InvalidOperationException>(() => LoggerService.Current);
        }
    }
}