using System.Reflection;

public static class AssemblyUtil {
  private static Assembly Assembly = Assembly.GetExecutingAssembly();

  public static T CreateInstance<T>(string typeName) where T : class {
    return Assembly.CreateInstance(typeName) as T;
  }
}