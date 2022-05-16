using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorWASMDemo.Shared.Extensions
{
    public static class LoggerExtensionMethods
    {
        public static void CaptureExecutionTimeAsTrace(this ILogger logger, string message, Action action)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                watch.Stop();

                string duration = $"{message} executed in {watch.ElapsedMilliseconds} ms";
                logger.LogInformation(duration);
            }
        }
    }
}
