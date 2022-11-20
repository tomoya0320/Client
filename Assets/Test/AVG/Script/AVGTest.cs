using GameCore.UI;
using UnityEngine;

namespace GameCore.Test {
  public class AVGTest : MonoBehaviour {
    [SerializeField]
    private AVGGraph AVGGraph;

    private async void Start() {
      await UIManager.Instance.Open<UIAVG>(UIType.NORMAL, "UIAVG", AVGGraph);
    }
  }
}