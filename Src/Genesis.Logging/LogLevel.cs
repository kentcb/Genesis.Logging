namespace Genesis.Logging
{
    /// <summary>
    /// Defines possible log levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A debug message.
        /// </summary>
        Debug = 0,

        /// <summary>
        /// An informational message.
        /// </summary>
        Info = 1,

        /// <summary>
        /// A performance message.
        /// </summary>
        Perf = 2,

        /// <summary>
        /// A warning message.
        /// </summary>
        Warn = 3,

        /// <summary>
        /// An error message.
        /// </summary>
        Error = 4,

        /// <summary>
        /// All levels disabled.
        /// </summary>
        None = 5
    }
}