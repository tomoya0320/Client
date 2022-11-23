using GameCore.UI;
using UnityEngine;

namespace GameCore.Test {
  public class AVGTest : MonoBehaviour {
    [SerializeField]
    private AVGGraph AVGGraph;

    private void Awake() => UIMain.OnStart += () => AVG.Enter(AVGGraph);
  }
}