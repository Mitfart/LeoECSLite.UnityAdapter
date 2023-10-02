using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityAdapter.Extensions {
  public static class RegisterWorldsExt {
    public static IEcsSystems RegisterWorlds(this IEcsSystems systems) {
      EcsWorldsLocator.RegisterAllWorlds(systems);
      return systems;
    }
  }
}