using DG.Tweening;
using System.Linq;
using UnityEngine;
using XNode;

namespace GameCore {
  [CreateAssetMenu(menuName = "模板/AVG/剧情通用")]
  public class AVGGraph : NodeGraph {
    public AVGNode GetEnterNode() => nodes.Find(n => n is Enter) as AVGNode;

    public void Run(AVG avg) {
      if (avg.Block > 0) {
        return;
      }

      if (avg.Tweens.Count > 0) {
        while (avg.Tweens.Count > 0) {
          avg.Tweens.First().Complete();
        }
        return;
      }

      avg.AVGNode?.Run(avg);
    }
  }
}