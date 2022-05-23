using UnityEngine;
using Sirenix.OdinInspector;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/单位")]
  public class UnitTemplate : ScriptableObject {
    public string Name;
    public string AttribId;
    public int MaxLevel;
  }
}