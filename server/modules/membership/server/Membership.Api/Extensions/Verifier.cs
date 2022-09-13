using System;

namespace Membership.Api.Extensions
{
    public static class Verifier
    {
        public static void IsNotNull<T>(T o, string objectDescription = null) where T : class
        {
            if (o == null)
            {
                throw new ArgumentNullException(string.Format("\"{0}\" shoud not be null", objectDescription ?? typeof(T).Name));
            }
        }

        public static void IsEquals<T>(T value1, T value2, string message)
        {
            if (value1.Equals(value2))
            {
                throw new Exception(message);
            }
        }

        public static void IsNotNullOrEmpty(string o, string objectDescription = null)
        {
            if (string.IsNullOrEmpty(o))
            {
                throw new Exception(string.Format("{0} should not be empty", objectDescription ?? "String"));
            }
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception(message);
            }
        }
    }
}