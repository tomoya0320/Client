using GameCore.BehaviorFuncs;
using System;
using System.Collections.Generic;
using UnityEditor;
using XNodeEditor;

namespace GameCore {
  [CustomNodeGraphEditor(typeof(BehaviorGraph))]
  public class BehaviorGraphEditor : NodeGraphEditor {
    private static HashSet<Type> UniqueTypes = new HashSet<Type> {
      typeof(Root),
      typeof(Init),
      typeof(Finalize),
    };

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
      if (UniqueTypes.Contains(type) && target.nodes.Find(node => node.GetType() == type)) {
        EditorUtility.DisplayDialog("提示", $"已添加{type.Name}节点,请勿重复添加!", "确定");
        return false;
      }
      return true;
    }
  }
}