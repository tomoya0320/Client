using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore {
  public abstract class BattleOperation : IPoolObject {
    public Unit Unit;

    public abstract UniTask DoOperation();
    public virtual void Release() { 
      Unit = null;
    }
  }

  public class PlayCardOp : BattleOperation {
    public Card Card;
    public Unit MainTarget;

    public override async UniTask DoOperation() {
      Unit.Battle.UIBattle.ShowText(Card.CardTemplate.name, Unit.UIUnit.BattleTextNode.position, Color.black, false);
      await Card.Cast(MainTarget);
      await Unit.BattleCardControl.OnPlayedCard(Card);
    }

    public override void Release() {
      base.Release();
      Card = null;
      MainTarget = null;
    }
  }
}