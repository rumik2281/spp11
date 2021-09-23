using System.IO;

namespace TracerApp.Serializers
{
    public class SerializeOption
    {

        public readonly TextWriter Writer;

        public readonly bool WriteIndented;
        
        public SerializeOption(TextWriter writer, bool writeIndented = false)
        {
            Writer = writer;
            WriteIndented = writeIndented;
        }

    }
}