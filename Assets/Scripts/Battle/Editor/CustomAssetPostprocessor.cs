using Battle;
using Battle.BehaviorFuncs;
using Battle.MagicFuncs;
using UnityEditor;

public class CustomAssetPostprocessor : AssetPostprocessor {
  public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
    foreach (string assetPath in importedAsset) {
      // behavior
      var behaviorTemplate = AssetDatabase.LoadAssetAtPath<BehaviorTemplate>(assetPath);
      if (behaviorTemplate) {
        behaviorTemplate.BehaviorId = behaviorTemplate.name;
        foreach (BehaviorNode node in behaviorTemplate.nodes) {
          node.UpdateIndex();
        }
      }
      // magic
      var magicTemplate = AssetDatabase.LoadAssetAtPath<MagicTemplate>(assetPath);
      if (magicTemplate) {
        magicTemplate.MagicId = magicTemplate.name;
      }
      // level
      var levelTemplate = AssetDatabase.LoadAssetAtPath<LevelTemplate>(assetPath);
      if (levelTemplate) {
        levelTemplate.LevelId = levelTemplate.name;
      }
      // buff
      var buffTemplate = AssetDatabase.LoadAssetAtPath<BuffTemplate>(assetPath);
      if (buffTemplate) {
        buffTemplate.BuffId = buffTemplate.name;
      }
      // unit
      var unitTemplate = AssetDatabase.LoadAssetAtPath<UnitTemplate>(assetPath);
      if (unitTemplate) {
        unitTemplate.UnitId = unitTemplate.name;
      }
      // attrib
      var attribTemplate = AssetDatabase.LoadAssetAtPath<AttribTemplate>(assetPath);
      if (attribTemplate) {
        attribTemplate.AttribId = attribTemplate.name;
      }
    }
  }
}