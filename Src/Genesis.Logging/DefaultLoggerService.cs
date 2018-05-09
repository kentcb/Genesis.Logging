namespace Genesis.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    /// <summary>
    /// The default implementation of <see cref="ILoggerService"/>.
    /// </summary>
    public sealed class DefaultLoggerService : ILoggerService
    {
        private readonly IDictionary<string, ILogger> loggers;
        private readonly ISubject<LogEntry> entries;
        private LogLevel threshold;

        /// <summary>
        /// Creates a new instance of the <c>DefaultLoggerService</c> class.
        /// </summary>
        public DefaultLoggerService()
        {
            this.loggers = new Dictionary<string, ILogger>();
            this.entries = new Subject<LogEntry>();
        }

        /// <inheritdoc />
        public LogLevel Threshold
        {
            get { return this.threshold; }
            set { this.threshold = value; }
        }

        /// <inheritdoc />
        public bool IsVerboseEnabled => this.threshold <= LogLevel.Verbose;

        /// <inheritdoc />
        public bool IsDebugEnabled => this.threshold <= LogLevel.Debug;

        /// <inheritdoc />
        public bool IsInfoEnabled => this.threshold <= LogLevel.Info;

        /// <inheritdoc />
        public bool IsPerfEnabled => this.threshold <= LogLevel.Perf;

        /// <inheritdoc />
        public bool IsWarnEnabled => this.threshold <= LogLevel.Warn;

        /// <inheritdoc />
        public bool IsErrorEnabled => this.threshold <= LogLevel.Error;

        /// <inheritdoc />
        public IObservable<LogEntry> Entries => this.entries.Where(x => x.Level >= this.Threshold);

        /// <inheritdoc />
        public ILogger GetLogger(Type forType)
        {
            if (forType.IsConstructedGenericType)
            {
                forType = forType.GetGenericTypeDefinition();
            }

            return this.GetLogger(forType.FullName);
        }

        /// <inheritdoc />
        public ILogger GetLogger(string name)
        {
            ILogger logger;

            // There is a race condition here, but we don't care enough to synchronize access. It just means we may end up
            // with more than one Logger instance with the same name, but it doesn't matter.
            if (!this.loggers.TryGetValue(name, out logger))
            {
                logger = new Logger(this, name);
                this.loggers[name] = logger;
            }

            return logger;
        }

        private sealed class Logger : ILogger
        {
            private readonly DefaultLoggerService owner;
            private readonly string name;

            public Logger(DefaultLoggerService owner, string name)
            {
                this.owner = owner;
                this.name = name;
            }

            public string Name => this.name;

            public bool IsVerboseEnabled => this.owner.IsVerboseEnabled;

            public bool IsDebugEnabled => this.owner.IsDebugEnabled;

            public bool IsInfoEnabled => this.owner.IsInfoEnabled;

            public bool IsPerfEnabled => this.owner.IsPerfEnabled;

            public bool IsWarnEnabled => this.owner.IsWarnEnabled;

            public bool IsErrorEnabled => this.owner.IsErrorEnabled;

            public void Log(LogLevel level, string message)
            {
                var entry = new LogEntry(DateTime.UtcNow, this.name, level, Environment.CurrentManagedThreadId, message);
                this.owner.entries.OnNext(entry);
            }
        }
    }
}