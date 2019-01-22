using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoCRUD.Interfaces;

namespace MongoCRUD
{
    public class Multiton : IEnumerable<ICrud>
    {
        private readonly ConcurrentDictionary<string, ICrud> _cruds;
        public bool Contains(string tableName)
        {
            return ((IDictionary) _cruds).Contains(tableName);
        }

        public void Add(ICrud crudObject)
        {
            ((IDictionary) _cruds).Add(crudObject.TableName, crudObject);
        }

        public void Remove(string key)
        {
            ((IDictionary) _cruds).Remove(key);
        }

        public ICrud this[string key]
        {
            get => (_cruds)[key];
            set => (_cruds)[key] = value;
        }

        public ICollection<string> Keys =>_cruds.Keys;

        public ICollection<ICrud> Values => _cruds.Values;

        public bool ContainsKey(string key)
        {
            return _cruds.ContainsKey(key);
        }

        public bool TryRemove(string key, out ICrud value)
        {
            return _cruds.TryRemove(key, out value);
        }

        public bool TryGetValue(string key, out ICrud value)
        {
            return _cruds.TryGetValue(key, out value);
        }

        public bool TryUpdate(string key, ICrud newValue, ICrud comparisonValue)
        {
            return _cruds.TryUpdate(key, newValue, comparisonValue);
        }

        public void Clear()
        {
            _cruds.Clear();
        }

        public KeyValuePair<string, ICrud>[] ToArray()
        {
            return _cruds.ToArray();
        }

        public Multiton()
        {
            _cruds = new ConcurrentDictionary<string, ICrud>();
        }
        public bool TryAdd(ICrud crudObject)
        {
            return _cruds.TryAdd(crudObject.TableName, crudObject);
        }

        public IEnumerator<ICrud> GetEnumerator()
        {
            return _cruds.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
