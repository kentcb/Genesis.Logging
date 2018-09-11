namespace Genesis.Logging
{
    /// <summary>
    /// Defines possible log levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// A verbose debug message.
        /// </summary>
        Verbose,
        
        /// <summary>
        /// A debug message.
        /// </summary>
        Debug,

        /// <summary>
        /// An informational message.
        /// </summary>
        Info,

        /// <summary>
        /// A performance message.
        /// </summary>
        Perf,

        /// <summary>
        /// A warning message.
        /// </summary>
        Warn,

        /// <summary>
        /// An error message.
        /// </summary>
        Error,

        /// <summary>
        /// All levels disabled.
        /// </summary>
        None
    }
}