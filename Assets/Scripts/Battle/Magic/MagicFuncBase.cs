using UnityEngine;

namespace Battle.MagicFuncs {
  public struct MagicArgs {
    public bool IsEnd;
    public Unit Source;
    public Unit Target;
  }

  public abstract class MagicFuncBase : ScriptableObject {
    public abstract bool IgnoreOnEnd { get; }

    public abstract void Run(BattleManager battleManager, Context context, MagicArgs args);
  }
}