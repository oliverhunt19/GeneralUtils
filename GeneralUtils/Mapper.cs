using System.Collections;

namespace GeneralUtils
{
    //https://github.com/farlee2121/BidirectionalMap/tree/main
    //https://stackoverflow.com/questions/10966331/two-way-bidirectional-dictionary-in-c
    public class Mapper<T1, T2> : IMapper<T1, T2>
        where T1 : notnull
        where T2 : notnull
    {
        private readonly Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private readonly Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Mapper()
        {
            Forward = new Indexer<T1, T2>(_forward);
            Reverse = new Indexer<T2, T1>(_reverse);
        }

        public Mapper(IEnumerable<KeyValuePair<T1, T2>> keyValuePairs) : this()
        {
            AddRange(keyValuePairs);
        }

        public int Count => _forward.Count;

        public Indexer<T1, T2> Forward { get; }
        public Indexer<T2, T1> Reverse { get; }


        public T1 this[T2 t2]
        {
            get => Reverse[t2];
        }

        public T2 this[T1 t1]
        {
            get => Forward[t1];
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
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void Remove(T1 t1)
        {
            T2 revKey = Forward[t1];
            _forward.Remove(t1);
            _reverse.Remove(revKey);
        }

        public void Remove(T2 t2)
        {
            T1 forwardKey = Reverse[t2];
            _reverse.Remove(t2);
            _forward.Remove(forwardKey);
        }

        public void Clear()
        {
            _reverse.Clear();
            _forward.Clear();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        public class Indexer<T3, T4>
            where T3 : notnull
        {
            private readonly Dictionary<T3, T4> _dictionary;

            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }

            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }

            public bool Contains(T3 key)
            {
                return _dictionary.ContainsKey(key);
            }
        }
    }

    public interface IMapper<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        public T1 this[T2 t2] { get; }

        public T2 this[T1 t1] { get; }

        public void Add(T1 key, T2 value);

        public void Remove(T1 t1);

        public void Remove(T2 t2);

        int Count { get; }

        void Clear();
    }

    public class ExtendedMapper<T1, T2> : IMapper<T1,T2>
        where T1 : notnull
        where T2 : notnull
    {
        //private readonly Dictionary<int, T2> _forward = new Dictionary<int, T2>();
        //private readonly Dictionary<int, T1> _reverse = new Dictionary<int, T1>();

        private readonly Func<T1,int> T1CustomHashCode;
        private readonly Func<T2,int> T2CustomHashCode;


        //public ExtendedMapper(IEnumerable<KeyValuePair<T1, T2>> keyValuePairs)
        //{
        //    AddRange(keyValuePairs);
        //}

        public ExtendedMapper(Func<T1, int> t1CustomHashCode, Func<T2, int> t2CustomHashCode)
        {
            T1CustomHashCode = t1CustomHashCode;
            T2CustomHashCode = t2CustomHashCode;
            Forward = new ExtendedIndexer<T1, T2>(t1CustomHashCode);
            Reverse = new ExtendedIndexer<T2, T1>(t2CustomHashCode);
        }

        public int Count => Forward.Count;

        public ExtendedIndexer<T1, T2> Forward { get; }
        public ExtendedIndexer<T2, T1> Reverse { get; }


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

    }

    public class ExtendedIndexer<T3,T4> : IEnumerable<KeyValuePair<T3, T4>>// : IDictionary<T3,T4>
        where T3 : notnull
    {
        private readonly Dictionary<int, T4> _dictionary;

        private readonly Dictionary<int, T3> _dicKey;
 
        private readonly Func<T3, int> _keyToHashCustom;

        public int Count => _dictionary.Count;

        public ICollection<T3> Keys => _dicKey.Values;

        public ICollection<T4> Values => _dictionary.Values;


        public ExtendedIndexer(Func<T3,int> KeyToHashCustom)
        {
            _keyToHashCustom = KeyToHashCustom;
            _dictionary = new Dictionary<int, T4>();
            _dicKey = new Dictionary<int, T3>();
        }

        public void Add(T3 key, T4 value)
        {
            int hashCode = _keyToHashCustom.Invoke(key);
            _dictionary.Add(hashCode, value);
            _dicKey.Add(hashCode,key);
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
            get { return _dictionary[_keyToHashCustom.Invoke( index)]; }
            //set 
            //{
            //    _dicKey[_keyToHashCustom.Invoke(index)] = index;
            //    _dictionary[_keyToHashCustom.Invoke(index)] = value; 
            //}
        }

        public bool Contains(T3 key)
        {
            return _dictionary.ContainsKey(_keyToHashCustom.Invoke( key));
        }


        public IEnumerator<KeyValuePair<T3, T4>> GetEnumerator()
        {
            List< KeyValuePair < T3,T4>> list = new List<KeyValuePair<T3, T4>>();
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
