using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Tracer;
using Tracer.result;

namespace TestTracer
{
    
    public class TimeTracerTest
    {
        private const int SleepTime = 40;
        private const int ThreadsCount = 3;

        private TimeTracer _tracer;

        [SetUp]
        public void SetupBeforeEachTest()
        {
            _tracer = new TimeTracer();
        }

        private void SingleMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(SleepTime);
            _tracer.StopTrace();
        }

        private void MethodWithInnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(SleepTime);
            SingleMethod();
            _tracer.StopTrace();
        }

        [Test]
        public void TestSingleMethod()
        {
            SingleMethod();
            TraceResult traceResult = _tracer.GetTraceResult();

            Assert.AreEqual(nameof(SingleMethod), traceResult.Threads[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(TimeTracerTest), traceResult.Threads[0].Methods[0].ClassName);
            Assert.AreEqual(0, traceResult.Threads[0].Methods[0].Methods.Count);
            Assert.IsTrue(traceResult.Threads[0].Methods[0].ElapsedTime >= SleepTime);
        }

        [Test]
        public void TestMethodWithInnerMethod()
        {
            MethodWithInnerMethod();
            TraceResult traceResult = _tracer.GetTraceResult();

            Assert.AreEqual(nameof(MethodWithInnerMethod), traceResult.Threads[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(SingleMethod), traceResult.Threads[0].Methods[0].Methods[0].MethodName);
            Assert.AreEqual(nameof(TimeTracerTest), traceResult.Threads[0].Methods[0].ClassName);
            Assert.AreEqual(1, traceResult.Threads[0].Methods[0].Methods.Count);
            Assert.AreEqual(0, traceResult.Threads[0].Methods[0].Methods[0].Methods.Count);
            Assert.IsTrue(traceResult.Threads[0].Methods[0].ElapsedTime >= SleepTime * 2);
        }

        [Test]
        public void TestSingleMethodInMultiThreads()
        {
            var threads = new List<Thread>();
            double expectedTotalElapsedTime = 0;

            for (int i = 0; i < ThreadsCount; i++)
            {
                var newThread = new Thread(SingleMethod);
                threads.Add(newThread);
                newThread.Start();
                expectedTotalElapsedTime += SleepTime;
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            double actualTotalElapsedTime = 0;

            foreach (var threadResult in _tracer.GetTraceResult().Threads)
            {
                actualTotalElapsedTime += threadResult.TotalElapsedTime;
            }

            Assert.IsTrue(actualTotalElapsedTime >= expectedTotalElapsedTime);
            Assert.AreEqual(ThreadsCount, _tracer.GetTraceResult().Threads.Count);
        }
    }
}