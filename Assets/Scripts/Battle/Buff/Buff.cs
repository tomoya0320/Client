namespace GameCore {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffTemplate BuffTemplate { get; private set; }
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }
    private int Turn;

    public bool Init(int runtimeId, BuffComponent buffComponent, BuffTemplate buffTemplate, Unit source, Unit target) {
      RuntimeId = runtimeId;
      BuffComponent = buffComponent;
      BuffTemplate = buffTemplate;
      Source = source;
      Target = target;
      Turn = 0;

      return true;
    }

    public bool UpdateTurn() => ++Turn < BattleConstant.TURN_PHASE_COUNT * BuffTemplate.Duration;

    public void Release() {
      RuntimeId = 0;
      BuffComponent = null;
      BuffTemplate = null;
      Source = null;
      Target = null;
      Turn = 0;
    }
  }
}