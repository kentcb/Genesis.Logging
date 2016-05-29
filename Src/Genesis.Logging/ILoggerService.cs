namespace Genesis.Logging
{
    using System;

    /// <summary>
    /// Provides a means of obtaining <see cref="ILogger"/> instances, and monitoring log output.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Gets or sets the threshold for log messages. Any message under this threshold will be silently dropped.
        /// </summary>
        LogLevel Threshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Debug"/> level is enabled.
        /// </summary>
        bool IsDebugEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Info"/> level is enabled.
        /// </summary>
        bool IsInfoEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Perf"/> level is enabled.
        /// </summary>
        bool IsPerfEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Warn"/> level is enabled.
        /// </summary>
        bool IsWarnEnabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Error"/> level is enabled.
        /// </summary>
        bool IsErrorEnabled
        {
            get;
        }

        /// <summary>
        /// Gets an observable that ticks a <see cref="LogEntry"/> whenever a new log entry comes in.
        /// </summary>
        IObservable<LogEntry> Entries
        {
            get;
        }

        /// <summary>
        /// Gets a logger for a given type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is a convenience. It simply forwards the call onto <see cref="GetLogger(string)"/>, passing in the type's full name.
        /// </para>
        /// </remarks>
        /// <param name="forType">
        /// The type that will be consuming the <see cref="ILogger"/>.
        /// </param>
        /// <returns>
        /// The <see cref="ILogger"/> instance.
        /// </returns>
        ILogger GetLogger(Type forType);

        /// <summary>
        /// Gets a logger with a given name.
        /// </summary>
        /// <param name="name">
        /// The name for the logger.
        /// </param>
        /// <returns>
        /// The <see cref="ILogger"/> instance.
        /// </returns>
        ILogger GetLogger(string name);
    }
}