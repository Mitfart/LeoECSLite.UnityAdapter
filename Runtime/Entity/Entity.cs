﻿using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Mitfart.LeoECSLite.UnityAdapter {
  [DisallowMultipleComponent]
  public class Entity : MonoBehaviour {
    [SerializeReference] public List<IComponentsAdapter> components = new();

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
#if UNITY_EDITOR
      if (components.Count <= 0)
        throw new Exception("Can't Create Empty Entity!");
#endif

      int e = world.NewEntity();
      AddComponents(e, world);
      OnCreateEntity(e, world);
      return world.PackEntityWithWorld(e);
    }



    public void Add(Type componentType) {
      object component = Activator.CreateInstance(componentType);
      components.Add(new OneComponentsAdapter(component));
    }



    protected virtual void OnCreateEntity(int e, EcsWorld world) { }

    protected void AddComponents(int e, EcsWorld world) {
      foreach (IComponentsAdapter component in components) {
        ThrowIfBroken(component);
        component.AddTo(e, world);
      }
    }



    private void ThrowIfBroken(IComponentsAdapter components) {
#if UNITY_EDITOR
      if (components.Broken)
        throw new Exception($"Entity has broken component! | on \"{name}\"");
#endif
    }
  }
}