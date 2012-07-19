using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.utils
{
    public class MappedList<TKey, TValue>
        where TValue : IMapped<TKey>
    {
        private OrderedDictionary<TKey, TValue> _dict = new OrderedDictionary<TKey, TValue>();

        public int Add(TValue data)
        {
            return _dict.Add(data.GetKey(), data);
        }

        public void Insert(int index, TValue data) 
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

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public ICollection<TKey> Keys
        {
            get { return _dict.Keys; }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        public int IndexOfKey(TKey key)
        {
            return _dict.IndexOfKey(key);
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public TValue this[TKey key]
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

        public TValue this[int index]
        {
            get
            {
                return _dict[index];
            }
        }
    }
}
