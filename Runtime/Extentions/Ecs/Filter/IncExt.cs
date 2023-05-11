using System;
using System.Reflection;
using Leopotam.EcsLite;

namespace LeoECSLite.UnityAdapter.Extentions.Ecs.Filter {
  public static class IncExt {
    private static readonly MethodInfo _Filter_Inc_Method_Info = typeof(EcsWorld.Mask).GetMethod(nameof(EcsWorld.Mask.Inc));


    public static EcsWorld.Mask Inc(this EcsWorld.Mask mask, Type type) {
      MethodInfo method = _Filter_Inc_Method_Info.MakeGenericMethod(type);
      return (EcsWorld.Mask) method.Invoke(mask, null);
    }
  }
}