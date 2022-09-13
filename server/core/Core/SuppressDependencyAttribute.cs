using System;

namespace Core
{
    public class SuppressDependencyAttribute : Attribute
    {
        public SuppressDependencyAttribute(Type typeToSuppress)
        {
            FullName = typeToSuppress.FullName;
        }

        public string FullName { get; set; }
    }
}