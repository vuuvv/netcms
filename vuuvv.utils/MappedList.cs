using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.utils
{
    public class MappedList<T>
        where T : IMapped
    {
        private OrderedDictionary<string, T> _dict = new OrderedDictionary<string, T>();

        public int Add(T data)
        {
            return _dict.Add(data.GetKey(), data);
        }

        public void Insert(int index, T data) 
        {
            _dict.Insert(index, data.GetKey(), data);
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<string> Keys
        {
            get { return _dict.Keys; }
        }

        public ICollection<T> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        public int IndexOfKey(string key)
        {
            return _dict.IndexOfKey(key);
        }

        public bool Remove(string key)
        {
            return _dict.Remove(key);
        }

        public T this[string key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                _dict[key] = value;
            }
        }

        public T this[int index]
        {
            get
            {
                return _dict[index];
            }
        }
    }
}
