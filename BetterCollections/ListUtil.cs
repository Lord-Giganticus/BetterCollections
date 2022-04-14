using System.Collections.Generic;

namespace BetterCollections
{
    public static class ListUtil
    {
        public static List<TNew> Change<TOld, TNew>(this List<TOld> list, BetterList<TOld>.ChangeDelegate<TNew> func)
        {
            var cast = (BetterList<TOld>)list;
            return cast.Change(func);
        }

        public static long GetAllHashCode<T>(this List<T> list)
        {
            long res = 0;
            var hashs = list.Change(x => x.GetHashCode());
            hashs.ForEach(x => res += x);
            return res;
        }
    }
}
