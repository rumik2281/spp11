using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Tracer.result;

namespace Tracer
{
    public class TimeTracer : ITracer
    {
        private static readonly object ThreadLocker = new object();

        private readonly Dictionary<int, ThreadTracer> _threads;

        public TimeTracer()
        {
            _threads = new Dictionary<int, ThreadTracer>();
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_threads);
        }

        public void StartTrace()
        {
            var methodBase = new StackTrace().GetFrame(1).GetMethod();
            var methodTracer = new MethodTracer(methodBase.ReflectedType.Name, methodBase.Name);
            var threadTracer = GetThreadTracer(Thread.CurrentThread.ManagedThreadId);

            threadTracer.StartTrace(methodTracer);
        }

        public void StopTrace()
        {
            GetThreadTracer(Thread.CurrentThread.ManagedThreadId).StopTrace();
        }

        private ThreadTracer GetThreadTracer(int id)
        {
            lock (ThreadLocker)
            {
                if (!_threads.TryGetValue(id, out ThreadTracer thread))
                {
                    thread = new ThreadTracer(id);
                    _threads.Add(id, thread);
                }

                return thread;
            }
        }
    }
}