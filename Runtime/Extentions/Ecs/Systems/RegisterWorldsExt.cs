using LeoECSLite.UnityAdapter.WorldsLocator;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityAdapter.Extentions.Ecs.Systems {
  public static class RegisterWorldsExt {
    public static IEcsSystems RegisterWorlds(this IEcsSystems systems) {
      EcsWorldsLocator.RegisterAllWorlds(systems);
      return systems;
    }
  }
}