using DG.Tweening;
using System;

namespace GameCore.UI {
  public interface IUIAVG {
    void SetOption(int index, string option, Action callback);
    Tween SetDialogue(string name, string dialogue, float fadeTime, bool setSpeedBased);
  }
}