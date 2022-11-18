using Sirenix.OdinInspector;
using System;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [Serializable]
  public class CardData {
    public AssetReferenceT<CardTemplate> Template;
    [LabelText("µÈ¼¶")]
    public int Lv;
  }
}