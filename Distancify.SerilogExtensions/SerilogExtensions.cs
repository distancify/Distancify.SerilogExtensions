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
                return _logger ?? global::Serilog.Log.Logger;
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
        /// Creates a logger for this class. Identical to Log&lt;T&gt;. Use if you need to avoid ambiguous references with legacy logging frameworks.
        /// </summary>
        public static ILogger Serilog<T>(this T target)
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

        /// <summary>
        /// Creates a log message tied to an incident. Can be used to trigger alarms.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="priority"></param>
        /// <param name="incidentType">In order to group type incidents that may have different messages, this code can be used</param>
        /// <param name="incidentId">In order to group multiple errors into the same incident instance, this ID can be used</param>
        /// <returns></returns>
        public static ILogger ForIncident(this ILogger logger, IncidentPriority priority, string incidentType = null, string incidentId = null)
        {
            var result = logger.ForContext("Incident", priority.ToString());
            if (!string.IsNullOrWhiteSpace(incidentType))
            {
                result = result.ForContext("IncidentType", incidentType);
            }
            if (!string.IsNullOrWhiteSpace(incidentId))
            {
                result = result.ForContext("IncidentId", incidentId);
            }
            return result;
        }
    }
}