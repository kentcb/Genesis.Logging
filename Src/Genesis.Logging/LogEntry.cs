namespace Genesis.Logging
{
    using System;

    /// <summary>
    /// Encapsulates data about a single log entry.
    /// </summary>
    public struct LogEntry
    {
        private readonly DateTime timestamp;
        private readonly string name;
        private readonly LogLevel level;
        private readonly int threadId;
        private readonly string message;

        /// <summary>
        /// Creates a new instance of the <c>LogEntry</c> structure.
        /// </summary>
        /// <param name="timestamp">
        /// The timestamp.
        /// </param>
        /// <param name="name">
        /// The logger name.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="threadId">
        /// The thread ID.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public LogEntry(DateTime timestamp, string name, LogLevel level, int threadId, string message)
        {
            this.timestamp = timestamp;
            this.name = name;
            this.level = level;
            this.threadId = threadId;
            this.message = message;
        }

        /// <summary>
        /// Gets the timestamp for the log entry.
        /// </summary>
        public DateTime Timestamp => this.timestamp;

        /// <summary>
        /// Gets the name for the log entry.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Gets the level for the log entry.
        /// </summary>
        public LogLevel Level => this.level;

        /// <summary>
        /// Gets the thread ID for the log entry.
        /// </summary>
        public int ThreadId => this.threadId;

        /// <summary>
        /// Gets the message for the log entry.
        /// </summary>
        public string Message => this.message;
    }
}