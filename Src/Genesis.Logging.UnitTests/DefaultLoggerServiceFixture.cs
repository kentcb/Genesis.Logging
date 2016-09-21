namespace Genesis.Logging.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using Xunit;

    public sealed class DefaultLoggerServiceFixture
    {
        [Fact]
        public void threshold_can_be_retrieved()
        {
            var sut = new DefaultLoggerService();
            Assert.Equal(LogLevel.Debug, sut.Threshold);
        }

        [Fact]
        public void threshold_can_be_set()
        {
            var sut = new DefaultLoggerService();
            sut.Threshold = LogLevel.Error;
            Assert.Equal(LogLevel.Error, sut.Threshold);
            sut.Threshold = LogLevel.Info;
            Assert.Equal(LogLevel.Info, sut.Threshold);
        }

#if LOGGING

        [Fact]
        public void threshold_determines_which_log_entries_tick()
        {
            var sut = new DefaultLoggerService();
            var logger = sut.GetLogger(this.GetType());
            var entries = new List<LogEntry>();
            sut.Entries.Subscribe(entries.Add);

            logger.Debug("Expected 1");
            logger.Info("Expected 2");
            sut.Threshold = LogLevel.Info;
            logger.Debug("NOT expected");
            logger.Info("Expected 3");
            logger.Warn("Expected 4");
            sut.Threshold = LogLevel.Perf;
            logger.Info("NOT expected");
            logger.Perf("Expected 5").Dispose();
            sut.Threshold = LogLevel.Warn;
            logger.Perf("NOT expected").Dispose();
            logger.Warn("Expected 6");
            sut.Threshold = LogLevel.Error;
            logger.Warn("NOT expected");
            logger.Error("Expected 7");
            sut.Threshold = LogLevel.None;
            logger.Error("NOT expected");

            Assert.Equal(7, entries.Count);
            Assert.Equal("Expected 1", entries[0].Message);
            Assert.Equal("Expected 2", entries[1].Message);
            Assert.Equal("Expected 3", entries[2].Message);
            Assert.Equal("Expected 4", entries[3].Message);
            Assert.True(entries[4].Message.StartsWith("Expected 5 ["));
            Assert.Equal("Expected 6", entries[5].Message);
            Assert.Equal("Expected 7", entries[6].Message);
        }

#endif

        [Fact]
        public void is_debug_enabled_returns_correct_value()
        {
            var sut = new DefaultLoggerService();

            Assert.True(sut.IsDebugEnabled);
            sut.Threshold = LogLevel.Info;
            Assert.False(sut.IsDebugEnabled);
        }

        [Fact]
        public void is_info_enabled_returns_correct_value()
        {
            var sut = new DefaultLoggerService();

            Assert.True(sut.IsInfoEnabled);
            sut.Threshold = LogLevel.Perf;
            Assert.False(sut.IsInfoEnabled);
        }

        [Fact]
        public void is_perf_enabled_returns_correct_value()
        {
            var sut = new DefaultLoggerService();

            Assert.True(sut.IsPerfEnabled);
            sut.Threshold = LogLevel.Warn;
            Assert.False(sut.IsPerfEnabled);
        }

        [Fact]
        public void is_warn_enabled_returns_correct_value()
        {
            var sut = new DefaultLoggerService();

            Assert.True(sut.IsWarnEnabled);
            sut.Threshold = LogLevel.Error;
            Assert.False(sut.IsWarnEnabled);
        }

        [Fact]
        public void is_error_enabled_always_returns_true()
        {
            var sut = new DefaultLoggerService();

            Assert.True(sut.IsErrorEnabled);
            sut.Threshold = LogLevel.Error;
            Assert.True(sut.IsErrorEnabled);
        }

        [Fact]
        public void entries_is_not_null()
        {
            var sut = new DefaultLoggerService();

            Assert.NotNull(sut.Entries);
        }

#if LOGGING

        [Fact]
        public void entries_ticks_for_every_log_entry()
        {
            var sut = new DefaultLoggerService();
            var entries = new List<LogEntry>();
            sut.Entries.Subscribe(entries.Add);
            var logger1 = sut.GetLogger("First");
            var logger2 = sut.GetLogger("Second");

            logger1.Debug("Debug message.");
            logger1.Warn("Some warning.");
            logger2.Info("Something from logger 2.");
            logger1.Debug("Another debug message.");
            logger2.Error("Something bad happened.");

            Assert.Equal(5, entries.Count);
            Assert.Equal(LogLevel.Debug, entries[0].Level);
            Assert.Equal("Debug message.", entries[0].Message);
            Assert.Equal(LogLevel.Warn, entries[1].Level);
            Assert.Equal("Some warning.", entries[1].Message);
            Assert.Equal(LogLevel.Info, entries[2].Level);
            Assert.Equal("Something from logger 2.", entries[2].Message);
            Assert.Equal(LogLevel.Debug, entries[3].Level);
            Assert.Equal("Another debug message.", entries[3].Message);
            Assert.Equal(LogLevel.Error, entries[4].Level);
            Assert.Equal("Something bad happened.", entries[4].Message);
        }

#endif

        [Fact]
        public void get_logger_for_type_returns_a_logger_with_the_full_name_of_the_type_passed_in()
        {
            var sut = new DefaultLoggerService();
            var logger = sut.GetLogger(this.GetType());

            Assert.Equal(this.GetType().FullName, logger.Name);
        }

        [Fact]
        public void get_logger_for_type_returns_the_same_logger_for_the_same_type()
        {
            var sut = new DefaultLoggerService();
            var logger1 = sut.GetLogger(this.GetType());
            var logger2 = sut.GetLogger(this.GetType());

            Assert.Same(logger1, logger2);
        }

        [Fact]
        public void get_logger_by_name_returns_a_logger_with_the_specified_name()
        {
            var sut = new DefaultLoggerService();
            var logger = sut.GetLogger("Some Logger");

            Assert.Equal("Some Logger", logger.Name);
        }

        [Fact]
        public void get_logger_by_name_returns_the_same_logger_for_the_same_name()
        {
            var sut = new DefaultLoggerService();
            var logger1 = sut.GetLogger("Name");
            var logger2 = sut.GetLogger("Name");

            Assert.Same(logger1, logger2);
        }
    }
}