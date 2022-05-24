using System;
using System.Collections.Generic;

namespace GameCore {
  [Serializable]
  public abstract class TargetSelector {
    public abstract void Select(Battle battle, Unit source, Unit target, List<Unit> result);
  }

  public class DefaultSelector : TargetSelector {
    public override void Select(Battle battle, Unit source, Unit target, List<Unit> result) => result.Add(target);
  }
}