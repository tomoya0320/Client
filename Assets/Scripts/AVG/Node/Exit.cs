using GameCore.UI;
using Cysharp.Threading.Tasks;

namespace GameCore.AVGFuncs {
  [CreateNodeMenu("节点/效果/退出")]
  public class Exit : EffectNode {
    public override void Run(AVG avg) {
      UIManager.Instance.Close(avg.UI).Forget();
    }
  }
}