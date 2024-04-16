using System.Collections;

namespace GeneralUtils.Mapper
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
            foreach (var item in keyValuePairs)
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

    

    


}
