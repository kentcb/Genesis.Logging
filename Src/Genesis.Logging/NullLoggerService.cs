namespace Genesis.Logging
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Subjects;

    /// <summary>
    /// An implementation of <see cref="ILoggerService"/> that does nothing.
    /// </summary>
    public sealed class NullLoggerService : ILoggerService
    {
        /// <summary>
        /// Get an observable that never ticks.
        /// </summary>
        public IObservable<LogEntry> Entries => NullSubject<LogEntry>.Instance;

        /// <summary>
        /// Always return <see langword="false"/>.
        /// </summary>
        public bool IsDebugEnabled => false;

        /// <summary>
        /// Always return <see langword="false"/>.
        /// </summary>
        public bool IsErrorEnabled => false;

        /// <summary>
        /// Always return <see langword="false"/>.
        /// </summary>
        public bool IsInfoEnabled => false;

        /// <summary>
        /// Always return <see langword="false"/>.
        /// </summary>
        public bool IsPerfEnabled => false;

        /// <summary>
        /// Always return <see langword="false"/>.
        /// </summary>
        public bool IsWarnEnabled => false;

        /// <summary>
        /// Always returns <see cref="LogLevel.None"/>.
        /// </summary>
        public LogLevel Threshold
        {
            get { return LogLevel.None; }
            set { }
        }

        /// <summary>
        /// Returns a logger that does nothing.
        /// </summary>
        /// <param name="name">
        /// The logger name.
        /// </param>
        /// <returns>
        /// A logger that does nothing.
        /// </returns>
        public ILogger GetLogger(string name) => NullLogger.Instance;

        /// <summary>
        /// Returns a logger that does nothing.
        /// </summary>
        /// <param name="forType">
        /// The type.
        /// </param>
        /// <returns>
        /// A logger that does nothing.
        /// </returns>
        public ILogger GetLogger(Type forType) => NullLogger.Instance;

        private sealed class NullSubject<T> : ISubject<T>
        {
            public static NullSubject<T> Instance = new NullSubject<T>();

            private NullSubject()
            {
            }

            public IDisposable Subscribe(IObserver<T> observer) =>
                Disposable.Empty;

            public void OnCompleted()
            {
            }

            public void OnError(Exception error)
            {
            }

            public void OnNext(T value)
            {
            }
        }

        private sealed class NullLogger : ILogger
        {
            public static NullLogger Instance = new NullLogger();

            private NullLogger()
            {
            }

            public bool IsDebugEnabled => false;

            public bool IsErrorEnabled => false;

            public bool IsInfoEnabled => false;

            public bool IsPerfEnabled => false;

            public bool IsWarnEnabled => false;

            public string Name => "Null Logger";

            public void Log(LogLevel level, string message)
            {
            }
        }
    }
}