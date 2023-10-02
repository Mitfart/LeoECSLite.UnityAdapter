using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityAdapter {
  public static class EcsWorldsLocator {
    private static readonly Dictionary<string, EcsWorld> _Worlds = new();



    public static EcsWorld Get(string worldName) {
#if UNITY_EDITOR
      if (!_Worlds.ContainsKey(ValidWorldName(worldName)))
        throw new Exception($"Can't find World nameof: {worldName}! \n You forgot to register it?");
#endif
      return _Worlds[ValidWorldName(worldName)];
    }



    public static void RegisterAllWorlds(IEcsSystems systems) {
      RegisterDefaultWorld(systems);
      RegisterAllNamedWorlds(systems);
    }


    public static void RegisterDefaultWorld(IEcsSystems systems) {
      RegisterWorld(null, systems.GetWorld());
    }

    public static void RegisterAllNamedWorlds(IEcsSystems systems) {
      foreach ((string name, EcsWorld world) in systems.GetAllNamedWorlds())
        RegisterWorld(name, world);
    }


    public static void RegisterWorld(string worldName, EcsWorld world) {
      _Worlds[ValidWorldName(worldName)] = world;
    }



    private static string ValidWorldName(string worldName) {
      // Dictionary can't handle 'Null' as the key
      if (string.IsNullOrWhiteSpace(worldName))
        worldName = string.Empty;

      return worldName;
    }
  }
}