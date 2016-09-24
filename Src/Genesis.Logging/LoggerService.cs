namespace Genesis.Logging
{
    using System;

    /// <summary>
    /// Provides a convenient ambient context and related members.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Since logging is a cross-cutting concern that rarely needs to be unit tested, an ambient context makes good sense. This
    /// saves having to inject an <see cref="ILogger"/> instance (or <see cref="ILoggerService"/> instance) into components that
    /// need to log. Instead, a single <see cref="ILoggerService"/> can be assigned to the <see cref="Current"/> ambient context,
    /// and <see cref="ILogger"/> instances can be easily resolved via one of the <c>GetLogger</c> overloads.
    /// </para>
    /// </remarks>
    public static class LoggerService
    {
        private static ILoggerService current = new NullLoggerService();

        /// <summary>
        /// Gets or sets the current <see cref="ILoggerService"/>.
        /// </summary>
        public static ILoggerService Current
        {
            get
            {
                var current = LoggerService.current;

                if (current == null)
                {
                    throw new InvalidOperationException("You must assign an ILoggerService to LoggerService.Current before it can be retrieved.");
                }

                return current;
            }
            set { current = value; }
        }

        /// <summary>
        /// Gets a logger with the given name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a convenience method that is equivalent to calling <c>Current.GetLogger(string)</c>.
        /// </para>
        /// </remarks>
        /// <param name="name">
        /// The logger name.
        /// </param>
        /// <returns>
        /// The logger.
        /// </returns>
        public static ILogger GetLogger(string name) =>
            Current.GetLogger(name);

        /// <summary>
        /// Gets a logger for the given type.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is a convenience method that is equivalent to calling <c>Current.GetLogger(Type)</c>.
        /// </para>
        /// </remarks>
        /// <param name="forType">
        /// The logger type.
        /// </param>
        /// <returns>
        /// The logger.
        /// </returns>
        public static ILogger GetLogger(Type forType) =>
            Current.GetLogger(forType);
    }
}