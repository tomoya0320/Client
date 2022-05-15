using System;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace BehaviorTree.Battle {
  [CustomNodeGraphEditor(typeof(BehaviorGraph))]
  public class BehaviorGraphEditor : NodeGraphEditor {
    public override void OnOpen() {
      window.titleContent = new GUIContent("��Ϊ���༭��");
    }

    /// <summary> 
    /// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
    /// In this example we are sorting out all node types that are not in the XNode.Examples namespace.
    /// </summary>
    public override string GetNodeMenuName(Type type) {
      if (type.Namespace == "BehaviorTree.Battle") {
        return base.GetNodeMenuName(type);
      } else return null;
    }

    public override void OnGUI() {
      var behaviorGraph = target as BehaviorGraph;
      GUI.color = Color.green;
      GUILayout.Label($"Id:{behaviorGraph.BehaviorId}");
    }

    protected override bool CheckAddNode(Type type) {
      if (typeof(Root).IsAssignableFrom(type)) {
        var root = target.nodes.Find(node => node is Root);
        if (root) {
          EditorUtility.DisplayDialog("��ʾ", $"����Ӹ��ڵ�{root.GetType().Name},�����ظ����!", "ȷ��");
          return false;
        }
      }
      return true;
    }
  }
}