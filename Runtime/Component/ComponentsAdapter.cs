using System;
using Leopotam.EcsLite;
using Unity.VisualScripting;

namespace Mitfart.LeoECSLite.UnityAdapter {
  public abstract class ComponentsAdapter : IComponentsAdapter {
#if UNITY_EDITOR
    public          string DebugMsg { get; protected set; }
    public abstract bool   Broken   { get; }
#endif

    public abstract string Name  { get; }
    public abstract Type[] Types { get; }



    public void AddTo(int e, EcsWorld world) {
      BeforeAdd();
      Add(e, world);
      AfterAdd();
    }

    protected virtual  void BeforeAdd() { }
    protected abstract void Add(int e, EcsWorld world);
    protected virtual  void AfterAdd() { }



    protected virtual void ThrowIfNotCompatible(object newComponent) {
#if UNITY_EDITOR
      if (newComponent == null)
        throw new Exception($"{nameof(OneComponentsAdapter)}: Component is NULL!");

      if (!newComponent.GetType().IsStruct())
        throw new Exception($"{nameof(OneComponentsAdapter)}: Component not a Struct!");
#endif
    }
  }
}