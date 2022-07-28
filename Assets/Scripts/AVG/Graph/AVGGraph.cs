using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace GameCore.AVG {
  [CreateAssetMenu(menuName = "模板/AVG/剧情通用")]
  public class AVGGraph : NodeGraph {
    [NonSerialized]
    public int Block;
    [NonSerialized]
    public AVGNode AVGNode;
    [NonSerialized]
    public List<Tween> Tweens = new List<Tween>();

    public void Enter() {
      var node = nodes.Find(n => n is Enter);
      (node as AVGNode)?.Run();
    }

    public void Run() {
      if (Block > 0) {
        return;
      }

      if (Tweens.Count > 0) {
        while (Tweens.Count > 0) {
          Tweens.First().Complete();
        }
        return;
      }

      AVGNode?.Run();
    }

    public void CleanUp() {
      if (Tweens.Count > 0) {
        foreach (var tween in Tweens) {
          tween?.Kill();
        }
        Tweens.Clear();
      }

      AVGNode = null;
      Block = 0;
    }
  }
}