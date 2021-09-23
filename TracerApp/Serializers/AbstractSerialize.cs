namespace TracerApp.Serializers
{
    public abstract class AbstractSerialize<T> : ISerialize<T>
    {
        public SerializeOption Option { get; set; }
        
        protected AbstractSerialize(SerializeOption option)
        {
            Option = option;
        }
        
        public abstract void Serialize(T value);
        
    }
}