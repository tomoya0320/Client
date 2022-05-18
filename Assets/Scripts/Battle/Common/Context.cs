using System.Collections;
using System.Collections.Generic;

namespace Battle {
  public abstract class Context : IPoolObject {
    public abstract void Release();
  }

  public class BuffContext : Context {
    public int BuffRuntimeId;
    public int BehaviorRuntimeId;

    public override void Release() {
      BuffRuntimeId = 0;
      BehaviorRuntimeId = 0;
    }
  }
}