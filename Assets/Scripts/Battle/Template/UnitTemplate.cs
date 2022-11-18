using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/单位")]
  public class UnitTemplate : SerializedScriptableObject {
    public string Name;
    [LabelText("属性")]
    public AssetReferenceT<AttribTemplate> Attrib;
    public int MaxLevel;
    [LabelText("单位行为树列表")]
    public AssetReferenceT<BehaviorGraph>[] Behaviors;
  }
}