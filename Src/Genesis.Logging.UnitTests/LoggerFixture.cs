namespace Genesis.Logging.UnitTests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public sealed class LoggerFixture
    {
        [Fact]
        public void name_returns_the_logger_name()
        {
            var service = new LoggerService();
            var sut = service.GetLogger("Captain Winters");
            Assert.Equal("Captain Winters", sut.Name);
        }

        [Fact]
        public void is_debug_enabled_returns_correct_value()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());

            Assert.True(sut.IsDebugEnabled);
            service.Threshold = LogLevel.Info;
            Assert.False(sut.IsDebugEnabled);
        }

        [Fact]
        public void is_info_enabled_returns_correct_value()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());

            Assert.True(sut.IsInfoEnabled);
            service.Threshold = LogLevel.Perf;
            Assert.False(sut.IsInfoEnabled);
        }

        [Fact]
        public void is_perf_enabled_returns_correct_value()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());

            Assert.True(sut.IsPerfEnabled);
            service.Threshold = LogLevel.Warn;
            Assert.False(sut.IsPerfEnabled);
        }

        [Fact]
        public void is_warn_enabled_returns_correct_value()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());

            Assert.True(sut.IsWarnEnabled);
            service.Threshold = LogLevel.Error;
            Assert.False(sut.IsWarnEnabled);
        }

        [Fact]
        public void is_error_enabled_returns_correct_value()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());

            Assert.True(sut.IsErrorEnabled);
            service.Threshold = LogLevel.None;
            Assert.False(sut.IsErrorEnabled);
        }

#if LOGGING

        [Theory]
        [InlineData(LogLevel.Debug, "A message.")]
        [InlineData(LogLevel.Debug, "A different message.")]
        [InlineData(LogLevel.Info, "A message.")]
        [InlineData(LogLevel.Info, "A different message.")]
        [InlineData(LogLevel.Perf, "A message.")]
        [InlineData(LogLevel.Perf, "A different message.")]
        [InlineData(LogLevel.Warn, "A message.")]
        [InlineData(LogLevel.Warn, "A different message.")]
        [InlineData(LogLevel.Error, "A message.")]
        [InlineData(LogLevel.Error, "A different message.")]
        public void logging_message_generates_appropriate_log_entry(LogLevel level, string message)
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());
            var logEntry = default(LogEntry);
            service.Entries.Subscribe(x => logEntry = x);

            var result = typeof(LoggerExtensions)
                .GetTypeInfo()
                .DeclaredMethods
                .Where(method => method.Name == level.ToString() && method.GetParameters().Length == 2)
                .Single()
                .Invoke(null, new object[] { sut, message });

            (result as IDisposable)?.Dispose();

            Assert.Equal(this.GetType().FullName, logEntry.Name);
            Assert.Equal(level, logEntry.Level);
            Assert.Equal(Environment.CurrentManagedThreadId, logEntry.ThreadId);
            Assert.True(logEntry.Message.StartsWith(message));
        }

        [Theory]
        [InlineData(LogLevel.Debug, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Debug, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Info, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Info, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Perf, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Perf, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Warn, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Warn, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Error, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Error, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        public void logging_formatted_message_generates_appropriate_log_entry(LogLevel level, string format, object[] args)
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());
            var logEntry = default(LogEntry);
            service.Entries.Subscribe(x => logEntry = x);

            var parameters = new object[] { sut, format }
                .Concat(args)
                .ToArray();
            var typeParameters = args
                .Select(arg => arg.GetType())
                .ToArray();

            var result = typeof(LoggerExtensions)
                .GetTypeInfo()
                .DeclaredMethods
                .Where(
                    method =>
                        method.Name == level.ToString() &&
                        method.GetParameters().Length == parameters.Length &&
                        method.GetParameters()[1].ParameterType != typeof(Exception) &&
                        method.GetParameters()[2].ParameterType != typeof(object[]))
                .Single()
                .MakeGenericMethod(typeParameters)
                .Invoke(null, parameters);

            (result as IDisposable)?.Dispose();

            Assert.Equal(this.GetType().FullName, logEntry.Name);
            Assert.Equal(level, logEntry.Level);
            Assert.Equal(Environment.CurrentManagedThreadId, logEntry.ThreadId);
            Assert.True(logEntry.Message.StartsWith(string.Format(format, args)));
        }

        [Theory]
        [InlineData(LogLevel.Debug, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Debug, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Info, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Info, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Warn, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Warn, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        [InlineData(LogLevel.Error, "A {0}.", new object[] { "message" })]
        [InlineData(LogLevel.Error, "A {0} from {1} containing {2}.", new object[] { "message", "me", 42 })]
        public void logging_formatted_message_with_exception_generates_appropriate_log_entry(LogLevel level, string format, object[] args)
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());
            var logEntry = default(LogEntry);
            service.Entries.Subscribe(x => logEntry = x);

            var exception = new Exception("Circular logic detected.");
            var parameters = new object[] { sut, exception, format }
                .Concat(args)
                .ToArray();
            var typeParameters = args
                .Select(arg => arg.GetType())
                .ToArray();

            typeof(LoggerExtensions)
                .GetTypeInfo()
                .DeclaredMethods
                .Where(
                    method =>
                        method.Name == level.ToString() &&
                        method.GetParameters().Length == parameters.Length &&
                        method.GetParameters()[1].ParameterType == typeof(Exception) &&
                        method.GetParameters()[3].ParameterType != typeof(object[]))
                .Single()
                .MakeGenericMethod(typeParameters)
                .Invoke(null, parameters);

            Assert.Equal(this.GetType().FullName, logEntry.Name);
            Assert.Equal(level, logEntry.Level);
            Assert.Equal(Environment.CurrentManagedThreadId, logEntry.ThreadId);
            Assert.True(logEntry.Message.StartsWith(string.Format(format, args) + "System.Exception: Circular logic detected."));
        }

        [Fact]
        public void perf_logs_only_once_disposable_is_disposed()
        {
            var service = new LoggerService();
            var sut = service.GetLogger(this.GetType());
            var logged = false;
            service.Entries.Subscribe(x => logged = true);

            var disposable = sut.Perf("Here is the message.");

            Assert.False(logged);
            disposable.Dispose();
            Assert.True(logged);
        }

#endif
    }
}