using Cysharp.Threading.Tasks;

namespace GameCore {
  public abstract class BattleOperation : IPoolObject {
    public Unit Unit;

    public abstract UniTask DoOperation();
    public virtual void Release() { 
      Unit = null;
    }
  }

  public class EndTurnOp : BattleOperation {
    public override UniTask DoOperation() {
      Unit.Player.EndTurn();
      return UniTask.CompletedTask;
    }
  }

  public class PlayCardOp : BattleOperation {
    public Card Card;
    public Unit MainTarget;

    public override async UniTask DoOperation() => await Card.Cast(MainTarget);

    public override void Release() {
      base.Release();
      Card = null;
      MainTarget = null;
    }
  }
}