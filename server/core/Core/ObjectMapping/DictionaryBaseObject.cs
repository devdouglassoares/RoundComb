using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;

namespace Core.ObjectMapping
{
    public class DictionaryBaseObject : DynamicObject
    {
        private readonly IDictionary<string, string> _dictionary;

        public DictionaryBaseObject()
        {
            _dictionary = new ConcurrentDictionary<string, string>();
        }

        public DictionaryBaseObject(string[] keys)
        {
            _dictionary = new ConcurrentDictionary<string, string>();
            foreach (var key in keys)
            {
                _dictionary[key] = string.Empty;
            }
        }

        public IEnumerable<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _dictionary.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_dictionary.ContainsKey(binder.Name))
            {
                _dictionary[binder.Name] = String.Empty;
                result = string.Empty;
                return true;
            }

            var zoneObject = _dictionary[binder.Name];
            if (zoneObject != null)
            {
                result = zoneObject;
                return true;
            }
            result = null;
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value.ToString();
            return true;
        }


        public object this[string key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                _dictionary[key] = value.ToString();
            }
        }
    }
}
