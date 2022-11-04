using System.Collections.Generic;

namespace GameCore {
  public static class TempList<T> {
    private static readonly Stack<List<T>> ListCache = new Stack<List<T>>();

    public static List<T> Get() {
      if (!ListCache.TryPop(out var list)) {
        list = new List<T>();
      }
      return list;
    }

    public static void Release(List<T> list) {
      list.Clear();
      ListCache.Push(list);
    }
  }
}