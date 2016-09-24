namespace Genesis.Logging.UnitTests
{
    using Xunit;

    public sealed class LoggerFixture
    {
        [Fact]
        public void current_is_null_logger_service_by_default()
        {
            Assert.IsType<NullLoggerService>(LoggerService.Current);
        }
    }
}