using System;

namespace TracerApp.Serializers
{
    public interface ISerialize<in T>
    {
        void Serialize(T value);
    }
}