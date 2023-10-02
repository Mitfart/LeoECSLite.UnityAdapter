namespace Mitfart.LeoECSLite.UnityAdapter.Editor.Extensions {
  public static class BrokenMsgExt {
    public static string BrokenMsg(this IComponentsAdapter adapter)
      => "Broken"
       + (string.IsNullOrWhiteSpace(adapter.DebugMsg)
           ? string.Empty
           : $" | {adapter.DebugMsg}");
  }
}