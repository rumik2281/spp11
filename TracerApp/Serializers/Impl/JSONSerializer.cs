using System.Text.Json;
using TracerApp.Serializers;
using Tracer.result;
using TracerApp.Serializers;

namespace TracerApp.Serializers.Impl
{
    public class TraceResultJsonSerializer : AbstractSerializer<TraceResult>
    {
        public TraceResultJsonSerializer(SerializeOption serializeOption) : base(serializeOption) { }

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