using System.Collections;

namespace GeneralUtils.Mapper
{
    public class ExtendedMapper<T1, T2> : IMapper<T1, T2>
        where T1 : notnull
        where T2 : notnull
    {

        public ExtendedMapper(Func<T1, int> t1CustomHashCode, Func<T2, int> t2CustomHashCode)
        {
            Forward = new ExtendedIndexer<T1, T2>(t1CustomHashCode);
            Reverse = new ExtendedIndexer<T2, T1>(t2CustomHashCode);
        }

        public int Count => Forward.Count;

        private ExtendedIndexer<T1, T2> Forward { get; }
        private ExtendedIndexer<T2, T1> Reverse { get; }


        public T1 this[T2 t2]
        {
            get
            {
                return Reverse[t2];
            }
        }

        public T2 this[T1 t1]
        {
            get
            {
                return Forward[t1];
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<T1, T2>> keyValuePairs)
        {
            foreach(var item in keyValuePairs)
            {
                Add(item.Key, item.Value);
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            Forward.Add(t1, t2);
            Reverse.Add(t2, t1);
        }

        public void Remove(T1 t1)
        {
            T2 revKey = Forward[t1];
            Forward.Remove(t1);
            Reverse.Remove(revKey);
        }

        public void Remove(T2 t2)
        {
            T1 forwardKey = Reverse[t2];
            Reverse.Remove(t2);
            Forward.Remove(forwardKey);
        }

        public void Clear()
        {
            Reverse.Clear();
            Forward.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return Forward.GetEnumerator();
        }

        private class ExtendedIndexer<T3, T4> : IEnumerable<KeyValuePair<T3, T4>>// : IDictionary<T3,T4>
        where T3 : notnull
        {
            private readonly Dictionary<int, T4> _dictionary;

            private readonly Dictionary<int, T3> _dicKey;

            private readonly Func<T3, int> _keyToHashCustom;

            public int Count => _dictionary.Count;

            public ICollection<T3> Keys => _dicKey.Values;

            public ICollection<T4> Values => _dictionary.Values;


            public ExtendedIndexer(Func<T3, int> KeyToHashCustom)
            {
                _keyToHashCustom = KeyToHashCustom;
                _dictionary = new Dictionary<int, T4>();
                _dicKey = new Dictionary<int, T3>();
            }

            public void Add(T3 key, T4 value)
            {
                int hashCode = _keyToHashCustom.Invoke(key);
                _dictionary.Add(hashCode, value);
                _dicKey.Add(hashCode, key);
            }

            public bool Remove(T3 key)
            {
                int hashCode = _keyToHashCustom.Invoke(key);
                return _dictionary.Remove(hashCode) &&
                _dicKey.Remove(hashCode);
            }

            public void Clear()
            {
                _dicKey.Clear();
                _dictionary.Clear();
            }

            public T4 this[T3 index]
            {
                get { return _dictionary[_keyToHashCustom.Invoke(index)]; }
                //set 
                //{
                //    _dicKey[_keyToHashCustom.Invoke(index)] = index;
                //    _dictionary[_keyToHashCustom.Invoke(index)] = value; 
                //}
            }

            public bool Contains(T3 key)
            {
                return _dictionary.ContainsKey(_keyToHashCustom.Invoke(key));
            }


            public IEnumerator<KeyValuePair<T3, T4>> GetEnumerator()
            {
                List<KeyValuePair<T3, T4>> list = new List<KeyValuePair<T3, T4>>();
                foreach(int key in _dicKey.Keys)
                {
                    list.Add(new KeyValuePair<T3, T4>(_dicKey[key], _dictionary[key]));
                }
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

    }
}
