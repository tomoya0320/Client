using GameCore.UI;
using UnityEngine;

namespace GameCore.Test {
  public class AVGTest : MonoBehaviour {
    [SerializeField]
    private AVGGraph AVGGraph;

    private void Start() {
      AVG.Enter(AVGGraph);
    }
  }
}