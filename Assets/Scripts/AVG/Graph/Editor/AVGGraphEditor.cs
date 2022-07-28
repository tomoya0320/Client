using System;
using System.Collections.Generic;
using UnityEditor;
using XNodeEditor;

namespace GameCore.AVG {
  [CustomNodeGraphEditor(typeof(AVGGraph))]
  public class AVGGraphEditor : NodeGraphEditor {
    private static HashSet<Type> UniqueTypes = new HashSet<Type> {
      typeof(Enter),
    };

    public override void OnGUI() {
      window.titleContent.text = target.name;
    }

    /// <summary> 
    /// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
    /// In this example we are sorting out all node types that are not in the XNode.Examples namespace.
    /// </summary>
    public override string GetNodeMenuName(Type type) {
      if (type.Namespace.StartsWith("GameCore.AVG")) {
        return base.GetNodeMenuName(type);
      } else return null;
    }

    protected override bool CheckAddNode(Type type) {
      if (UniqueTypes.Contains(type) && target.nodes.Find(node => node.GetType() == type)) {
        EditorUtility.DisplayDialog("��ʾ", $"�����{type.Name}�ڵ�,�����ظ����!", "ȷ��");
        return false;
      }
      return true;
    }
  }
}