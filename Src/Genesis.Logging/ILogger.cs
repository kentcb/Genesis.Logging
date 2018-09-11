namespace Genesis.Logging
{
    /// <summary>
    /// Provides a means of logging messages of differing levels.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="LogLevel.Verbose"/> level is enabled.
        /// </summary>
        bool IsVerboseEnabled
        {
            get;
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
        /// Logs a message at a given level.
        /// </summary>
        /// <param name="level">
        /// The level to log the message at.
        /// </param>
        /// <param name="message">
        /// The message to log.
        /// </param>
        void Log(LogLevel level, string message);
    }
}