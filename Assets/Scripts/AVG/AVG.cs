using DG.Tweening;
using GameCore.AVGFuncs;
using GameCore.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore {
  public class AVG {
    public int Block;
    public AVGNode AVGNode;
    public List<Tween> Tweens = new List<Tween>();
    public UIAVG UI { get; private set; }
    public AVGGraph AVGGraph { get; private set; }
    public static AVG Instance { get; private set; }

    public static bool Enter(AVGGraph avgGraph) {
      if (Instance != null) {
        Debug.LogError("上一个AVG还在播放中!");
        return false;
      }
      EnterInternal(avgGraph);
      return true;
    }

    private static async void EnterInternal(AVGGraph avgGraph) => await UIManager.Instance.Open<UIAVG>(UIType.NORMAL, "UIAVG", avgGraph);

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

      Instance = null;
      UI = null;
      AVGNode = null;
      Block = 0;
    }
  }
}