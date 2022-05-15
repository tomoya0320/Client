using Battle;

public class Game : SingletonMono<Game> {
  #region ’Ω∂∑≤‚ ‘¬ﬂº≠
  public BattleData BattleData;
  public BehaviorTree.Battle.BehaviorGraph BehaviorGraph;

  private async void Start() {
    // BattleManager.Enter(BattleData);
    BehaviorGraph.Init(BattleManager.Instance);
    await BehaviorGraph.Run();
  }
  #endregion
}