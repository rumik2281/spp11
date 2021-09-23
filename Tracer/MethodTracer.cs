using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Tracer
{
    public class MethodTracer
    {
        [JsonPropertyName("name")]
        public string MethodName { get; private set; }

        [JsonPropertyName("class")]
        public string ClassName { get; private set; }

        [JsonPropertyName("time")]
        public double ElapsedTime { get; private set; }

        [JsonPropertyName("methods")]
        public List<MethodTracer> Methods { get; internal set; }

        private readonly Stopwatch Stopwatch;

        public MethodTracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            Methods = new List<MethodTracer>();
            Stopwatch = new Stopwatch();
        }

        internal MethodTracer GetTraceResult()
        {
            var result = new MethodTracer(ClassName, MethodName);
            result.ElapsedTime = ElapsedTime;

            foreach (var method in Methods)
            {
                result.Methods.Add(method.GetTraceResult());
            }

            return result;
        }

        internal void StartTrace()
        {
            Stopwatch.Start();
        }

        internal void StopTrace()
        {
            Stopwatch.Stop();
            ElapsedTime = Stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}