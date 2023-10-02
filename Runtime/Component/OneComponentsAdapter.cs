using System;
using Leopotam.EcsLite;
using Mitfart.LeoECSLite.UnityAdapter.Extensions;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityAdapter {
  [Serializable]
  public class OneComponentsAdapter : ComponentsAdapter {
    [SerializeReference] private object component;

    public override bool   Broken => RawComponent == null;
    public override string Name   => ComponentType.Name;
    public override Type[] Types  { get; } = new Type[1];

    public object RawComponent {
      get => component;
      set {
        ThrowIfNotCompatible(value);
        component = value;
      }
    }

    public Type ComponentType {
      get => RawComponent?.GetType();
      private set {
        ThrowIfNotCompatible(value);
        Types[0] = value;
        RefreshDebugMsg(value);
      }
    }


    public OneComponentsAdapter(object component) {
      base.ThrowIfNotCompatible(component);
      this.component = component;
    }



    protected override void Add(int e, EcsWorld world) {
      IEcsPool pool = world.GetPool(ComponentType);

      if (pool.Has(e))
        pool.Del(e);

      pool.AddRaw(e, RawComponent);
    }



    private void RefreshDebugMsg(Type newComponentType) {
#if UNITY_EDITOR
      DebugMsg = newComponentType.Name;
#endif
    }

    protected override void ThrowIfNotCompatible(object newComponent) {
#if UNITY_EDITOR
      if (newComponent.GetType() != ComponentType)
        throw new Exception($"{nameof(OneComponentsAdapter)}: Incorrect type! \n (type: {newComponent.GetType().Name} | required: {ComponentType.Name})");
#endif
    }
  }
}