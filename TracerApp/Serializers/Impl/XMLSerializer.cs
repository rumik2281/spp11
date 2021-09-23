using System.Linq;
using System.Xml.Linq;
using TracerApp.Serializers;
using Tracer;
using Tracer.result;
using TracerApp.Serializers;

namespace TracerApp.Serializers.Impl
{
    public class TraceResultXmlSerializer : AbstractSerializer<TraceResult>
    {
       
        public TraceResultXmlSerializer(SerializeOption serializeOption) : base(serializeOption) { }

        public override void Serialize(TraceResult data)
        {
            Option.Writer.WriteLine( new XDocument( new XElement("root",
                from thread in data.Threads select SerializeThreadInfo(thread))).ToString());
        }

        private XElement SerializeThreadInfo(ThreadTracer thread)
        {
            return new XElement("thread",
                new XAttribute("id", thread.Id),
                new XAttribute("time", thread.TotalElapsedTime + "ms"),
                from method in thread.Methods select SerializeMethodInfo(method));
        }

        private XElement SerializeMethodInfo(MethodTracer method)
        {
            var serializedMethod = new XElement("method",
                new XAttribute("name", method.MethodName),
                new XAttribute("time", method.ElapsedTime + "ms"),
                new XAttribute("class", method.ClassName));

            if (method.Methods.Count > 0)
            {
                serializedMethod.Add(from innerMethod in method.Methods select SerializeMethodInfo(innerMethod));
            }

            return serializedMethod;
        } 
    }
}