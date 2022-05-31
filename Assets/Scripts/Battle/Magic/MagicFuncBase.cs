using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.MagicFuncs {
  public struct MagicArgs {
    public bool IsEnd;
    public Unit Source;
    public Unit Target;
  }

  public abstract class MagicFuncBase : ScriptableObject {
    public abstract bool IgnoreOnEnd { get; }

    public abstract UniTask Run(Battle battleManager, Context context, MagicArgs args);
  }
}