using System;

namespace TracerApp.Serializers
{
    public interface ISerializer<in T>
    {
        void Serialize(T value);
    }
}