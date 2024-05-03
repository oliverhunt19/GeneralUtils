namespace GeneralUtils.Mapper
{
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
}
