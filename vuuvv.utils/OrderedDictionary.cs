using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.utils
{
    class OrderedDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dict;
        private List<TKey> _list;

        public OrderedDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
            _list = new List<TKey>();
        }

        public void Insert(int index, TKey key, TValue value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException("index");
            _dict.Add(key, value);
            _list.Insert(index, key);
        }

        public int Add(TKey key, TValue value)
        {
            _dict.Add(key, value);
            _list.Add(key);
            return Count - 1;
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Clear()
        {
            _dict.Clear();
            _list.Clear();
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
            get { return _list; }
        }

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                foreach (TKey key in _list)
                {
                    values.Add(_dict[key]);
                }
                return values;
            }
        }

        public int IndexOfKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            for (int index = 0; index < _list.Count; index++)
            {
                if (_list[index].Equals(key))
                    return index;
            }

            return -1;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (!_dict.ContainsKey(key))
            {
                return false;
            }

            int index = IndexOfKey(key);
            if (index == -1)
                throw new InvalidOperationException("The OrderedDictionary object is not valid");
            _dict.Remove(key);
            _list.RemoveAt(index);
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                if (_dict.ContainsKey(key))
                {
                    _dict[key] = value;
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public TValue this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw new ArgumentOutOfRangeException("index");
                return _dict[_list[index]];
            }
        }
    }
}
