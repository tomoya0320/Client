using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/单位")]
  public class UnitTemplate : SerializedScriptableObject {
    public string Name;
    [LabelText("预制体")]
    public AssetReferenceT<GameObject> Prefab;
    [LabelText("属性")]
    public AssetReferenceT<AttribTemplate> Attrib;
    [LabelText("最大等级")]
    public int MaxLevel;
    [LabelText("死亡动画时间")]
    public float DieAnimTime;
    [LabelText("单位行为树列表")]
    public AssetReferenceT<BehaviorGraph>[] Behaviors;
  }
}