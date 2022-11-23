using System.Collections.Generic;
using UnityEngine;

namespace GameCore.UI {
  public class UICardHeap : UIBase {
    [SerializeField]
    private DynamicScrollRect DynamicScrollRect;
    private List<Card> CardList;

    public override UIBase Init(params object[] args) {
      CardList = args[0] as List<Card>;
      DynamicScrollRect.Init<UICardGrid, Card>(CardList);
      return base.Init(args);
    }

    public async void Close() {
      bool closed = IsChildUI ? await UIManager.Instance.CloseChild(ParentUI, this) : await UIManager.Instance.Close(this);
      if (closed) {
        TempList<Card>.Release(CardList);
        CardList = null;
      }
    }
  }
}