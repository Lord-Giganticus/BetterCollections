using System.Collections.Generic;

namespace BetterCollections
{
    public static class DictUtil
    {
        public static Dictionary<TKeyN, TValueN> Change<TKey, TValue, TKeyN, TValueN>(this Dictionary<TKey, TValue> dict, BetterDictionary<TKey, TValue>.ChangeDelegate<TKeyN, TValueN> func)
        {
            var cast = (BetterDictionary<TKey, TValue>)dict;
            return cast.Change(func);
        }

        public static long GetAllHashCode<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            long res= 0;
            var hashes = dict.Change(x => new KeyValuePair<int, int>(x.Key.GetHashCode(), x.Value.GetHashCode()));
            foreach (var hash in hashes)
            {
                res += hash.Key;
                res += hash.Value;
            }
            return res;
        }
    }
}
