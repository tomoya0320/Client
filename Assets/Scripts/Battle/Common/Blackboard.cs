using System.Collections.Generic;

namespace Battle {
  public class Blackboard : Dictionary<string, float>, IPoolObject {
    public void Release() => Clear();
  }
}