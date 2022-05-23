using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle {
  [CreateAssetMenu(menuName = "模板/关卡")]
  public class LevelTemplate : ScriptableObject {
    [ReadOnly]
    public string LevelId;
    public string[] BehaviorIds;
    public PlayerData[] EnemyData;
  }
}