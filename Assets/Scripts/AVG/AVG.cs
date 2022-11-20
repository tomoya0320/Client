using DG.Tweening;
using GameCore.AVGFuncs;
using GameCore.UI;
using System.Collections.Generic;

namespace GameCore {
  public class AVG {
    public int Block;
    public AVGNode AVGNode;
    public List<Tween> Tweens = new List<Tween>();
    public UIAVG UI { get; private set; }
    public AVGGraph AVGGraph { get; private set; }

    public AVG(UIAVG ui, AVGGraph avgGraph) {
      UI = ui;
      AVGGraph = avgGraph;
      AVGNode = AVGGraph.GetEnterNode();
    }

    public void Run() => AVGGraph.Run(this);

    public void Clear() {
      if (Tweens.Count > 0) {
        foreach (var tween in Tweens) {
          tween?.Kill();
        }
        Tweens.Clear();
      }

      UI = null;
      AVGNode = null;
      Block = 0;
    }
  }
}