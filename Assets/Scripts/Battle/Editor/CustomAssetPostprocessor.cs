using UnityEditor;
using XNode;

public class CustomAssetPostprocessor : AssetPostprocessor {
  public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    foreach (string assetPath in importedAsset) {
      var graph = AssetDatabase.LoadAssetAtPath<NodeGraph>(assetPath);
      if (graph) {
        foreach (Node node in graph.nodes) {
          node.UpdateIndex();
        }
      }
    }
  }
}