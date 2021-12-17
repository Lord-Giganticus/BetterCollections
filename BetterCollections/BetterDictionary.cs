using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Timers;

namespace BetterCollections
{
    /// <summary>
    /// <inheritdoc cref="Dictionary{TKey, TValue}"/>
    /// </summary>
    public class BetterDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        #region Constructors
        public BetterDictionary() : base()
        {
        }

        public BetterDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public BetterDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        public BetterDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        public BetterDictionary(int capacity) : base(capacity)
        {
        }

        public BetterDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        protected BetterDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion

        #region Events
        public event ItemDelegate Item_Added;

        public event ItemDelegate Item_Removed;

        public event ItemChangedDelegate Item_Changed;
        #endregion

        #region Methods
        public new TValue this[TKey key]
        {
            get => GetItem(key);
            set => SetItem(key, value);
        }

        public int[] this[TValue value]
        {
            get
            {
                IEnumerable<int> arr()
                {
                    var values = Values.ToArray();
                    for (int i = 0; i < values.Length; i++)
                        if (values[i].Equals(value))
                            yield return i;
                }
                return arr().ToArray();
            }
        }

        protected TValue GetItem(TKey key)
        {
            return base[key];
        }

        protected void SetItem(TKey key, TValue value)
        {
            var original = this[key];
            base[key] = value;
            Item_Changed?.Invoke(new KeyValuePair<TKey, TValue>(key, original), new KeyValuePair<TKey, TValue>(key, value));
        }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            Item_Added?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
        }

        public new bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                var value = this[key];
                base.Remove(key);
                Item_Removed?.Invoke(new KeyValuePair<TKey, TValue>(key, value));
                return true;
            }
            return false;
        }
        #endregion

        #region Casting
        public static explicit operator MultiKeyDict<TKey, TValue>(BetterDictionary<TKey, TValue> dict) =>
            new MultiKeyDict<TKey, TValue>(dict);
        #endregion

        #region Delegates
        public delegate void ItemDelegate(KeyValuePair<TKey, TValue> item);

        public delegate void ItemChangedDelegate(KeyValuePair<TKey, TValue> original, KeyValuePair<TKey, TValue> newitem);
        #endregion
    }
}
