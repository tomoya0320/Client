using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/Buff")]
  public class BuffTemplate : ScriptableObject {
    public string MagicTemplateId;
    public int Duration;
  }
}