using System.Collections.Generic;
using System;
using System.Linq;
using System.Timers;

namespace BetterCollections
{
    /// <summary>
    /// <inheritdoc cref="List{T}"/>
    /// </summary>
    public class BetterList<T> : List<T>
    {
        #region Constructors
        public BetterList() : base()
        {
        }

        public BetterList(IEnumerable<T> collection) : base(collection)
        {
        }

        public BetterList(int capacity) : base(capacity)
        {
        }
        #endregion

        #region Events
        public event ItemAddedDelegate Item_Added;

        public event ItemRemovedDelegate Item_Removed;

        public event ItemChangedDelegate Item_Changed;
        #endregion

        #region Methods
        public virtual new T this[int index]
        {
            get => GetItem(index);
            set => SetItem(value, index);
        }

        public virtual int[] this[T item]
        {
            get
            {
                IEnumerable<int> arr()
                {
                    for (int i = 0; i < Count; i++)
                        if (this[i].Equals(item))
                            yield return i;
                }
                return arr().ToArray();
            }
        }

        
        protected virtual T GetItem(int index)
        {
            return base[index];
        }

        protected virtual void SetItem(T item, int index)
        {
            var ogitem = this[index];
            base[index] = item;
            Item_Changed?.Invoke(ogitem, item, index);
        }

        public virtual new void Add(T item)
        {
            base.Add(item);
            var index = this[item].Last();
            Item_Added?.Invoke(item, index);
        }

        public virtual new void Insert(int index, T item)
        {
            base.Insert(index, item);
            Item_Added?.Invoke(item, index);
        }

        public virtual new void InsertRange(int index, IEnumerable<T> collection)
        {
            var arr = collection.ToArray();
            for (int i = index; i < arr.Length; i++)
                Insert(i, arr[i]);
        }

        public virtual new bool Remove(T item)
        {
            var res = base.Remove(item);
            if (res)
                Item_Removed?.Invoke(item);
            return res;
        }

        public virtual new int RemoveAll(Predicate<T> match)
        {
            int res = 0;
            foreach (T item in this)
                if (match.Invoke(item))
                {
                    Remove(item);
                    res++;
                }
            return res;
        }

        public virtual new void RemoveRange(int index, int count)
        {
            for (int i = index; i < index + Count + 1 && index < Count; i++)
                Remove(this[i]);
        }

        protected virtual BetterDictionary<int, T> ToDict()
        {
            var res = new BetterDictionary<int, T>();
            for (int i = 0; i < Count; i++)
                res.Add(i, this[i]);
            return res;
        }

        public static bool HasIDisposeable<TCheck>()
        {
            return typeof(TCheck).GetInterfaces().Contains(typeof(IDisposable));
        }

        public virtual BetterList<TResult> Change<TResult>(ChangeDelegate<TResult> func)
        {
            var res = new BetterList<TResult>();
            ForEach(x => res.Add(func.Invoke(x)));
            return res;
        }

        #endregion

        #region Castings
        public static explicit operator T[](BetterList<T> list) => list.ToArray();

        public static implicit operator BetterDictionary<int, T>(BetterList<T> list) => list.ToDict();

        public static implicit operator BetterList<T>(T[] arr) => new BetterList<T>(arr);
        #endregion

        #region Delegates
        public delegate void ItemAddedDelegate(T item, int index);

        public delegate void ItemRemovedDelegate(T item);

        public delegate void ItemChangedDelegate(T original, T newitem, int index);

        public delegate TResult ChangeDelegate<out TResult>(T item);
        #endregion
    }
}
