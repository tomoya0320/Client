using System.Collections.Generic;

namespace GameCore {
  public class UnitManager : TemplateManager<UnitTemplate> {
    private int IncId;
    private Dictionary<int, Unit> Units = new Dictionary<int, Unit>();

    public UnitManager(Battle battle) : base(battle) {
      
    }

    public Unit Create(Player player, UnitData unitData) {
      var unit = new Unit(Battle);
      unit.Init(++IncId, player, unitData);
      Units.Add(unit.RuntimeId, unit);
      return unit;
    }

    public Unit GetUnit(int runtimeId) {
      if (Units.TryGetValue(runtimeId, out Unit unit)) {
        return unit;
      }
      return null;
    }

    public void OnUnitDie(int runtimeId) {
      if(!Units.TryGetValue(runtimeId, out var unit)){
        return;
      }
      Battle.BuffManager.RemoveComponent(runtimeId);
      unit.Player.DeadUnitCount++;
    }
  }
}