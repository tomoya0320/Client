using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/关卡")]
  public class LevelTemplate : SerializedScriptableObject {
    [LabelText("关卡行为树列表")]
    public AssetReferenceT<BehaviorGraph>[] Behaviors;
    [LabelText("玩家数据列表")]
    public PlayerData[] PlayerData;
  }
}