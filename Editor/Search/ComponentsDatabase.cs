using System;
using System.Collections.Generic;
using LeoECSLite.UnityAdapter.Attributes;
using UnityEditor;

namespace LeoECSLite.UnityAdapter.Editor.Search {
  public static class ComponentsDatabase {
    private static readonly List<Type> _SerializableComponents = new();

    public static IReadOnlyCollection<Type> SerializableComponents => _SerializableComponents;



    static ComponentsDatabase() {
      foreach (Type component in TypeCache.GetTypesWithAttribute<EcsComponentAttribute>())
        _SerializableComponents.Add(component);
    }
  }
}