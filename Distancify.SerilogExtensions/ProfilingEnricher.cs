using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Distancify.SerilogExtensions
{
    /// <summary>
    /// This enricher adds an "ElapsedMs" property to all events containing the milliseconds between each log message.
    /// </summary>
    public class ProfilingEnricher : ILogEventEnricher
    {
        private DateTime _timestampStart;
        private DateTime _timestamp;
        private readonly string _name;

        public ProfilingEnricher(string name = null)
        {
            _timestamp = _timestampStart = DateTime.UtcNow;
            _name = name ?? "Default";
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var elapsed = (DateTime.UtcNow - _timestamp).TotalMilliseconds;
            var elapsedTotal = (DateTime.UtcNow - _timestampStart).TotalMilliseconds;
            _timestamp = DateTime.UtcNow;
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ElapsedMs", elapsed));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ElapsedMsTotal", elapsedTotal));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Profiler", _name));
        }
    }
}