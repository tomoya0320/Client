using Cysharp.Threading.Tasks;
using System;

namespace GameCore {
  public class Buff : IPoolObject {
    public int RuntimeId { get; private set; }
    public BuffComponent BuffComponent { get; private set; }
    public BuffTemplate BuffTemplate { get; private set; }
    public BuffContext BuffContext { get; private set; }
    public string MagicId => BuffTemplate.Magic?.AssetGUID;
    public Unit Source { get; private set; }
    public Unit Target { get; private set; }
    public int Turn { get; private set; }

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
        string magicId = ++Turn == BuffTemplate.Delay ? BuffTemplate.Magic?.AssetGUID : BuffTemplate.IntervalMagic?.AssetGUID;
        await Target.Battle.MagicManager.DoMagic(magicId, Source, Target, BuffContext);
      }

      return BuffTemplate.Duration < 0 || Turn < BuffTemplate.TotalDuration;
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