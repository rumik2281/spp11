using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Tracer;

namespace Tracer.result
{
    [Serializable]
    public class TraceResult
    {
        [JsonPropertyName("threads")]
        public List<ThreadTracer> Threads { get; }

        public TraceResult() { }

        public TraceResult(Dictionary<int, ThreadTracer> threads)
        {
            Threads = new List<ThreadTracer>();

            foreach (var thread in threads)
            {
                Threads.Add(thread.Value.GetTraceResult());
            }
        }
    }
}