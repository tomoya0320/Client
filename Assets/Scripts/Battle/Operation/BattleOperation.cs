using Cysharp.Threading.Tasks;

namespace GameCore {
  public abstract class BattleOperation : BattleBase, IPoolObject {
    protected Unit Unit;

    protected BattleOperation(Battle battle) : base(battle) { }
    public abstract UniTask DoOperation();
    public virtual void Release() { }
  }

  public class EndTurnOp : BattleOperation {
    public EndTurnOp(Battle battle) : base(battle) { }

    public override UniTask DoOperation() {
      Unit.Player.EndTurn();
      return UniTask.CompletedTask;
    }
  }
}