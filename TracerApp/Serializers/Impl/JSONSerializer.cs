using System.Text.Json;
using Tracer.result;

namespace TracerApp.Serializers.Impl
{
    public class JSONSerializer : AbstractSerialize<TraceResult>
    {
        public JSONSerializer(SerializeOption serializeOption) : base(serializeOption) { }

        public override void Serialize(TraceResult data)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = Option.WriteIndented
            };
            var jsonString = JsonSerializer.Serialize(data, options);
            
            Option.Writer.WriteLine(jsonString);
        }

    }
}