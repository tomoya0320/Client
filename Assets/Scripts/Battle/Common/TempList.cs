using System.Collections.Generic;

namespace GameCore {
  public static class TempList<T> {
    private static List<T> List = new List<T>();

    public static List<T> Get() => List;

    public static void CleanUp() => List.Clear();
  }
}