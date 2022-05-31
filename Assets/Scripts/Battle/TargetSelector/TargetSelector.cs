using System.Collections.Generic;

namespace GameCore {
  public abstract class TargetSelector {
    public abstract void Select(Battle battle, Unit source, Unit mainTarget, List<Unit> result);
  }

  public class DefaultSelector : TargetSelector {
    public override void Select(Battle battle, Unit source, Unit mainTarget, List<Unit> result) => result.Add(mainTarget);
  }
}