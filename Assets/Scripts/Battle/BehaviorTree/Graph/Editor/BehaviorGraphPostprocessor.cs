using UnityEditor;

namespace BehaviorTree.Battle {
  public class BehaviorGraphPostprocessor : AssetPostprocessor {
    public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
      foreach (string assetPath in importedAsset) {
        var behaviorGraph = AssetDatabase.LoadAssetAtPath<BehaviorGraph>(assetPath);
        if (behaviorGraph) {
          behaviorGraph.BehaviorId = behaviorGraph.name;
          foreach (BehaviorNode node in behaviorGraph.nodes) {
            node.UpdateIndexInEditor();
          }
        }
      }
    }
  }
}