using System;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityAdapter {
  public interface IComponentsAdapter {
#if UNITY_EDITOR
    string DebugMsg { get; }
    bool   Broken   { get; }
#endif

    string Name  { get; }
    Type[] Types { get; }

    void AddTo(int e, EcsWorld world);
  }
}