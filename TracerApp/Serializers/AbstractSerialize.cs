namespace TracerApp.Serializers
{
    public abstract class AbstractSerializer<T> : ISerializer<T>
    {
        public SerializeOption Option { get; set; }
        
        protected AbstractSerializer(SerializeOption option)
        {
            Option = option;
        }
        
        public abstract void Serialize(T value);
        
    }
}