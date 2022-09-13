using System;

namespace Core.Exceptions
{
    public class BaseNotFoundException<T> : Exception where T : class
    {
        public BaseNotFoundException() : base("The requested " + typeof(T).Name + " could not be found")
        {

        }
    }
}
