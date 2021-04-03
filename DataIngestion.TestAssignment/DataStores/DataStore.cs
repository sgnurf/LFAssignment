using System.Collections.Generic;

namespace DataIngestion.TestAssignment.DataStores
{
    public delegate TKey GetKey<TKey, TValue>(TValue value);

    internal class DataStore<TKey, TValue> : IDataStore<TValue>, IDataProvider<TKey, TValue>
    {
        private readonly GetKey<TKey, TValue> keySelector;
        Dictionary<TKey, TValue> store = new Dictionary<TKey, TValue>();

        public DataStore(GetKey<TKey,TValue> keySelector)
        {
            this.keySelector = keySelector;
        }

        public virtual void Add(TValue item)
        {
            TKey key = keySelector(item);
            store[key] = item;
        }

        public void AddMany(IEnumerable<TValue> items)
        {
            foreach(TValue item in items)
            {
                Add(item);
            }
        }

        public IEnumerable<TValue> GetAll()
        {
            return store.Values;
        }

        public TValue GetById(TKey id)
        {
            return store.TryGetValue(id, out TValue value)
                ? value
                : default(TValue);
        }
    }
}