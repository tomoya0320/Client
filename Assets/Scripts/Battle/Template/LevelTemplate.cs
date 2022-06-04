using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/关卡")]
  public class LevelTemplate : ScriptableObject {
    [LabelText("关卡行为树Id列表")]
    public string[] BehaviorIds;
    [LabelText("玩家数据列表")]
    public PlayerData[] PlayerData;
  }
}