using GameCore.UI;

namespace GameCore.AVGFuncs {
  [CreateNodeMenu("节点/效果/退出")]
  public class Exit : EffectNode {
    public async override void Run(AVG avg) {
      if (avg.OnExit == null) {
        await UIManager.Instance.Close(avg.UI);
      } else {
        avg.OnExit();
      }
    }
  }
}