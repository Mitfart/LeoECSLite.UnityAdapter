﻿using System.Reflection;
using Leopotam.EcsLite;

namespace Mitfart.LeoECSLite.UnityAdapter.Plugins.Mitfart.LeoECSLite.UnityAdapter.Runtime.Extensions.Ecs.World {
  public static class GetPoolExt {
    private static readonly MethodInfo _Get_Pool_Method_Info = typeof(EcsWorld).GetMethod(nameof(EcsWorld.GetPool));


    public static IEcsPool GetPool(this EcsWorld world, System.Type type) {
      IEcsPool pool = world.GetPoolByType(type);

      if (pool != null)
        return pool;

      MethodInfo getPool = _Get_Pool_Method_Info.MakeGenericMethod(type);
      pool = (IEcsPool) getPool.Invoke(world, null);

      return pool;
    }
  }
}