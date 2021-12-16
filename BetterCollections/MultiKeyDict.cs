using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterCollections
{
    /// <summary>
    /// <inheritdoc cref="BetterList{T}"/>
    /// </summary>
    public class MultiKeyDict<TKey, TValue> : BetterList<KeyValuePair<TKey, TValue>>
    {
        #region Constructors
        public MultiKeyDict() : base()
        {
            Keys = new KeyCollection();
            Values = new ValueCollection();
            Item_Added += (x, y) =>
            {
                Keys.Add(x.Key);
                Values.Add(x.Value);
            };
            Item_Changed += (x, y, i) =>
            {
                Keys[i] = y.Key;
                Values[i] = y.Value;
            };
            Item_Removed += (x) =>
            {
                Keys.Remove(x.Key);
                Values.Remove(x.Value);
            };
        }

        public MultiKeyDict(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
            Keys = new KeyCollection(collection.Select(x => x.Key));
            Values = new ValueCollection(collection.Select(x => x.Value));
            Item_Added += (x, y) =>
            {
                Keys.Add(x.Key);
                Values.Add(x.Value);
            };
            Item_Changed += (x, y, i) =>
            {
                Keys[i] = y.Key;
                Values[i] = y.Value;
            };
            Item_Removed += (x) =>
            {
                Keys.Remove(x.Key);
                Values.Remove(x.Value);
            };
        }

        public MultiKeyDict(int capacity) : base(capacity)
        {
            Keys = new KeyCollection(capacity);
            Values = new ValueCollection(capacity);
            Item_Added += (x, y) =>
            {
                Keys.Add(x.Key);
                Values.Add(x.Value);
            };
            Item_Changed += (x, y, i) =>
            {
                Keys[i] = y.Key;
                Values[i] = y.Value;
            };
            Item_Removed += (x) =>
            {
                Keys.Remove(x.Key);
                Values.Remove(x.Value);
            };
        }
        #endregion

        #region Sub Types
        public sealed class KeyCollection : BetterList<TKey>
        {
            public KeyCollection() : base()
            {
            }

            public KeyCollection(IEnumerable<TKey> collection) : base(collection)
            {
            }

            public KeyCollection(int capacity) : base(capacity)
            {
            }
        }

        public sealed class ValueCollection : BetterList<TValue>
        {
            public ValueCollection() : base()
            {
            }

            public ValueCollection(IEnumerable<TValue> collection) : base(collection)
            {
            }

            public ValueCollection(int capacity) : base(capacity)
            {
            }
        }
        #endregion

        #region Properties
        public KeyCollection Keys { get; protected set; }

        public ValueCollection Values { get; protected set; }
        #endregion

        #region Methods
        public virtual TValue[] this[TKey key] =>
            this.Where(x => x.Key.Equals(key)).Select(x => x.Value).ToArray();

        protected virtual KeyValuePair<TKey, TValue> Create(TKey key, TValue value) =>
            new KeyValuePair<TKey, TValue>(key, value);

        public virtual void Add(TKey key, TValue value)
        {
            Add(Create(key, value));
        }

        public virtual void Insert(int index, TKey key, TValue value)
        {
            Insert(index, Create(key, value));
        }

        public virtual bool TKeyHasIDisposeable()
        {
            return HasIDisposeable<TKey>();
        }

        public virtual bool TValueHasIDisposeable()
        {
            return HasIDisposeable<TValue>();
        }

        public virtual MultiKeyDict<TKeyO, TValueO> Change<TKeyO, TValueO>(ChangeDelegate<KeyValuePair<TKeyO, TValueO>> func)
        {
            return new MultiKeyDict<TKeyO, TValueO>(base.Change(func));
        }
        #endregion

        #region Casting
        public static explicit operator KeyValuePair<TKey, TValue>[](MultiKeyDict<TKey, TValue> dict)
        {
            return dict.ToArray();
        }

        public static implicit operator Dictionary<TKey, TValue>(MultiKeyDict<TKey, TValue> dict)
        {
            var res = new Dictionary<TKey, TValue>();
            foreach (var pair in dict)
                if (!res.ContainsKey(pair.Key))
                    res.Add(pair.Key, pair.Value);
            return res;
        }

        public static implicit operator MultiKeyDict<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            return new MultiKeyDict<TKey, TValue>(dict);
        }
        #endregion
    }
}
