using Battle;
using Battle.BehaviorFuncs;
using Battle.MagicFuncs;
using UnityEditor;

public class CustomAssetPostprocessor : AssetPostprocessor {
  public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    foreach (string assetPath in importedAsset) {
      // behavior
      var behaviorGraph = AssetDatabase.LoadAssetAtPath<BehaviorGraph>(assetPath);
      if (behaviorGraph) {
        behaviorGraph.BehaviorId = behaviorGraph.name;
        foreach (BehaviorNode node in behaviorGraph.nodes) {
          node.UpdateIndex();
        }
      }
      // magic
      var magicAction = AssetDatabase.LoadAssetAtPath<MagicFuncBase>(assetPath);
      if (magicAction) {
        magicAction.MagicId = magicAction.name;
      }
    }
  }
}