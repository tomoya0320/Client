using System.Collections.Generic;

namespace GameCore {
  public class UnitManager : TemplateManager<UnitTemplate> {
    private int IncId;
    private Dictionary<int, Unit> Units = new Dictionary<int, Unit>();

    public UnitManager(Battle battle) : base(battle) {
      
    }

    public Unit Create(UnitData unitData) {
      var unit = new Unit(Battle);
      return unit.Init(++IncId, unitData);
    }

    public Unit GetUnit(int runtimeId) {
      Units.TryGetValue(runtimeId, out Unit unit);
      return unit;
    }

    public void OnUnitDie(int runtimeId) {
      if(!Units.TryGetValue(runtimeId, out var unit)){
        return;
      }

      Units.Remove(runtimeId);
      Battle.BuffManager.RemoveComponent(runtimeId);
    }
  }
}