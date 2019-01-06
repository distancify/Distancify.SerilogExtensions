using Serilog;

namespace Distancify.SerilogExtensions
{
    public static class SerilogExtensions
    {
        private static ILogger _logger;

        /// <summary>
        /// The underlying logger to use in the .Log() extension method provided by this class. If not set, the static <see cref="Serilog.Log.Logger"/> will be used.
        /// </summary>
        public static ILogger Logger
        {
            get
            {
                return _logger ?? Serilog.Log.Logger;
            }
            set
            {
                _logger = value;
            }
        }

        /// <summary>
        /// Creates a logger for this class
        /// </summary>
        public static ILogger Log<T>(this T target)
        {
            return Logger.ForContext(typeof(T));
        }

        /// <summary>
        /// Creates a logger which will enrich every message with an "ElapsedMs" that specifies the time between each log message in milliseconds.
        /// </summary>
        /// <param name="name">Optional profiling name to add to logs</param>
        public static ILogger Profile(this ILogger logger, string name = null)
        {
            return logger.ForContext(new[] { new ProfilingEnricher(name) });
        }
    }
}