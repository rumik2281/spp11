using System;
using System.IO;
using System.Threading;
using TracerApp.Serializers;
using TracerApp.Serializers.Impl;
using TracerApp.Serializers;
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

        static void Main()
        {
            var filePath = "/Users/vladislavmajskij/Desktop/";
            Thread thread1 = new Thread(Method1);
            Thread thread2 = new Thread(Method1);

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            
            _serializer = new TraceResultJsonSerializer(new SerializeOption(Console.Out, true));
            _serializer.Serialize(tracer.GetTraceResult());

            using (var fs = new FileStream(filePath + "test.json", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                _serializer.Option = new SerializeOption(sw, true);
                _serializer.Serialize(tracer.GetTraceResult());
            }
            
            Console.WriteLine();
            _serializer = new TraceResultXmlSerializer(new SerializeOption(Console.Out, true));
            _serializer.Serialize(tracer.GetTraceResult());

            using (var fs = new FileStream(filePath + "test.xml", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                _serializer.Option = new SerializeOption(sw, true);
                _serializer.Serialize(tracer.GetTraceResult());
            }

            Console.ReadKey();
        }
    }
}