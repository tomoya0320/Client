using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace Battle.BehaviorFuncs {
  [CustomNodeEditor(typeof(BehaviorNode))]
  public class BehaviorNodeEditor : NodeEditor {
    public override void OnHeaderGUI() {
      base.OnHeaderGUI();

      var behaviorNode = target as BehaviorNode;
      if (behaviorNode && behaviorNode.Index >= 0) {
        GUI.Label(GUILayoutUtility.GetLastRect(), $"{behaviorNode.Index}", NodeEditorResources.styles.indexHeader);
      }
    }
  }
}