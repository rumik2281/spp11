using System;
using System.IO;
using System.Threading;
using TracerApp.Serializers;
using TracerApp.Serializers.Impl;
using Tracer;
using Tracer.result;


namespace TracerApp
{
    class Program
    {
        private static TimeTracer tracer = new TimeTracer();
        private static AbstractSerializer<TraceResult> _serializer;

        static void Method1()
        {
            tracer.StartTrace();
            Method2();
            Method3();
            Thread.Sleep(14);
            tracer.StopTrace();
        }

        static void Method2()
        {
            tracer.StartTrace();
            Method4();
            Thread.Sleep(16);
            tracer.StopTrace();
        }

        static void Method3()
        {
            tracer.StartTrace();
            Thread.Sleep(11);
            tracer.StopTrace();
        }

        static void Method4()
        {
            tracer.StartTrace();
            Thread.Sleep(12);
            tracer.StopTrace();
        }

        private static void StartThreads()
        {
            var thread1 = new Thread(Method1);
            var thread2 = new Thread(Method1);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        private static void SerializeToJson(string filePath)
        {
            _serializer = new JSONSerializer(new SerializeOption(Console.Out, true));
            _serializer.Serialize(tracer.GetTraceResult());

            using var fs = new FileStream(filePath + "test.json", FileMode.Create);
            using var sw = new StreamWriter(fs);
            _serializer.Option = new SerializeOption(sw, true);
            _serializer.Serialize(tracer.GetTraceResult());
        }

        private static void SerializeToXml(string filePath)
        {
            _serializer = new XMLSerializer(new SerializeOption(Console.Out, true));
            _serializer.Serialize(tracer.GetTraceResult());

            using var fs = new FileStream(filePath + "test.xml", FileMode.Create);
            using var sw = new StreamWriter(fs);
            _serializer.Option = new SerializeOption(sw, true);
            _serializer.Serialize(tracer.GetTraceResult());
        }
        static void Main()
        {
            const string filePath = "/Users/KIRYL/Desktop/";
            StartThreads();
            SerializeToJson(filePath);
            Console.WriteLine();
            SerializeToXml(filePath);
            Console.ReadKey();
        }
    }
}