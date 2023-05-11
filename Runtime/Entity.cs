using System;
using System.Collections.Generic;
using LeoECSLite.UnityAdapter.WorldsLocator;
using Leopotam.EcsLite;
using UnityEngine;

namespace LeoECSLite.UnityAdapter {
  [DisallowMultipleComponent]
  public class Entity : MonoBehaviour {
    [SerializeReference] public List<ComponentAdapter> components = new();

    public string        worldName;
    public EntityConvert entityConvert = EntityConvert.OnAwake;

    public EcsPackedEntityWithWorld PackedEntity { get; private set; }
    public bool                     IsConverted  { get; private set; }



    protected virtual void Awake() {
      if (entityConvert == EntityConvert.OnAwake)
        Convert();
    }

    protected virtual void Start() {
      if (entityConvert == EntityConvert.OnStart)
        Convert();
    }



    public EcsPackedEntityWithWorld Convert() => Convert(EcsWorldsLocator.Get(worldName));

    public EcsPackedEntityWithWorld Convert(EcsWorld world) {
      if (IsConverted)
        return PackedEntity;

      PackedEntity = CreateEntity(world);
      IsConverted  = true;

      return PackedEntity;
    }



    private EcsPackedEntityWithWorld CreateEntity(EcsWorld world) {
      int e = world.NewEntity();
      AddComponents(e, world);
      OnCreateEntity(e, world);
      return world.PackEntityWithWorld(e);
    }



    public void AddAdapter(Type componentType) {
      object component = Activator.CreateInstance(componentType);
      components.Add(new ComponentAdapter(component));
    }



    protected virtual void OnCreateEntity(int e, EcsWorld world) { }

    protected void AddComponents(int e, EcsWorld world) {
      foreach (ComponentAdapter component in components) {
        ThrowIfBroken(component);
        component.AddTo(e, world);
      }
    }



    private void ThrowIfBroken(ComponentAdapter component) {
#if UNITY_EDITOR
      if (component.Broken)
        throw new Exception($"Entity has broken component! | on \"{name}\"");
#endif
    }
  }
}