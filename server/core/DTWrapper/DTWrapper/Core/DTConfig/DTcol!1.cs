namespace DTWrapper.Core.DTConfig
{
    using DTWrapper.Core.DTConfig.FluentInterfaces;
    using System;
    using System.Runtime.CompilerServices;

    public class DTcol<T>
    {
        private readonly DTcolFluentInterface<T> set;

        public DTcol()
        {
            this.set = new DTcolFluentInterface<T>((DTcol<T>) this);
        }

        public bool Flag { get; set; }

        public string Name { get; set; }

        public string Property { get; set; }

        public DTcolFluentInterface<T> Set
        {
            get
            {
                return this.set;
            }
        }

        public System.Type Type { get; set; }
    }
}

