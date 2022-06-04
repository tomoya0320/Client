using GameCore.BehaviorFuncs;
using System;
using UnityEditor;
using XNodeEditor;

namespace GameCore {
  [CustomNodeGraphEditor(typeof(BehaviorGraph))]
  public class BehaviorGraphEditor : NodeGraphEditor {
    public override void OnGUI() {
      window.titleContent.text = target.name;
    }

    /// <summary> 
    /// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
    /// In this example we are sorting out all node types that are not in the XNode.Examples namespace.
    /// </summary>
    public override string GetNodeMenuName(Type type) {
      if (type.Namespace.StartsWith("GameCore.BehaviorFuncs")) {
        return base.GetNodeMenuName(type);
      } else return null;
    }

    protected override bool CheckAddNode(Type type) {
      if (typeof(Root).IsAssignableFrom(type)) {
        var root = target.nodes.Find(node => node is Root);
        if (root) {
          EditorUtility.DisplayDialog("提示", $"已添加根节点{root.GetType().Name},请勿重复添加!", "确定");
          return false;
        }
      }
      if (typeof(Init).IsAssignableFrom(type)) {
        var init = target.nodes.Find(node => node is Init);
        if (init) {
          EditorUtility.DisplayDialog("提示", $"已添加初始化节点{init.GetType().Name},请勿重复添加!", "确定");
          return false;
        }
      }
      return true;
    }
  }
}