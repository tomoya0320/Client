using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle {
  public struct EffectArgs {
    public bool IsEnd;
    public Unit Source;
    public Unit Target;
  }

  public abstract class EffectAction : ScriptableObject {
    public abstract bool IgnoreOnEnd { get; }

    public abstract void Run(BattleManager battleManager, Context context, EffectArgs args);
  }
}