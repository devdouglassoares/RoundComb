using System;

namespace Core.DynamicProperties.Exceptions
{
    public class DynamicPropertyAlreadyExistException<T>: InvalidOperationException
    {
        public DynamicPropertyAlreadyExistException()
            : base($"The DynamicProperty links to type {typeof (T).FullName} already exists")
        {
            
        }

        public DynamicPropertyAlreadyExistException(string msg): base (msg)
        {
            
        }
    }
}