namespace Battle {
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
}