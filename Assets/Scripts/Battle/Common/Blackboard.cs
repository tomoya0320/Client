using System.Collections.Generic;

namespace GameCore {
  public class Blackboard : Dictionary<string, float>, IPoolObject {
    public void Release() => Clear();
  }
}