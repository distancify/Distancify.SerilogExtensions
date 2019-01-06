using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Distancify.SerilogExtensions
{
    public interface IProfilingLogger : ILogger
    {
        void ResetTimer();
    }
}
