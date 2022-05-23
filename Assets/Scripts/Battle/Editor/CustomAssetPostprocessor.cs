using Battle;
using Battle.BehaviorFuncs;
using UnityEditor;

public class CustomAssetPostprocessor : AssetPostprocessor {
  public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    foreach (string assetPath in importedAsset) {
      // behavior
      var behaviorGraph = AssetDatabase.LoadAssetAtPath<BehaviorGraph>(assetPath);
      if (behaviorGraph) {
        foreach (BehaviorNode node in behaviorGraph.nodes) {
          node.UpdateIndex();
        }
      }
    }
  }
}