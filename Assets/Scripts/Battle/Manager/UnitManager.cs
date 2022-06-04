using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameCore {
  public class UnitManager : TemplateManager<UnitTemplate> {
    private int IncId;
    private Dictionary<int, Unit> Units = new Dictionary<int, Unit>();

    public UnitManager(Battle battle) : base(battle) { }

    public async UniTask<Unit> Create(Player player, UnitData unitData) {
      var unit = new Unit(Battle, ++IncId, player, unitData);
      await unit.Init();
      Units.Add(unit.RuntimeId, unit);
      Battle.BuffManager.AddComponent(unit);
      return unit;
    }

    public Unit GetUnit(int runtimeId) {
      if (Units.TryGetValue(runtimeId, out Unit unit)) {
        return unit;
      }
      return null;
    }

    public void OnUnitDie(Unit unit) {
      if(!Units.ContainsKey(unit.RuntimeId)){
        return;
      }
      Battle.BuffManager.RemoveComponent(unit.RuntimeId);
      unit.Player.DeadUnitCount++;
    }
  }
}