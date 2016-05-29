namespace Genesis.Logging
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Used to time the execution of a block of code.
    /// </summary>
    public struct PerformanceBlock : IDisposable
    {
        /// <summary>
        /// An empty performance block.
        /// </summary>
        public static readonly PerformanceBlock Empty = new PerformanceBlock();

        private readonly ILogger owner;
        private readonly string message;
        private readonly Stopwatch stopwatch;
        private int disposed;

        /// <summary>
        /// Creates a new instance of <c>PerformanceBlock</c>.
        /// </summary>
        /// <param name="owner">
        /// The owning <see cref="ILogger"/> instance.
        /// </param>
        /// <param name="message">
        /// The message to output when the block completes.
        /// </param>
        public PerformanceBlock(ILogger owner, string message)
        {
            this.owner = owner;
            this.message = message;

            // NOTE: it's vital we use 0 to represent already disposed so that the Empty performance block does nothing
            this.disposed = 1;
            this.stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Completes the block. That is, stops the timer and outputs the execution time to the owning <see cref="ILogger"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref this.disposed, 0, 1) != 1)
            {
                return;
            }

            this.stopwatch.Stop();
            this.owner.Log(LogLevel.Perf, string.Format(CultureInfo.InvariantCulture, "{0} [{1} ({2}ms)]", message, this.stopwatch.Elapsed, this.stopwatch.ElapsedMilliseconds));
        }
    }
}