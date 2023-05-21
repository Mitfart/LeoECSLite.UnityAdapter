using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Runtime.WorldsLocator;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Runtime.Extensions.Ecs.Systems {
  public static class RegisterWorldsExt {
    public static IEcsSystems RegisterWorlds(this IEcsSystems systems) {
      EcsWorldsLocator.RegisterAllWorlds(systems);
      return systems;
    }
  }
}