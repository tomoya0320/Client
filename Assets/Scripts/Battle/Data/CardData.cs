using Sirenix.OdinInspector;
using System;
using UnityEngine.AddressableAssets;

namespace GameCore {
  [Serializable]
  public class CardData {
    public AssetReferenceT<CardTemplate> Template;
    [LabelText("�ȼ�")]
    public int Lv;
  }
}