using System;
using System.Collections.Generic;
using Mitfart.LeoECSLite.UnityAdapter.Attributes;
using UnityEditor;

namespace Mitfart.LeoECSLite.UnityAdapter.Editor.Search {
  public static class ComponentsDatabase {
    private static readonly List<Type> _SerializableComponents = new();

    public static IReadOnlyCollection<Type> SerializableComponents => _SerializableComponents;



    static ComponentsDatabase() {
      foreach (Type component in TypeCache.GetTypesWithAttribute<EcsComponentAttribute>())
        _SerializableComponents.Add(component);
    }
  }
}