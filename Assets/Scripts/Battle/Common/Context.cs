namespace GameCore {
  public abstract class Context : IPoolObject {
    public abstract void Release();
  }

  public class BehaviorContext : Context {
    public Behavior Behavior;

    public override void Release() {
      Behavior = null;
    }
  }

  public class BuffContext : Context {
    public Buff Buff;
    public Behavior Behavior;
    public int AttribValue;

    public override void Release() {
      Buff = null;
      Behavior = null;
      AttribValue = 0;
    }
  }

  public class DamageContext : Context {
    public Unit Source;
    public Unit Target;
    public int DamageValue;

    public override void Release() {
      Source = null;
      Target = null;
      DamageValue = 0;
    }
  }
}