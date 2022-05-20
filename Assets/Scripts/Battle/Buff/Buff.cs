namespace Battle {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffData BuffData { get; private set; }
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }

    public bool Init(int runtimeId, BuffComponent buffComponent, BuffData buffData, Unit source, Unit target) {
      RuntimeId = runtimeId;
      BuffComponent = buffComponent;
      BuffData = buffData;
      Source = source;
      Target = target;

      return true;
    }

    public void Release() {

    }
  }
}