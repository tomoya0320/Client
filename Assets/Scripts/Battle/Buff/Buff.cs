namespace Battle {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffTemplate BuffTemplate { get; private set; }
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }

    public bool Init(int runtimeId, BuffComponent buffComponent, BuffTemplate buffTemplate, Unit source, Unit target) {
      RuntimeId = runtimeId;
      BuffComponent = buffComponent;
      BuffTemplate = buffTemplate;
      Source = source;
      Target = target;

      return true;
    }

    public void Release() {

    }
  }
}