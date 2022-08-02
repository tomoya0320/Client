using DG.Tweening;
using System;

namespace GameCore.UI {
  public interface IUIAVG {
    void SetOptions(string[] options, Action<int> callback);
    Tween SetDialogue(string name, string dialogue, float fadeTime, bool setSpeedBased);
  }
}