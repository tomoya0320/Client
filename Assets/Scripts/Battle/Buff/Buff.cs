using Cysharp.Threading.Tasks;

namespace GameCore {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffTemplate BuffTemplate { get; private set; }
    public BuffContext BuffContext { get; private set; }
    public string MagicId => BuffTemplate.MagicId;
    public string IntervalMagicId => BuffTemplate.IntervalMagicId;
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }
    private int Turn;

    public void Init(int runtimeId, BuffComponent buffComponent, BuffTemplate buffTemplate, Unit source, Unit target) {
      RuntimeId = runtimeId;
      BuffComponent = buffComponent;
      BuffTemplate = buffTemplate;
      Source = source;
      Target = target;
      Turn = 0;
      BuffContext = Target.Battle.ObjectPool.Get<BuffContext>();
      BuffContext.Buff = this;
    }

    public async UniTask<bool> Update(TickTime updateTime) {
      if (updateTime == BuffTemplate.UpdateTime) {
        await Target.Battle.MagicManager.DoMagic(IntervalMagicId, Source, Target, BuffContext);
        Turn++;
      }
      return BuffTemplate.Duration < 0 || Turn < BuffTemplate.Duration;
    }

    public void Release() {
      Target.Battle.ObjectPool.Release(BuffContext);
      BuffContext = null;
      BuffComponent = null;
      BuffTemplate = null;
      Source = null;
      Target = null;
      RuntimeId = 0;
      Turn = 0;
    }
  }
}